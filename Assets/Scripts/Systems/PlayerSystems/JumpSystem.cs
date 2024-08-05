using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace GPN
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial struct JumpSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CanJump>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var jumper in SystemAPI.Query<JumperAspect>().WithAll<CanJump, IsJumping>())
            {
                float3 impulse = new float3(0f, 1f, 0f) * jumper.JumpForce.ValueRO.Value;
                jumper.Velocity.ValueRW.Linear = impulse;
                SystemAPI.SetComponentEnabled<CanJump>(jumper.Entity, false);
            }

            PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            foreach (var jumper in SystemAPI.Query<JumperAspect>().WithNone<CanJump>())
            {
                float3 start = SystemAPI.GetComponent<LocalToWorld>(jumper.Entity).Position;
                float3 end = start + new float3(0f, -0.5f, 0f);
                CollisionFilter filter = CollisionFilter.Default;
                filter.CollidesWith = ~(1u << 6);

                var raycast = new RaycastInput
                {
                    Start = start,
                    End = end,
                    Filter = filter,
                };

                SystemAPI.SetComponentEnabled<CanJump>(jumper.Entity, physicsWorld.CastRay(raycast));
            }
        }
    }
}
