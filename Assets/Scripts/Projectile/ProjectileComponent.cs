using Unity.Entities;

public struct ProjectileComponent : IComponentData
{
    public float MoveSpeed;
    public float DeathTimer;
    public float TimeToKill;
    public Entity ProjectileEntity;
}
