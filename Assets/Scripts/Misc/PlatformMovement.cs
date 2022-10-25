using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float ascendRange = 1.0f;
    public float movementSpeed = 1.0f;
    private Vector3 _originalPosition, _upPosition;
    private bool goingUp = true;

    public void Start()
    {
        _originalPosition = transform.position;
        _upPosition = _originalPosition + Vector3.up * ascendRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > _upPosition.y)
        {
            goingUp = false;
        }
        else if (transform.position.y < _originalPosition.y)
        {
            goingUp = true;
        }

        if (goingUp)
        {
            transform.position += Vector3.up * movementSpeed * Time.deltaTime;
        }
        else
        {
            transform.position -= Vector3.up * movementSpeed * Time.deltaTime;
        }
    }
}
