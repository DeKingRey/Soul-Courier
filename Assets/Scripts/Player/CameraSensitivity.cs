using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivity : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public float sensitivity = 1f;

    public float xSensitivity;
    public float ySensitivity;

    void Update()
    {
        if (Time.timeScale == 0) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        freeLookCam.m_XAxis.Value += mouseX * sensitivity * xSensitivity;
        freeLookCam.m_YAxis.Value += -mouseY * sensitivity * ySensitivity;
    }
}
