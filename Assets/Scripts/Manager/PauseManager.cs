using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject confirmUI;
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
        if (confirmUI.activeSelf)
        {
            confirmUI.SetActive(false);
        }

        //Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible = isPaused;
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
    public void back()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;

        confirmUI.SetActive(isPaused);
    }
    public void confirmQuit()
    {
        pauseMenuUI.SetActive(false);
        confirmUI.SetActive(true);
    }
    public void QuitGame()
    {
        AudioManager.instance.StopAllBGM();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        confirmUI.SetActive(false);
    }
    public void closeUI()
    {
        pauseMenuUI.SetActive(false);
    }
}
