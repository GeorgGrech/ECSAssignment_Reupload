using Unity.Entities;

namespace GPN
{
    public readonly partial struct SpawnerAspect : IAspect
    {
        public readonly Entity Entity;

        public readonly RefRW<Spawner> Spawner;

        public Entity EnemyPrefab => Spawner.ValueRO.Enemy;
    }
}