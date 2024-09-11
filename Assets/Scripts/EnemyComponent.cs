using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct EnemyComponent : IComponentData
{
    public float MoveSpeed;
    public float DeathTimer;
    public float DeathTimer2;
    public Entity enemyEntity;
}
