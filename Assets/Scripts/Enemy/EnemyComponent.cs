using Unity.Entities;

public struct EnemyComponent : IComponentData
{
    public float MoveSpeed;
    public float DeathTimer;
    public float TimeToKill;
    public Entity EnemyEntity;
}
