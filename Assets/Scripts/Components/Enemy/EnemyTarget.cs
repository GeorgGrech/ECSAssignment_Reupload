using Unity.Entities;
using Unity.Mathematics;

namespace GPN
{
    public struct EnemyTarget : IComponentData
    {
        public float3 targetPos;
    }
}