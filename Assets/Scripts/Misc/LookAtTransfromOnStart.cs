using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransfromOnStart : MonoBehaviour
{
    public Transform lookAt;
    // Start is called before the first frame update
    void Start()
    {
        LookAtTransform();
    }

    public void LookAtTransform()
    {
        transform.LookAt(lookAt);
        var rot = transform.rotation.eulerAngles;
        rot.x = rot.z = 0;
        transform.rotation = Quaternion.Euler(rot);
    }
}
