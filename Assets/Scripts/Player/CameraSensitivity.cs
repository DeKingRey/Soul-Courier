using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivity : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public float hSensitivity = 150f;
    public float vSensitivity = 2f;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        freeLookCam.m_XAxis.Value += mouseX * hSensitivity * Time.deltaTime;
        freeLookCam.m_YAxis.Value += -mouseY * vSensitivity * Time.deltaTime;
    }
}
