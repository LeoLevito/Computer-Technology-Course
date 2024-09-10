using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionLayer
{
    //create these in the User Layers in the Editor as well.
    Default = 1 << 0,
    Wall = 1 << 6,
    Enemy = 1 << 7
}
