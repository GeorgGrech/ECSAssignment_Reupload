using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GPN
{
    public partial struct PlayerMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(
                (RefRW<LocalTransform> transform,
                RefRO<PlayerMoveInput> input,
                RefRO<MovementSpeed> speed) data
                
             in SystemAPI.Query<
              RefRW<LocalTransform>,
              RefRO<PlayerMoveInput>,
              RefRO<MovementSpeed>>())
              {
                float lookX = data.input.ValueRO.LookInput.x;
                quaternion spin = quaternion.RotateY(lookX * SystemAPI.Time.DeltaTime);
                data.transform.ValueRW.Rotation
                    = math.mul(spin, data.transform.ValueRO.Rotation);

                float3 forward = data.transform.ValueRO.Forward();
                float3 right = data.transform.ValueRO.Right();
                float2 moveInput =
                    data.input.ValueRO.MoveInput *
                    SystemAPI.Time.DeltaTime *
                    data.speed.ValueRO.Value;

                data.transform.ValueRW.Position +=
                    right * moveInput.x +
                    forward * moveInput.y;
            }
        }
    }
}
