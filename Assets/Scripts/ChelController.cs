using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChelController : MonoBehaviour
{
    public PlayerController player;
    private Animator _animator;
    private Rigidbody _rb;
    private static readonly int animIsGrabbed = Animator.StringToHash("IsGrabbed");

    public AudioSource AudioSource;
    public AudioClip end;
    public AudioClip pickup;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Grablya")
            BeGrabbed();
        if (other.name.Contains(this.name) && AudioSource != null)
        {
            GameEndScript.End[this.name] = true;
            AudioSource.clip = end;
            AudioSource.Play();
        }
    }

    public void BeGrabbed()
    {
        AudioSource.clip = pickup;
        if (AudioSource != null)
            AudioSource.Play();
        _animator.SetBool(animIsGrabbed, true);
        player.SetAnimsHold(true);
        player.isGrabbed = true;
        player.GrabbedChel = GetComponent<ChelController>();
        GetComponent<Rigidbody>().isKinematic = true;
        //transform.SetParent(player.grablyaR.transform);
    }

    public void BeUngrabbed()
    {
        AudioSource.clip = pickup;
        if (AudioSource != null)
            AudioSource.Play();
        _animator.SetBool(animIsGrabbed, false);
        player.SetAnimsHold(false);
        player.isGrabbed = false;
        player.GrabbedChel = null;
        GetComponent<Rigidbody>().isKinematic = false;
        
    }
}
