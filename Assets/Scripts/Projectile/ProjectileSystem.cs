using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct ProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.TempJob);

        float deltaTime = SystemAPI.Time.DeltaTime;

        new ProjectileSpawnJob
        {
            ecb = ECB
        }.Schedule();

        new ProjectileMoveJob
        {
            DeltaTime = deltaTime
        }.Schedule();

        new ProjectileCollisionJob
        {
            DeltaTime = deltaTime,
            physicsWorld = physicsWorld,
            ecb = ECB,
        }.Schedule();

        new ProjectileKillJob
        {
            DeltaTime = deltaTime,
            ecb = ECB
        }.Schedule();

        state.Dependency.Complete();
        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}

[BurstCompile]
public partial struct ProjectileSpawnJob : IJobEntity
{
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref LocalTransform transform, in PlayerComponent player, InputComponent input)
    {
        if (input.ShootInput) //only happens once when when pressed.
        {
            var newProjectile = ecb.Instantiate(player.ProjectilePrefab);
            var projectileTransform = LocalTransform.FromPositionRotation(transform.Position, transform.Rotation);
            ecb.SetComponent(newProjectile, projectileTransform);
        }
    }
}

[BurstCompile]
public partial struct ProjectileMoveJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(ref LocalTransform transform, in ProjectileComponent projectile)
    {
        transform.Position += transform.Up() * projectile.MoveSpeed * DeltaTime;
    }
}

[BurstCompile]
public partial struct ProjectileCollisionJob : IJobEntity
{
    public float DeltaTime;
    public PhysicsWorldSingleton physicsWorld;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref LocalTransform transform, ref ProjectileComponent projectile)
    {
        NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
        float3 point1 = new float3(transform.Position - transform.Right() * 0.15f);
        float3 point2 = new float3(transform.Position + transform.Right() * 0.15f);
        physicsWorld.CapsuleCastAll(point1, point2, 0f, float3.zero, 1f, ref hits, new CollisionFilter
        {
            BelongsTo = (uint)CollisionLayer.Default,
            CollidesWith = (uint)CollisionLayer.Enemy, //since we already know this will be an enemy, we don't need to check if it's an enemy later when it hits.
        });

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++) //if I want to have a EnemyHealth type of component, I would use ecb.SetComponent to update value of health on the hitEnemy.
            {
                Entity hitEntity = hits[i].Entity;
                ecb.DestroyEntity(hitEntity);
            }
            ecb.DestroyEntity(projectile.ProjectileEntity);
        }
        hits.Dispose();
    }
}

[BurstCompile]
public partial struct ProjectileKillJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ecb;

    [BurstCompile]
    private void Execute(ref ProjectileComponent projectile)
    {
        projectile.DeathTimer += DeltaTime;
        if (projectile.DeathTimer >= projectile.TimeToKill)
        {
            ecb.DestroyEntity(projectile.ProjectileEntity);
        }
    }
}
