using Unity.Entities;
using Unity.Physics;

namespace GPN
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct LockRotationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var mass
                in SystemAPI.Query<RefRW<PhysicsMass>>().WithAll<LockRotationTag>())
            {
                mass.ValueRW.InverseInertia
                    = new(0f, 1f, 0f);
            }
        }
    }
}
