using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GPN
{
    public class EnemySpawnAuthoring : MonoBehaviour
    {
        public GameObject Enemy;
        [Min(0.01f)] public float SpawnInterval;
        [Min(0.01f)] public float MaxSpawnDistance;
        public float MinSpawnDistance;
        // Start is called before the first frame update
        private class EnemySpawnAuthoringBaker: Baker<EnemySpawnAuthoring>
        {
            public override void Bake(EnemySpawnAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<IsEnemySpawner>(entity);
                AddComponent<CanSpawn>(entity);

                AddComponent(entity, new Spawner
                {
                    Enemy = GetEntity(authoring.Enemy, TransformUsageFlags.Dynamic),
                    SpawnInterval = authoring.SpawnInterval,
                    Cooldown = 0f,
                    MaxSpawnDistance = authoring.MaxSpawnDistance,
                    MinSpawnDistance = authoring.MinSpawnDistance
                });
            }
        }
    }
}
