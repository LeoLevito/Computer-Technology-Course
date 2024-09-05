using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public struct Spawner : IComponentData
{
    public Entity Prefab; 
    public float2 SpawnPosition; //better vector2
    public float NextSpawnTime;
    public float SpawnRate;
    public float DestroyTime;
}
