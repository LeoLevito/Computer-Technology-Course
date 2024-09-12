using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed;

    public GameObject ProjectilePrefab;

    class PlayerAuthoringBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(playerEntity, new PlayerMoveSpeed
            {
                Value = authoring.MoveSpeed,
            });

            AddComponent(playerEntity, new ProjectilePrefab
            {
                Value = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
            });

            AddComponent<InputComponent>(playerEntity);
        }
    }
}

public struct PlayerMoveSpeed : IComponentData
{
    public float Value;
}

public struct ProjectilePrefab : IComponentData
{
    public Entity Value;
}