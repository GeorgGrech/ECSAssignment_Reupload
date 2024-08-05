using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateBefore(typeof(PhysicsSimulationGroup))]
    public partial struct EnemyDamageSystem : ISystem
    {
        private ComponentLookup<Health> targetHealth;
        private ComponentLookup<IsEnemyTag> enemies;
        private ComponentLookup<Damage> damage;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<IsEnemyTag>();
            targetHealth = SystemAPI.GetComponentLookup<Health>();
            enemies = SystemAPI.GetComponentLookup<IsEnemyTag>(true);
            damage = SystemAPI.GetComponentLookup<Damage>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            var ecbSingleton
                = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            targetHealth.Update(ref state);
            enemies.Update(ref state);
            damage.Update(ref state);

            state.Dependency = new EnemyHitJob()
            {
                TargetHealth = targetHealth,
                Enemies = enemies,
                Damage = damage,
                CommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            }.Schedule(simulation, state.Dependency);
        }

        public struct EnemyHitJob : ICollisionEventsJob
        {
            public ComponentLookup<Health> TargetHealth;
            [ReadOnly] public ComponentLookup<IsEnemyTag> Enemies;
            [ReadOnly] public ComponentLookup<Damage> Damage;

            public EntityCommandBuffer CommandBuffer;

            public void Execute(CollisionEvent collisionEvent)
            {
                Entity enemy = Entity.Null;
                Entity targetEntity = Entity.Null;

                if (TargetHealth.HasComponent(collisionEvent.EntityA))
                    targetEntity = collisionEvent.EntityA;
                if (TargetHealth.HasComponent(collisionEvent.EntityB))
                    targetEntity = collisionEvent.EntityB;
                if (Enemies.HasComponent(collisionEvent.EntityA))
                    enemy = collisionEvent.EntityA;
                if (Enemies.HasComponent(collisionEvent.EntityB))
                    enemy = collisionEvent.EntityB;

                if (enemy == Entity.Null)
                    return;


                if (targetEntity == Entity.Null)
                    return;

                if (enemy == targetEntity) //Turning off enemy collision doesnt avoid self-damage, so just check if enemy and target is same
                {
                    return;  
                }


                RefRW<Health> hitHealth = TargetHealth.GetRefRW(targetEntity);
                RefRO<Damage> damage = Damage.GetRefRO(enemy);

                CommandBuffer.AddComponent(targetEntity,new DisplayDamage { Value = damage.ValueRO.Value});

                //This theoretically should damage other enemies...but it doesn't? I guess that's fine by me.
                hitHealth.ValueRW.Value = math.max(0f, hitHealth.ValueRO.Value - damage.ValueRO.Value);


                UnityEngine.Debug.Log("Health: " + hitHealth.ValueRO.Value);

                if (hitHealth.ValueRO.Value <= 0f)
                    CommandBuffer.DestroyEntity(targetEntity);
            }
        }
    }
}