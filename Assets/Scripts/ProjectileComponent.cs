using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ProjectileComponent : IComponentData
{
    public float DeathTimer;
    public float DeathTimer2;
    public float ProjectileMoveSpeed;
    public float ColliderSize;
    public Entity projectileEntity;
}
