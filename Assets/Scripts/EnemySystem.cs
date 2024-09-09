using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemySystem : ISystem
{
    float KillTimer;
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.TempJob);
        float deltaTime = SystemAPI.Time.DeltaTime;
        float timeElapsed = (float)SystemAPI.Time.ElapsedTime;
        new EnemyMoveJob
        {
            DeltaTime = deltaTime
        }.Schedule(); //whenever we have an available thread where gonna run it.

        new EnemyKillJob
        {
            DeltaTime = deltaTime,
            ecb = ECB,
        }.Schedule(); //whenever we have an available thread where gonna run it.
        state.Dependency.Complete();
        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}

[BurstCompile]
public partial struct EnemyMoveJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(ref LocalTransform transform, in EnemyMoveSpeed speed)
    {
        transform.Position.y -= speed.Value * DeltaTime;
    }
}

[BurstCompile]
public partial struct EnemyKillJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref EnemyEntity entity, ref EnemyDeathTimer deathTimer) //ref is very important here, without it I can't update the death timer values for specific enemies!
    {
        deathTimer.Value += DeltaTime;
       // Debug.Log(deathTimer.Value.ToString());
        if (deathTimer.Value >= deathTimer.Value2)
        {
            ecb.DestroyEntity(entity.Value);
        }
    }
}
