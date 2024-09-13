using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.TempJob);
        float deltaTime = SystemAPI.Time.DeltaTime;
        float elapsedTime = (float)SystemAPI.Time.ElapsedTime;

        new EnemySpawnJob
        {
            ElapsedTime = elapsedTime,
            ecb = ECB,
        }.Schedule();

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
public partial struct EnemySpawnJob : IJobEntity
{
    public float ElapsedTime;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref SpawnerComponent spawner)
    {
        if (spawner.NextSpawnTime < ElapsedTime)
        {
            Entity newEntity = ecb.Instantiate(spawner.EnemyPrefab);
            float3 pos = new float3(spawner.SpawnPosition.x, spawner.SpawnPosition.y, 0);
            ecb.SetComponent(newEntity, LocalTransform.FromPosition(pos));
            spawner.NextSpawnTime = ElapsedTime + spawner.SpawnRate;
        }
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

        if (enemy.DeathTimer >= enemy.TimeToKill)
        {
            ecb.DestroyEntity(enemy.EnemyEntity);
        }
    }
}
