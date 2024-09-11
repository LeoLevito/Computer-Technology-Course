using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;


[UpdateInGroup (typeof(InitializationSystemGroup), OrderLast = true)] //run in initialization system group, and run it last in that. happens before main logic group. Check the entity Systems tab in unity editor.
[BurstCompile]
public partial class PlayerInputSystem : SystemBase
{
    private GameInput InputActions;
    private Entity Player;

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>();
        RequireForUpdate<PlayerMoveInput>();
        InputActions = new GameInput(); //initialize
    }

    protected override void OnStartRunning() //happens after OnCreate, when it start's running, very descriptive, I know.
    {
        InputActions.Enable();
        InputActions.GamePlay.Shoot.performed += OnShoot;
        Player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (!SystemAPI.Exists(Player)) return; //null check player

        SystemAPI.SetComponentEnabled<FireProjectileTag>(Player, true);
    }

    protected override void OnUpdate()
    {
        Vector2 moveInput = InputActions.GamePlay.Move.ReadValue<Vector2>(); 

        SystemAPI.SetSingleton(new PlayerMoveInput { Value = moveInput });

    }
    protected override void OnStopRunning()
    {
        InputActions.Disable();
        Player = Entity.Null;
    }
}

