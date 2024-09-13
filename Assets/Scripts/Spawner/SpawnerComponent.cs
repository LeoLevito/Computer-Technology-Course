using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerComponent : IComponentData
{
    public Entity EnemyPrefab;
    public float2 SpawnPosition; //better vector2
    public float SpawnRate;
    public float NextSpawnTime;
    public Entity SpawnerEntity;
}
