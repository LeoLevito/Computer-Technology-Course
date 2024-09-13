using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))] //run before Transform System group, see the Systems window in unity editor.
[BurstCompile]
public partial struct PlayerSystem : ISystem
{
    [BurstCompile] //for job systems, marks code so it uses different compiler (burst complier) compared to normal one. Compiles it down to raw machine code (more performant on the cpu) instead of an intermediary version.
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        new PlayerMoveJob
        {
            DeltaTime = deltaTime
        }.Schedule(); //whenever we have an available thread where gonna run it.
    }
}

[BurstCompile]
public partial struct PlayerMoveJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(ref LocalTransform transform, in InputComponent input, PlayerComponent player)
    {
        transform.Position.xy += input.MoveInput * player.MoveSpeed * DeltaTime;
    }
}
