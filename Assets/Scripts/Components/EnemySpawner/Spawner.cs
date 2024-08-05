using Unity.Entities;

namespace GPN
{
    public struct Spawner : IComponentData
    {
        public Entity Enemy;
        public float SpawnInterval;
        public float Cooldown;
        public float MaxSpawnDistance;
        public float MinSpawnDistance;
    }
}