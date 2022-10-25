using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCoordinatesTransform : MonoBehaviour
{
    public ICoordinateSystem CoordinateSystem;
    public Vector3 position;

    private void Start()
    {
        CoordinateSystem = new EuclideanCoordinateSystem();
        SetPosition(position);
    }

    public void SetPosition(Vector3 position)
    {
        var converted = CoordinateSystem.Map(position);
        this.position = position;
        transform.position = converted.Item1;
        transform.rotation = converted.Item2;
        transform.localScale = converted.Item3;
    }
}
