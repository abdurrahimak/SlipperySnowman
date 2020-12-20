using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("LookAt")]
    public Vector3 LookAtOffset = Vector3.zero;

    [Header("Speeds")]
    public float TurnSpeed;
    public float ScrollSpeed;

    [Header("StartPosition")]
    [Range(10, 80)] public float CameraStartRotationX = 30;
    public float CameraStartRotationY = 0;
    public float CameraStartDistance = 15f;

    private Vector3 _dirToCamera;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _dirToCamera = Quaternion.AngleAxis(CameraStartRotationX, transform.right * -1f) * (transform.forward) * CameraStartDistance;
        _dirToCamera = Quaternion.AngleAxis(CameraStartRotationY, transform.up) * _dirToCamera;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            float horizontalScrollDelta = Input.GetAxis("Mouse X");
            _dirToCamera = Quaternion.AngleAxis(horizontalScrollDelta * TurnSpeed, transform.up) * _dirToCamera;

            float verticalMovement = Input.GetAxis("Mouse Y");
            Vector3 cameraForwardFromPlayer = (-1f * _dirToCamera).normalized;
            cameraForwardFromPlayer.y = 0f;
            cameraForwardFromPlayer.x = cameraForwardFromPlayer.x > 0 ? Mathf.Clamp(cameraForwardFromPlayer.x, 1f, 1f) : Mathf.Clamp(cameraForwardFromPlayer.x, -1f, -1f);
            Vector3 rightDir = Vector3.Cross(cameraForwardFromPlayer, transform.up).normalized;
            Debug.DrawRay(transform.position, rightDir * 5f, Color.blue);
            Debug.DrawRay(transform.position, _camera.transform.right * 5f, Color.red);
            Debug.DrawRay(transform.position, cameraForwardFromPlayer * 5f, Color.green);
            float angle = verticalMovement * TurnSpeed;
            _dirToCamera = Quaternion.AngleAxis(angle, _camera.transform.right * -1f) * _dirToCamera;
        }
        _dirToCamera += (Input.mouseScrollDelta.y * -1 * ScrollSpeed * Time.deltaTime) * _dirToCamera.normalized;
        _camera.transform.position = transform.position + _dirToCamera;
        _camera.transform.LookAt(transform.position + LookAtOffset);
    }
}
