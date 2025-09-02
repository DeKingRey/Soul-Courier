using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform cameraPivot;
    public float sensitivity = 1f;
    public float topClamp = 70f;
    public float bottomClamp = -30f;

    private float cameraHorizontal;
    private float cameraVertical;

    void Start()
    {
        Vector3 angles = cameraPivot.rotation.eulerAngles;
        cameraHorizontal = angles.y;
        cameraVertical = angles.x;

        Application.targetFrameRate = 60;
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0) return;

        float inputX = Input.GetAxis("Mouse X");
        float inputY = Input.GetAxis("Mouse Y");

        cameraHorizontal += inputX * sensitivity;
        cameraVertical -= inputY * sensitivity;

        cameraVertical = Mathf.Clamp(cameraVertical, bottomClamp, topClamp);

        cameraPivot.position = FindObjectOfType<Player>().transform.position + Vector3.up * 1.7f;
        cameraPivot.rotation  = Quaternion.Euler(cameraVertical, cameraHorizontal, 0f);
    }
}
