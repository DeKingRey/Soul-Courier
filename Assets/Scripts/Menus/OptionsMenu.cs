using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Sensitivity")]
    private CameraSensitivity cameraSensitivity;
    public Slider sensitivitySlider;

    void Start()
    {
        cameraSensitivity = FindObjectOfType<CameraSensitivity>();
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);

        cameraSensitivity.sensitivity = savedSensitivity;
        sensitivitySlider.value = savedSensitivity;

        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    public void SetSensitivity(float value)
    {
        cameraSensitivity.sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
    }
}
