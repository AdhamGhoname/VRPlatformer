using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAtRandomSpeed : MonoBehaviour
{
    public float speed;
    public float minSpeed, maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
