using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float TimeToKill = 6;
    class EnemyAuthoringBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(enemyEntity, new EnemyComponent
            {
                MoveSpeed = authoring.MoveSpeed,
                DeathTimer = 0,
                TimeToKill = authoring.TimeToKill,
                EnemyEntity = enemyEntity,
            });
        }
    }
}