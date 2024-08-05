using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(EnemySpawnSystem))]
    public partial struct EnemyUpdateTargetSystem : ISystem
    {

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (
                (RefRO<LocalTransform> transform,
                RefRO<IsPlayerTag> tag) playerData

             in SystemAPI.Query<
              RefRO<LocalTransform>,
              RefRO<IsPlayerTag>>())
            {


                foreach (
                (RefRW<LocalTransform> transform,
                RefRW<EnemyTarget> target,
                RefRO<MovementSpeed> speed) enemyData

             in SystemAPI.Query<
              RefRW<LocalTransform>,
              RefRW<EnemyTarget>,
              RefRO<MovementSpeed>>())
                {
                    enemyData.target.ValueRW.targetPos = playerData.transform.ValueRO.Position;
                }

            }
        }

    }
}