using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    public static bool isPaused = false; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // «–ªª ±º‰Àı∑≈
        Time.timeScale = isPaused ? 0 : 1;

        pauseMenuUI.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
    public void ResumeGame()
    {
        TogglePause();
    }

    public void QuitGame()
    {
        AudioManager.instance.StopAllBGM();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
