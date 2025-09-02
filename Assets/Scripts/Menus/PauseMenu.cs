using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;  

public class PauseMenu : MonoBehaviour 
{
    public static bool isPaused = false;
    public bool gameOverPause = false;

    public GameObject pauseMenuUI;
    public GameObject compendiumUI;
    public GameObject optionsMenuUI;
    public GameObject controlsUI;
    private CameraController cameraSensitivity;
    private CinemachineVirtualCamera cameraCinemachine;

    void Start()
    {
        cameraSensitivity = FindObjectOfType<CameraController>();
        cameraCinemachine = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (gameOverPause) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                pauseMenuUI.SetActive(true);
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                compendiumUI.SetActive(true);
                Pause();
            }
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
        optionsMenuUI.SetActive(false);
        compendiumUI.SetActive(false);
        controlsUI.SetActive(false);

        cameraSensitivity.enabled = true;
        cameraCinemachine.enabled = true;

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        Time.timeScale = 1f; 
        isPaused = false;
    }
 

    public void Pause()
    {
        cameraSensitivity.enabled = false;
        cameraCinemachine.enabled = false;

        Cursor.lockState = CursorLockMode.None; // Enables cursor within the screen
        Cursor.visible = true;

        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("loading menu...");
        SceneManager.LoadScene("MainMenu");
    }


    public void QuitGame()
    {
        Debug.Log ("quitting game...");
        Application.Quit();
    }
}