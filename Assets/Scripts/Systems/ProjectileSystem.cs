using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateBefore(typeof(PhysicsSimulationGroup))]
    public partial struct ProjectileSystem : ISystem
    {
        private ComponentLookup<Health> targetHealth;
        private ComponentLookup<IsProjectileTag> projectiles;
        private ComponentLookup<Damage> damage;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<IsProjectileTag>();
            targetHealth = SystemAPI.GetComponentLookup<Health>();
            projectiles = SystemAPI.GetComponentLookup<IsProjectileTag>(true);
            damage = SystemAPI.GetComponentLookup<Damage>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            var ecbSingleton
                = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            targetHealth.Update(ref state);
            projectiles.Update(ref state);
            damage.Update(ref state);

            state.Dependency = new ProjectileHitJob()
            {
                TargetHealth = targetHealth,
                Projectiles = projectiles,
                Damage = damage,
                CommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            }.Schedule(simulation, state.Dependency);
        }

        public struct ProjectileHitJob : ICollisionEventsJob
        {
            public ComponentLookup<Health> TargetHealth;
            [ReadOnly] public ComponentLookup<IsProjectileTag> Projectiles;
            [ReadOnly] public ComponentLookup<Damage> Damage;

            public EntityCommandBuffer CommandBuffer;

            public void Execute(CollisionEvent collisionEvent)
            {
                Entity projectile = Entity.Null;
                Entity targetEntity = Entity.Null;

                if (TargetHealth.HasComponent(collisionEvent.EntityA))
                    targetEntity = collisionEvent.EntityA;
                if (TargetHealth.HasComponent(collisionEvent.EntityB))
                    targetEntity = collisionEvent.EntityB;
                if (Projectiles.HasComponent(collisionEvent.EntityA))
                    projectile = collisionEvent.EntityA;
                if (Projectiles.HasComponent(collisionEvent.EntityB))
                    projectile = collisionEvent.EntityB;

                if (projectile == Entity.Null)
                    return;

                CommandBuffer.DestroyEntity(projectile);


                if (targetEntity == Entity.Null)
                    return;

                RefRW<Health> hitHealth = TargetHealth.GetRefRW(targetEntity);
                RefRO<Damage> damage = Damage.GetRefRO(projectile);

                CommandBuffer.AddComponent(targetEntity,new DisplayDamage { Value = damage.ValueRO.Value});

                hitHealth.ValueRW.Value = math.max(0f, hitHealth.ValueRO.Value - damage.ValueRO.Value);

                if (hitHealth.ValueRO.Value <= 0f)
                    CommandBuffer.DestroyEntity(targetEntity);
            }
        }
    }
}