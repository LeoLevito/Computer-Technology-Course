using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    public float ProjectileSpeed;
    public float TTK;
    public float ProjectileColliderSize;
    public class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileMoveSpeed { Value = authoring.ProjectileSpeed });

            AddComponent(entity, new ProjectileDeathTimer
            {
                Value = 0,
                Value2 = authoring.TTK,
            });
            AddComponent(entity, new ProjectileEntity
            {
                Value = entity
            });

            AddComponent(entity, new ProjectileColliderSize
            {
                Value = authoring.ProjectileColliderSize,
            });
        }
    }
}

public struct ProjectileDeathTimer : IComponentData
{
    public float Value;
    public float Value2;
}

public struct ProjectileEntity : IComponentData
{
    public Entity Value;
}

public struct ProjectileColliderSize : IComponentData
{
    public float Value;
}


