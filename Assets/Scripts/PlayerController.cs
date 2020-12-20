using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    Enter,
    Stay,
    Exit
}

public class PlayerController : MonoBehaviour
{
    public float ThrowSpeed;
    public float Force;
    public Transform snowBallStartTransform;
    public GameObject SnowBallPrefab;

    [Header("Movement")]
    public bool MovementActive;
    public float TurnSpeed;
    public float MovementSpeed;

    private Rigidbody rb;

    //events
    public event Action<PlayerController, TriggerType, Collision> CollisionEvent;
    public event Action<PlayerController, TriggerType, Collider> TriggerEvent;
    public event Action<PlayerController, GameObject> SnowBallCreated;

    private AudioSource _footstepAudioSource;
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        if (_footstepAudioSource == null)
        {
            _footstepAudioSource = GetComponent<AudioSource>();
            if(_footstepAudioSource == null)
            {
                _footstepAudioSource = gameObject.AddComponent<AudioSource>();
                _footstepAudioSource.clip = AudioSourceManager.Instance.SnowManStep;
                _footstepAudioSource.playOnAwake = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBall();
        }

        if (transform.position.y < -3f)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = new Vector3(0, 1f, 0);
        }
        if (MovementActive)
        {
            float vertical = Input.GetAxis("Vertical");
            rb.velocity = transform.forward * MovementSpeed * vertical;
        }
        float horizontal = Input.GetAxis("Horizontal");
        rb.MoveRotation(transform.rotation * Quaternion.Euler(0f, TurnSpeed * horizontal, 0f));
    }

    public void ThrowBall()
    {
        GameObject snowBall = GameObject.Instantiate(SnowBallPrefab);
        snowBall.transform.position = snowBallStartTransform.position;
        snowBall.GetComponent<Rigidbody>().AddForce(transform.forward * ThrowSpeed, ForceMode.Impulse);
        SnowBallCreated?.Invoke(this, snowBall);
        if (!MovementActive)
        {
            rb.AddForce(transform.forward * Force * -1, ForceMode.Impulse);
        }
        _footstepAudioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEvent?.Invoke(this, TriggerType.Enter, collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        CollisionEvent?.Invoke(this, TriggerType.Exit, collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        CollisionEvent?.Invoke(this, TriggerType.Stay, collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        TriggerEvent?.Invoke(this, TriggerType.Enter, other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerEvent?.Invoke(this, TriggerType.Exit, other);
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerEvent?.Invoke(this, TriggerType.Stay, other);
    }
}
