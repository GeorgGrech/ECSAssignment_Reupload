using Unity.Entities;

namespace GPN
{
    public struct Gun : IComponentData
    {
        public Entity Projectile;
        public Entity FirePoint;
        public float FireInterval;
        public float Cooldown;
    }
}