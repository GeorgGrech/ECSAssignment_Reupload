using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GPN
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class PlayerInputSystem : SystemBase
    {
        private Controls controls;
        private Entity playerEntity;
        protected override void OnCreate() //On Awake and Start
        {
            RequireForUpdate<IsPlayerTag>();
            RequireForUpdate<PlayerMoveInput>();

            controls = new Controls();
        }
        protected override void OnUpdate() //Update
        {
            float2 moveInput = controls.Game.Move.ReadValue<Vector2>();
            float2 lookInput = controls.Game.Look.ReadValue<Vector2>();
            bool isJumping = controls.Game.Jump.IsPressed();
            bool isFiring = controls.Game.Fire.IsPressed();

            // Processing of inputs...
            // ...
            SystemAPI.SetSingleton(new PlayerMoveInput
            { MoveInput = moveInput, LookInput = lookInput });

            SystemAPI.SetComponentEnabled<IsJumping>(playerEntity, isJumping);
            SystemAPI.SetComponentEnabled<IsShooting>(playerEntity, isFiring);

        }

        protected override void OnStartRunning() //OnEnable
        {
            controls.Enable();
            playerEntity = SystemAPI.GetSingletonEntity<IsPlayerTag>();
        }

        protected override void OnStopRunning() //OnDisable
        {
            controls.Disable();
            playerEntity = Entity.Null;
        }

    }
}
