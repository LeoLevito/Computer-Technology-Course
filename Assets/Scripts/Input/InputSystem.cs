using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)] //run in initialization system group, and run it last in that. Happens before main logic group. Check the entity Systems tab in unity editor.
public partial class InputSystem : SystemBase
{
    private GameInput InputActions;

    protected override void OnCreate()
    {
        InputActions = new GameInput();
        InputActions.Enable();
    }

    protected override void OnUpdate()
    {
        Vector2 moveInput = InputActions.GamePlay.Move.ReadValue<Vector2>();
        bool shootInput = InputActions.GamePlay.Shoot.triggered;

        foreach(RefRW<InputComponent> input in SystemAPI.Query<RefRW<InputComponent>>())
        {
            input.ValueRW.MoveInput = moveInput;
            input.ValueRW.ShootInput = shootInput;
        }
    }

    protected override void OnStopRunning()
    {
        InputActions.Disable();
    }
}

