using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace GPN
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class VfxSystem : SystemBase
    {
        UnityEngine.ParticleSystem particleSystem;
        Transform particleSystemTransform;
        float interval;
        float timer;

        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false; // Dont run the system until we have set everything up
        }

        protected override void OnUpdate()
        {
            var deltaTime = UnityEngine.Time.deltaTime;
            timer += deltaTime;
            var count = Mathf.RoundToInt(timer / interval);

            if(count > 0)
            {
                Entities.WithAll<VfxEmitter>().ForEach((in LocalTransform transform) =>
                {
                    particleSystemTransform.position = transform.Position;
                    particleSystemTransform.rotation = transform.Rotation;
                    particleSystem.Emit(1);
                }).WithoutBurst().Run();

                timer -= count * interval;
            }
        }

        public void Init(UnityEngine.ParticleSystem particleSystem)
        {
            Debug.Log("Vfx System Init");
            this.particleSystem = particleSystem;
            particleSystemTransform = particleSystem.transform;
            interval = 1f / particleSystem.emission.rateOverTimeMultiplier;
            Enabled = true; // Everything is ready, can begin running the system
        }
    }
}