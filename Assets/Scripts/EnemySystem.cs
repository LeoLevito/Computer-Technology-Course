using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemySystem : ISystem
{
    float KillTimer;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.TempJob);
        float deltaTime = SystemAPI.Time.DeltaTime;

        new EnemyMoveJob
        {
            DeltaTime = deltaTime
        }.Schedule(); 

        new EnemyKillJob
        {
            DeltaTime = deltaTime,
            ecb = ECB,
        }.Schedule(); 

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
    private void Execute(ref LocalTransform transform, in EnemyComponent enemy)
    {
        transform.Position.y -= enemy.MoveSpeed * DeltaTime;
    }
}

[BurstCompile]
public partial struct EnemyKillJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref EnemyComponent enemy) //ref is very important here, without it I can't update the death timer values for specific enemies!
    {
        enemy.DeathTimer += DeltaTime;

        if (enemy.DeathTimer >= enemy.DeathTimer2)
        {
            ecb.DestroyEntity(enemy.enemyEntity);
        }
    }
}
