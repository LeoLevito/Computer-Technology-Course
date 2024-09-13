using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float SpawnRate;

    class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new SpawnerComponent
            {
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                SpawnPosition = new float2(authoring.transform.position.x, authoring.transform.position.y), 
                SpawnRate = authoring.SpawnRate,
                NextSpawnTime = 0,
                SpawnerEntity = entity,
            });
        }
    }
}
