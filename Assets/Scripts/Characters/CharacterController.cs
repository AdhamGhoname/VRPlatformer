using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Valve.VR;
using Valve.VR.InteractionSystem;
//using UnityEngine.XR;


[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{

    public float horizontalSpeed = 1f;
    public float jumpForce = 1f;


    public float steamVRTouchPadDeadZone = 0.3f;

    public SteamVR_Action_Vector2 touchPadAction;
    public SteamVR_Action_Boolean trigger;

    public Animator animator;
    public AudioSource jumpSound;
    public bool roundLevel = true;

    public Transform Sprite, GroundCheck;
    public bool flipSprite = false;

    private BackgroundManager _backgroundManager;

    private Rigidbody _rb;
    private Collider _collider;

    //private List<InputDevice> controllers;

    private bool Jumping = false, Dead = false;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _backgroundManager = GameObject.FindGameObjectWithTag("BackgroundManager").GetComponent<BackgroundManager>();
        //var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand;
        //controllers = new List<InputDevice>();
        //InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, controllers);
        //Debug.Log("Connected controllers: " + controllers.Count);
    }

    private Vector2 GetPrimary2DAxis()
    {
        //Vector2 result = Vector2.zero;
        //foreach (var controller in controllers)
        //{
        //    Vector2 current;
        //    controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out current);
        //    if (current.magnitude > result.magnitude)
        //    {
        //        result = current;
        //    }
        //}
        //return result;
        Vector2 result = touchPadAction[SteamVR_Input_Sources.LeftHand].axis;
        return result;
    }

    private bool GetJumpAxis()
    {
        //bool current;
        //foreach (var controller in controllers)
        //{
        //    controller.TryGetFeatureValue(CommonUsages.primaryButton, out current);
        //    if (current)
        //        return true;
        //}
        //return false;
        return trigger[SteamVR_Input_Sources.RightHand].state;
    }

    // Update is called once per frame
    void Update()
    {
        //InputDevices.GetDevices(controllers);
        //Debug.Log("Connected controllers: " + controllers.Count);
        if (Dead)
        {
            return;
        }

        Vector2 touchpadAxis = GetPrimary2DAxis();

        float xAxis = (Mathf.Abs(touchpadAxis.x) > steamVRTouchPadDeadZone ? 1 : 0) * touchpadAxis.x;
        float yAxis = Mathf.Max(Input.GetAxis("Jump"), (GetJumpAxis() ? 1 : 0));

        xAxis = Mathf.Abs(xAxis) > Mathf.Abs(Input.GetAxis("Horizontal")) ? xAxis : Input.GetAxis("Horizontal");


        var speed = horizontalSpeed * xAxis;
        if (xAxis != 0)
        {
            animator.SetBool("Walking", true);
            if (xAxis < 0)
            {
                if (flipSprite)
                    Sprite.localRotation = Quaternion.Euler(Vector3.zero);
                else
                    Sprite.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            }
            else
            {
                if (flipSprite)
                    Sprite.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                else
                    Sprite.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        //Debug.Log(Grounded());

        if (Jumping && Grounded() && _rb.velocity.y <= 0)
        {
            if (yAxis == 0)
            {
                Jumping = false;
            }
            animator.SetBool("Jumping", false);
        }

        if (yAxis != 0 && !Jumping && Grounded())
        {
            Jumping = true;
            //Debug.Log("Jumping");
            _rb.velocity = new Vector3(_rb.velocity.x, jumpForce * yAxis, _rb.velocity.z);
            jumpSound.Play();
            animator.SetBool("Jumping", true);
        }

        Vector3 right = roundLevel ? GetRadialHorizontalUnitVector() : Vector3.right;
        _rb.velocity = right * speed + Vector3.up * _rb.velocity.y;

        //Debug.Log(_rb.velocity);
        if (roundLevel)
        {
            transform.LookAt(Camera.main.transform.position);
            var rotation = transform.rotation.eulerAngles;
            rotation.x = rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
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

    private bool Grounded()
    {
        var colliders = Physics.OverlapBox(GroundCheck.position, new Vector3(0.3f, 0.05f, 0.5f));
        return colliders.Any(collider => collider.gameObject != gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PitBottom")
        {
            ReloadScene();
        }
        else if (collision.collider.tag == "Killer")
        {
            Dead = true;
            Debug.Log(collision.collider.name);
            _rb.velocity = Vector3.up * jumpForce / 2;
            _rb.freezeRotation = true;
            animator.SetTrigger("Die");
            _collider.enabled = false;
            Invoke("ReloadScene", 3f);
        }
        else if (collision.collider.tag == "ZombieKillZone")
        {
            _rb.velocity = Vector3.up * collision.collider.GetComponentInParent<ZombieController>().jumpForce * 1.5f;
            collision.collider.GetComponentInParent<ZombieController>().Die();
        }
        else if (collision.collider.tag == "Finishline")
        {
            _rb.velocity = Vector3.up * jumpForce * 0.75f;
            animator.SetTrigger("Win");
            Invoke("ReloadScene", 5f);
            Invoke("ChangeBackground", 5f);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ChangeBackground()
    {
        _backgroundManager.SetRandomBackground();
    }
}
