using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] public float speed;
    public float dragValue;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    private Animator _acL;
    private Animator _acR;

    public AudioSource AudioSource;
    
    [Header("Grabbing")] public bool isGrabbed;
    public ChelController GrabbedChel;
    public GameObject handL;
    public GameObject grablyaL;
    public GameObject handR;
    public GameObject grablyaR;
    private int animIsWalking = Animator.StringToHash("IsWalking");
    private int animGrab = Animator.StringToHash("Grab");
    private int animIsHolding = Animator.StringToHash("IsHolding");

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        
        _acL = handL.GetComponent<Animator>();
        _acR = handR.GetComponent<Animator>();
        grablyaL = handL.GameObject().GetComponentInChildren<SphereCollider>().GameObject();
        grablyaL.SetActive(false);
        grablyaR = handR.GameObject().GetComponentInChildren<SphereCollider>().GameObject();
        grablyaR.SetActive(false);
        GrabbedChel = null;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        speed = 50;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= 2;
        _acL.SetBool(animIsWalking, false);
        _acR.SetBool(animIsWalking, false);
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        _rb.drag = dragValue;
        var velocity = _rb.velocity;
        Vector3 flatVel = new Vector3(velocity.x, 0, velocity.z).normalized;
        if (flatVel.magnitude > speed * 2)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
        if (velocity.magnitude > 0.01f)
        {
            AudioSource.enabled = true;
            _acL.SetBool(animIsWalking, true);
            _acR.SetBool(animIsWalking, true);
        }
        else
        {
            
            AudioSource.enabled = false;
        }
        if (Input.GetMouseButtonDown(0) && !isGrabbed)
            Grab();
        else if (Input.GetMouseButtonDown(0) && isGrabbed)
            UnGrab();
        if (isGrabbed && GrabbedChel != null)
        {
            GrabbedChel.transform.position = (grablyaL.transform.position + grablyaR.transform.position) * 0.5f - Vector3.up * 0.3f;
            var vec = new Vector3(transform.position.x, GrabbedChel.transform.position.y, transform.position.z);
            GrabbedChel.transform.LookAt(vec);
            GrabbedChel.transform.rotation *= quaternion.RotateY(160);
        }
    }
    private void FixedUpdate()
    {
        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        _rb.AddForce(_moveDirection.normalized * speed, ForceMode.Force);
    }

    public void SetAnimsHold(bool isholding)
    {
        _acL.SetBool(animIsHolding, isholding);
        _acR.SetBool(animIsHolding, isholding);
    }
    
    private void Grab()
    {
        grablyaL.SetActive(true);
        grablyaR.SetActive(true);
        _acL.SetTrigger(animGrab);
        _acR.SetTrigger(animGrab);
    }
    private void UnGrab()
    {
        if (isGrabbed && GrabbedChel != null)
            GrabbedChel.BeUngrabbed();
        _acL.SetTrigger(animGrab);
        _acR.SetTrigger(animGrab);
        grablyaL.SetActive(false);
        grablyaR.SetActive(false);
    }
}
