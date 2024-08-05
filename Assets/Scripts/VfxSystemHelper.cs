using GPN;
using Unity.Entities;
using UnityEngine;

public class VfxSystemHelper : MonoBehaviour
{
    public ParticleSystem particles;

    void Start()
    {
        Debug.Log("Helper Start");
        World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<VfxSystem>().Init(particles);
    }
}
