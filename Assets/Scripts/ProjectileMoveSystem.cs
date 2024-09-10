using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct ProjectileMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.TempJob);

        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach(var (transform, moveSpeed, colliderSize, projectileEntity) in SystemAPI.Query<RefRW<LocalTransform>, ProjectileMoveSpeed, ProjectileColliderSize, ProjectileEntity> ()) //make this a job
        {
            transform.ValueRW.Position += transform.ValueRO.Up() * moveSpeed.Value * deltaTime;




            //physics, make job todo!
            NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
            float3 point1 = new float3(transform.ValueRO.Position - transform.ValueRO.Right() * 0.15f);
            float3 point2 = new float3(transform.ValueRO.Position + transform.ValueRO.Right() * 0.15f);
            physicsWorld.CapsuleCastAll(point1, point2, colliderSize.Value / 2, float3.zero, 1f, ref hits, new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayer.Default,
                CollidesWith = (uint)CollisionLayer.Enemy,
            });

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Entity hitEntity = hits[i].Entity;
                    if (state.EntityManager.HasComponent<EnemyEntity>(hitEntity))
                    {
                        EnemyEntity enemyEntity = state.EntityManager.GetComponentData<EnemyEntity>(hitEntity);
                        ECB.DestroyEntity(enemyEntity.Value);
                        ECB.DestroyEntity(projectileEntity.Value);
                    }
                }
            }
            hits.Dispose();
        }
        state.Dependency.Complete();
        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}
