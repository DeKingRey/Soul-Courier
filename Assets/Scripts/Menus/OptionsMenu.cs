using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Sensitivity")]
    private CameraSensitivity cameraSensitivity;
    public Slider sensitivitySlider;

    [Header("Quality")]
    public TMP_Dropdown qualityDropdown;

    void Start()
    {
        // Sets values from player prefs
        cameraSensitivity = FindObjectOfType<CameraSensitivity>();
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.5f);

        int savedQuality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQuality, true); // Default is medium

        // Sets UI values from player prefs
        cameraSensitivity.sensitivity = savedSensitivity;
        sensitivitySlider.value = savedSensitivity;

        qualityDropdown.value = savedQuality;
        qualityDropdown.RefreshShownValue();

        // Adds event listeners to UI inputs
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    public void SetSensitivity(float value)
    {
        cameraSensitivity.sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("Quality", index);
        PlayerPrefs.Save();
    }
}
