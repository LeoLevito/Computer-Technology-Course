using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float TTK = 3;
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
                Value = 0,
                Value2 = authoring.TTK,
            });

            AddComponent(enemyEntity, new EnemyEntity
            {
                Value = enemyEntity
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
    public float Value2;
}

public struct EnemyEntity : IComponentData
{
    public Entity Value;
}