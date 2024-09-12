using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


[UpdateInGroup (typeof(InitializationSystemGroup), OrderLast = true)] //run in initialization system group, and run it last in that. happens before main logic group. Check the entity Systems tab in unity editor.
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

