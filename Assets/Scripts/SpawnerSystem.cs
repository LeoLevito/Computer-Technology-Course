using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SpawnerSystem : ISystem //handles the logic, other spawner scripts define the data.
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.TempJob);
        float elapsedTime = (float)SystemAPI.Time.ElapsedTime;

        new SpawnJob
        {
            ElapsedTime = elapsedTime,
            ecb = ECB,
        }.Schedule();

        state.Dependency.Complete();
        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}

[BurstCompile]
public partial struct SpawnJob : IJobEntity
{
    public float ElapsedTime;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref SpawnerComponent spawner)
    {
        if (spawner.NextSpawnTime < ElapsedTime)
        {
            Entity newEntity = ecb.Instantiate(spawner.Prefab);
            float3 pos = new float3(spawner.SpawnPosition.x, spawner.SpawnPosition.y, 0);
            ecb.SetComponent(newEntity, LocalTransform.FromPosition(pos));
            spawner.NextSpawnTime = ElapsedTime + spawner.SpawnRate;
            
        }
    }
}
