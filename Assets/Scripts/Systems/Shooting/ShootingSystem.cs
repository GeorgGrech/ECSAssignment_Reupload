using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct ShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var shooter in SystemAPI.Query<ShooterAspect>().WithAll<CanShoot, IsShooting>())
            {
                ecb.SetComponentEnabled<CanShoot>(shooter.Entity, false);

                var muzzle = SystemAPI.GetComponentRO<LocalToWorld>(shooter.Gun.ValueRO.FirePoint);

                shooter.Gun.ValueRW.Cooldown = shooter.Gun.ValueRO.FireInterval;
                float3 muzzlePos = muzzle.ValueRO.Position;
                quaternion spawnRotation = muzzle.ValueRO.Rotation;

                Entity projectile = ecb.Instantiate(shooter.ProjectilePrefab);
                ecb.SetComponent(projectile, new LocalTransform
                {
                    Position = muzzlePos,
                    Rotation = spawnRotation,
                    Scale = 1f,
                });

                ecb.SetComponent(projectile, new PhysicsVelocity
                {
                    Linear = muzzle.ValueRO.Forward * 50f
                });
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}