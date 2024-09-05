using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float DeathTimer;

    class EnemyAuthoringBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(enemyEntity, new EnemyMoveSpeed
            {
                Value = authoring.MoveSpeed,
            });
            AddComponent(enemyEntity, new EnemyDeathTimer
            {
                Value = authoring.DeathTimer,
            });
        }
    }
}

public struct EnemyMoveSpeed : IComponentData
{
    public float Value;
}

public struct EnemyDeathTimer : IComponentData
{
    public float Value;
}