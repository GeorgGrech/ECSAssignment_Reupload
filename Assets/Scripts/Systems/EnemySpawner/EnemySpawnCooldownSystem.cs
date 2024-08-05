using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(EnemySpawnSystem))]
    public partial struct EnemySpawnCooldownSystem : ISystem
    {        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var spawner in SystemAPI.Query<SpawnerAspect>().WithNone<CanSpawn>())
            {
                float cooldown = spawner.Spawner.ValueRO.Cooldown;

                spawner.Spawner.ValueRW.Cooldown = math.max(0f, cooldown - deltaTime);
                bool canSpawn = spawner.Spawner.ValueRO.Cooldown <= 0f;
                ecb.SetComponentEnabled<CanSpawn>(spawner.Entity, canSpawn);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}