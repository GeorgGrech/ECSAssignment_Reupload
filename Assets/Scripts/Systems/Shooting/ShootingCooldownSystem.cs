using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(ShootingSystem))]
    public partial struct ShootingCooldownSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var shooter in SystemAPI.Query<ShooterAspect>().WithNone<CanShoot>())
            {
                float cooldown = shooter.Gun.ValueRO.Cooldown;

                shooter.Gun.ValueRW.Cooldown = math.max(0f, cooldown - deltaTime);
                bool canShoot = shooter.Gun.ValueRO.Cooldown <= 0f;
                ecb.SetComponentEnabled<CanShoot>(shooter.Entity, canShoot);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}