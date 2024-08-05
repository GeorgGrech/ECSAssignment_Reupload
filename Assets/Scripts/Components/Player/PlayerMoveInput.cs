using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GPN
{
    public struct PlayerMoveInput : IComponentData
    {
        public float2 MoveInput;
        public float2 LookInput;
    }
}
