using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivity : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public float sensitivityMultiplier = 1f;

    public float xSensitivity;
    public float ySensitivity;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        freeLookCam.m_XAxis.Value += mouseX * sensitivityMultiplier * xSensitivity;
        freeLookCam.m_YAxis.Value += -mouseY * sensitivityMultiplier * ySensitivity;
    }
}
