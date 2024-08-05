using Unity.Entities;
using Unity.Physics;

namespace GPN
{
    public readonly partial struct JumperAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRW<PhysicsVelocity> Velocity;
        public readonly RefRO<PhysicsMass> Mass;
        public readonly RefRO<JumpForce> JumpForce;
    }
}
