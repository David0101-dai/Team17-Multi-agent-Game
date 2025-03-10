using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    public static bool isPaused = false; 

        void Awake()
    {

        if (pauseMenuUI != null)
    {
        pauseMenuUI.SetActive(isPaused);
    }
    else
    {
        Debug.LogWarning("pauseMenuUI is missing!");
    }

    }


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

        Time.timeScale = isPaused ? 0 : 1;

        pauseMenuUI.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
    public void TogglePauseUI()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;

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
    public void closeUI()
    {
        pauseMenuUI.SetActive(false);
    }
}
