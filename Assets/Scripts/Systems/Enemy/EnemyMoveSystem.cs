using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using UnityEngine;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(EnemyUpdateTargetSystem))]
    public partial struct EnemyMoveSystem : ISystem
    {
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (
                (RefRW<LocalTransform> transform,
                RefRO<EnemyTarget> target,
                RefRO<MovementSpeed> speed) data

             in SystemAPI.Query<
              RefRW<LocalTransform>,
              RefRO<EnemyTarget>,
            RefRO<MovementSpeed>>())
            {

                data.transform.ValueRW.Position = Vector3.Lerp(data.transform.ValueRO.Position,
                    data.target.ValueRO.targetPos,
                    Time.deltaTime * data.speed.ValueRO.Value);

            }
        }
    }
}