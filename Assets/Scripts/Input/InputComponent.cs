using Unity.Entities;
using Unity.Mathematics;

public struct InputComponent : IComponentData
{
    public float2 MoveInput;
    public bool ShootInput;
}
