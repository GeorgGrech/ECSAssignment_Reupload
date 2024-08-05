using Unity.Entities;

namespace GPN
{
    public readonly partial struct ShooterAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly EnabledRefRO<IsShooting> IsShooting;
        public readonly RefRW<Gun> Gun;

        public Entity ProjectilePrefab => Gun.ValueRO.Projectile;
    }
}