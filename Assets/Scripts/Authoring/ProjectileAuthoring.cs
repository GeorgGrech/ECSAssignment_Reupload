using Unity.Entities;
using UnityEngine;

namespace GPN
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        public float Damage;
        public float FlightSpeed;
        public float Lifetime;

        private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(
                    entity,
                    new Damage { Value = authoring.Damage });
                AddComponent(
                    entity,
                    new FlightSpeed { Value = authoring.FlightSpeed });

                AddComponent(entity, new ProjectileLifetime { Value = authoring.Lifetime });
                AddComponent<IsProjectileTag>(entity);
                AddComponent<VfxEmitter>(entity);
            }
        }
    }
}