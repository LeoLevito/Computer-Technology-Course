using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct FireProjectileSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (projectilePrefab, transform) in SystemAPI.Query<ProjectilePrefab, LocalTransform>().WithAll<FireProjectileTag>())
        {
            var newProjectile = ecb.Instantiate(projectilePrefab.Value);
            var projectileTransform = LocalTransform.FromPositionRotation(transform.Position, transform.Rotation);
            ecb.SetComponent(newProjectile, projectileTransform);

        }

        new ProjectileKillJob
        {
            DeltaTime = deltaTime,
            ecb = ecb,
        }.Schedule(); //whenever we have an available thread where gonna run it.
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose(); 
    }
}


[BurstCompile]
public partial struct ProjectileKillJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref ProjectileEntity entity, ref ProjectileDeathTimer deathTimer) //ref is very important here, without it I can't update the death timer values for specific enemies!
    {
        deathTimer.Value += DeltaTime;
        // Debug.Log(deathTimer.Value.ToString());
        if (deathTimer.Value >= deathTimer.Value2)
        {
            ecb.DestroyEntity(entity.Value);
        }
    }
}
