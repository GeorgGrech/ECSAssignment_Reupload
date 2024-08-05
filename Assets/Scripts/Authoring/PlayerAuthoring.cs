using Unity.Entities;
using UnityEngine;

namespace GPN
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float MovementSpeed;
        public float JumpForce;
        public int Health;

        public GameObject Projectile;
        [Min(0.01f)] public float FireInterval;
        public Transform FirePoint;

        public AudioClip FireSound;

        private class PlayerAuthoringBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(
                    entity,
                    new MovementSpeed { Value = authoring.MovementSpeed });
                AddComponent(
                    entity,
                    new JumpForce { Value = authoring.JumpForce });
                AddComponent(entity, new PlayerMoveInput());
                AddComponent<IsPlayerTag>(entity);
                AddComponent<CanJump>(entity);
                AddComponent<IsJumping>(entity);
                AddComponent(entity, new Health { Value = authoring.Health });

                SetComponentEnabled<IsJumping>(entity, false);

                AddComponent<IsShooting>(entity);
                AddComponent<CanShoot>(entity);

                SetComponentEnabled<IsShooting>(entity, false);

                AddComponent<LockRotationTag>(entity);

                AddComponent(entity, new Gun
                {
                    Projectile = GetEntity(authoring.Projectile, TransformUsageFlags.Dynamic),
                    FirePoint = GetEntity(authoring.FirePoint, TransformUsageFlags.Dynamic),
                    FireInterval = authoring.FireInterval,
                    Cooldown = 0f,
                });
            }
        }
    }
}