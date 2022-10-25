using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoordinateSystem
{
    (Vector3, Quaternion, Vector3) Map(Vector3 mapped);
}
