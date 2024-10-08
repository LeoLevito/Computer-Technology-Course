using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    public float ProjectileSpeed;
    public float TimeToKill;

    public class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            Entity projectileEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(projectileEntity, new ProjectileComponent
            {
                MoveSpeed = authoring.ProjectileSpeed,
                DeathTimer = 0,
                TimeToKill = authoring.TimeToKill,
                ProjectileEntity = projectileEntity,
            });
        }
    }
}




