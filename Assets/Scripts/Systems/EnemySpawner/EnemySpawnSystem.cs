using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct EnemySpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var spawner in SystemAPI.Query<SpawnerAspect>().WithAll<CanSpawn>())
            {
                ecb.SetComponentEnabled<CanSpawn>(spawner.Entity, false);

                float3 playerPos = new float3(0,0,0);
                spawner.Spawner.ValueRW.Cooldown = spawner.Spawner.ValueRO.SpawnInterval;

                foreach ( //This seems wildly inefficient
                (RefRO<LocalTransform> playerTransform,
                RefRO<IsPlayerTag> playerTag) data

                in SystemAPI.Query<
                  RefRO<LocalTransform>,
                  RefRO<IsPlayerTag>>())
                {
                    playerPos = data.playerTransform.ValueRO.Position;

                }

                float maxSpawnDistance = spawner.Spawner.ValueRO.MaxSpawnDistance;

                float3 randomPos = playerPos;

                while(math.distancesq(playerPos, randomPos) < spawner.Spawner.ValueRO.MinSpawnDistance)
                {
                    randomPos = new float3(UnityEngine.Random.Range(playerPos.x - maxSpawnDistance, playerPos.x + maxSpawnDistance),
                        playerPos.y,
                        UnityEngine.Random.Range(playerPos.z - maxSpawnDistance, playerPos.z + maxSpawnDistance));
                }

                Entity enemy = ecb.Instantiate(spawner.EnemyPrefab);
                ecb.SetComponent(enemy, new LocalTransform
                {
                    Position = randomPos,
                    Rotation = quaternion.identity,
                    Scale = 1f,
                });
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}