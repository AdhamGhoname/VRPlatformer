using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZombieController : MonoBehaviour
{
    public float horizontalSpeed = 1.0f;
    public float jumpForce = 4.0f;
    public float patrolTime = 3.0f;
    public Transform sprite;
    public Animator animator;

    private float _timer = 0;
    private float _direction = 1.0f;
    private bool Dead = false;
    public bool roundLevel = true;

    private Collider[] _colliders;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>().Select(c => c).Union(GetComponents<Collider>().Select(c => c)).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            return;
        }

        var speed = _direction * horizontalSpeed;
        Vector3 right = roundLevel ? GetRadialHorizontalUnitVector() : Vector3.right;
        _rb.velocity = right * speed;
        _timer += Time.deltaTime;

        if (roundLevel)
        {
            transform.LookAt(Camera.main.transform.position);
            var rotation = transform.rotation.eulerAngles;
            rotation.x = rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }

        if (_timer >= 0 && _timer <= patrolTime / 2.0)
        {
            _direction = 1.0f;
            if (roundLevel)
                sprite.localRotation = Quaternion.Euler(0, 180, 0);
            else
                sprite.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_timer > patrolTime / 2.0 && _timer <= patrolTime)
        {
            _direction = -1.0f;
            if (roundLevel)
                sprite.localRotation = Quaternion.Euler(0, 0, 0);
            else
                sprite.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _timer = 0.0f;
        }
    }

    private Vector3 GetRadialHorizontalUnitVector()
    {
        Vector3 radius = GetRadialUnitVector();
        return new Vector3(radius.z, 0, -radius.x).normalized;
    }

    private Vector3 GetRadialUnitVector()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 radius = transform.position - camPos;
        radius.y = 0;
        // Debug.DrawRay(camPos, radius);
        // Debug.Log(radius.magnitude);
        return radius.normalized;
    }

    public void Die()
    {
        Dead = true;
        _rb.velocity = Vector3.up * jumpForce / 2;
        _rb.freezeRotation = true;
        animator.SetTrigger("Die");
        _colliders.All(c => {
            c.enabled = false;
            return true;
        });
        Invoke("DestroySelf", 3.0f);
    }

    private void DestroySelf()
    {
        Destroy(this);
    }
}
