using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPlatform : MonoBehaviour
{
    public float ascendRange = 1.0f;
    public float movementSpeed = 1.0f;
    private Vector3 _originalPosition, _upPosition;
    private bool goingUp = false;


    public GameObject nextLevel;
    public Transform viewer;

    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
        _upPosition = _originalPosition + Vector3.up * ascendRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            if (transform.position.y < _upPosition.y)
            {
                transform.position += Vector3.up * movementSpeed * Time.deltaTime;
                viewer.position += Vector3.up * movementSpeed * Time.deltaTime;
                //monitors.position += Vector3.up * movementSpeed * Time.deltaTime;
            }
            else
            {
                goingUp = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            nextLevel.SetActive(true);
            goingUp = true;
        }
    }
}
