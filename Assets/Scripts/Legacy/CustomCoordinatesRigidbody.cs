using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(CustomCoordinatesTransform))]
public class CustomCoordinatesRigidbody : MonoBehaviour
{
    [HideInInspector]
    public Vector3 Acceleration { get; private set; }

    [HideInInspector]
    public Vector3 Velocity { get; set; }

    [HideInInspector]
    public Vector3 Position { get; private set; }

    public float mass = 1.0f;

    public bool useGravity = true;

    private CustomCoordinatesTransform _customTransform;

    private CapsuleCollider _collider;


    // Start is called before the first frame update
    void Start()
    {
        _customTransform = GetComponent<CustomCoordinatesTransform>();
        _collider = GetComponent<CapsuleCollider>();
        Position = transform.position;
    }

    public void FixedUpdate()
    {
        Velocity += Acceleration * Time.deltaTime;
        var translation = Velocity * Time.deltaTime;
        Position = _customTransform.position;
        if (CheckMovement(translation))
        {
            Position += translation;
        }
        else
        {
            Position += GetMaximumTranslation(translation);
            Velocity = Vector3.zero;
        }
        _customTransform.SetPosition(Position);

        Acceleration = Vector3.zero;

        if (useGravity)
        {
            Acceleration = Vector3.down * 9.81f;
        }
    }

    public void ApplyForce(Vector3 force)
    {
        if (mass == 0.0f)
        {
            throw new System.Exception("Can't apply forces to objects with zero mass.");
        }

        Acceleration += force / mass;
    }

    public bool CheckMovement(Vector3 translation)
    {
        Vector3 origin = transform.position + _collider.center + translation;
        var coordinateSystem = _customTransform.CoordinateSystem;
        Vector3 point0 = coordinateSystem.Map(origin + Vector3.up * (_collider.height / 2.0f - _collider.radius)).Item1;
        Vector3 point1 = coordinateSystem.Map(origin + Vector3.down * (_collider.height / 2.0f - _collider.radius)).Item1;
        //Debug.Log(_collider.center);
        //Debug.DrawLine(point0, point1);
        var colliders = Physics.OverlapCapsule(point0, point1, _collider.radius);
        return !colliders.Any(collider => collider != _collider);
    }

    public Vector3 GetMaximumTranslation(Vector3 upperBound)
    {
        float st = 0, en = upperBound.magnitude, mid, ans = 0;
        while (Mathf.Abs(st-en) > (1e-1))
        {
            mid = (st + en) / 2;
            if (CheckMovement(upperBound * mid))
            {
                st = mid;
                ans = mid;
            }
            else
            {
                en = mid;
            }
        }
        Vector3 ret = ans * upperBound;
        return ret;
    }
}
