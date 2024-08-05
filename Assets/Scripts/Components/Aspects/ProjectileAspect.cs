using Unity.Entities;

namespace GPN
{
    public readonly partial struct ProjectileAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRW<ProjectileLifetime> Lifetime;
    }
}