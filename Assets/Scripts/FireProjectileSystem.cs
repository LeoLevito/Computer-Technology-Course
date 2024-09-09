using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct FireProjectileSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach(var (projectilePrefab, transform) in SystemAPI.Query<ProjectilePrefab, LocalTransform>().WithAll<FireProjectileTag>())
        {
            var newProjectile = ecb.Instantiate(projectilePrefab.Value); //I believe this causes a considerable stutter
            var projectileTransform = LocalTransform.FromPositionRotation(transform.Position, transform.Rotation);
            ecb.SetComponent(newProjectile, projectileTransform);

        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose(); 
    }
}
