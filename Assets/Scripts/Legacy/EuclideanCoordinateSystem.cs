using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuclideanCoordinateSystem : ICoordinateSystem
{
    public (Vector3, Quaternion, Vector3) Map(Vector3 mapped)
    {
        
        return (mapped, Quaternion.identity, Vector3.one);
    }
}
