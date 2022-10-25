using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float sensitivity = 1f;
    public bool invertAxis = false;
    private Vector3 previousMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        previousMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
            if (invertAxis)
            {
                mouseDelta *= -1;
            }
            transform.Rotate(new Vector3(mouseDelta.y, -mouseDelta.x, 0) * sensitivity);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
        else
        {
            Cursor.visible = true;
        }

        previousMousePosition = Input.mousePosition;
    }
}
