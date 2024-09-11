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
            Entity projectileEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(projectileEntity, new ProjectileComponent
            {
                DeathTimer = 0,
                DeathTimer2 = authoring.TTK,
                ProjectileMoveSpeed = authoring.ProjectileSpeed,
                ColliderSize = authoring.ProjectileColliderSize,
                projectileEntity = projectileEntity,
            });
        }
    }
}




