using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class DamageUISystem : SystemBase
    {
        public Action<float, float3> OnDealDamage;

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);


            foreach (var (displayDamage, transform, entity) in
                     SystemAPI.Query<RefRO<DisplayDamage>,
                         LocalTransform>().WithEntityAccess())
            {

                float3 spawnOffset = new float3(0, 1, 0);

                float damage = displayDamage.ValueRO.Value;
                float3 pos = transform.Position;

                OnDealDamage?.Invoke(damage, pos+spawnOffset);
                //Debug.Log("Display Damage: "+damage);

                ecb.RemoveComponent<DisplayDamage>(entity);

            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}