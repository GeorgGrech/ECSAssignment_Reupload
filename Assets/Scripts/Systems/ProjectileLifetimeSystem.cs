using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct ProjectileLifetimeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var projectile in SystemAPI.Query <ProjectileAspect>())
            {
                projectile.Lifetime.ValueRW.Value -= UnityEngine.Time.deltaTime;

                if(projectile.Lifetime.ValueRW.Value <= 0)
                {
                    UnityEngine.Debug.Log("Projectile lifetime over, destroyed");
                    ecb.DestroyEntity(projectile.Entity);
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
    }
}