using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace GPN
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class CameraSystem : SystemBase
    {
        private Entity player;
        private Camera camera;

        protected override void OnCreate()
        {
            RequireForUpdate<IsPlayerTag>();
        }

        protected override void OnUpdate()
        {
            var playerTransform = SystemAPI.GetComponent<LocalToWorld>(player);
            camera.transform.position = playerTransform.Position;
            camera.transform.position -= 10f*(Vector3)playerTransform.Forward;
            camera.transform.position += new Vector3(0f,5f,0f);
            camera.transform.LookAt(playerTransform.Position);
        }


        protected override void OnStartRunning() //OnEnable
        {
            player = SystemAPI.GetSingletonEntity<IsPlayerTag>();
            camera = Camera.main;
        }

        protected override void OnStopRunning() //OnDisable
        {
            player = Entity.Null;
        }
    }
}
