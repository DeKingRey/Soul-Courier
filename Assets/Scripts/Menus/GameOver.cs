using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public PauseMenu pauseMenu;

    public void PreventPause()
    {
        pauseMenu.gameOverPause = true;
    }

    public void PauseGameOver()
    {
        pauseMenu.Pause();
    }

    public void PlayAgain()
    {
        PlayerStats.Instance.ResetStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        pauseMenu.Resume();
    }
}
