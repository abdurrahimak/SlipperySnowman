using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private PlayerController playerController;
    private float _nextThrowTime = 0f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MoveRotation(transform.rotation * Quaternion.Euler(0f, playerController.TurnSpeed, 0f));
        _nextThrowTime -= Time.deltaTime;
        if (_nextThrowTime < 0f)
        {
            playerController.ThrowBall();
            _nextThrowTime = UnityEngine.Random.Range(3f, 5f);
        }
    }
}
