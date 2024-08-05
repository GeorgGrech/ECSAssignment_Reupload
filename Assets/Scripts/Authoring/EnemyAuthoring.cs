using Unity.Entities;
using UnityEngine;

namespace GPN
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float MovementSpeed;
        public int Health;
        public int Damage;

        public Transform FollowPoint;

        private class EnemyAuthoringBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(
                    entity,
                    new MovementSpeed { Value = authoring.MovementSpeed });

                AddComponent<IsEnemyTag>(entity); 
                AddComponent(entity, new Health { Value = authoring.Health });
                AddComponent(entity, new Damage { Value = authoring.Damage });
                AddComponent<EnemyTarget>(entity);
                AddComponent<LockRotationTag>(entity);
            }


        }
    }
}
