using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateMonitors : MonoBehaviour
{
    public GameObject monitorPrefab;
    public Material monitorMaterial;
    public Transform lookAtTransform;
    public Vector3 overAllScale = new Vector3(0.35f, 1, 0.35f);
    public Vector3 monitorScale = new Vector3(1, 10, 1);

    public float radius;
    public int monitorsCount = 12;

    private GameObject[] monitors; 
    // Start is called before the first frame update
    void Start()
    {
        monitors = new GameObject[monitorsCount];
        float angle = 360.0f / monitorsCount;
        for (int i = 0; i < monitorsCount; i++)
        {
            monitors[i] = Instantiate(monitorPrefab);
            monitors[i].GetComponent<LookAtTransfromOnStart>().lookAt = lookAtTransform;
            monitors[i].transform.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle * i), 0, Mathf.Sin(Mathf.Deg2Rad * angle * i)) * radius + transform.position;
            monitors[i].GetComponent<LookAtTransfromOnStart>().LookAtTransform();
            monitors[i].transform.parent = transform;
            monitors[i].GetComponentInChildren<Renderer>().material = monitorMaterial;
            monitors[i].transform.localScale = monitorScale;
        }
        transform.localScale = overAllScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
