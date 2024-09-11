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

            AddComponent(enemyEntity, new EnemyComponent
            {
                MoveSpeed = authoring.MoveSpeed,
                DeathTimer = 0,
                DeathTimer2 = authoring.TTK,
                enemyEntity = enemyEntity,
            });
        }
    }
}