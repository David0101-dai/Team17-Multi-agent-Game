using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    // Start is called before the first frame update 
    public void ContinueGame(){
        SceneManager.LoadScene(sceneName);
    }

      public void StartNewGame(){
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame(){
       #if UNITY_EDITOR
        Debug.Log("Exit game is called in the editor.");
    #else
        Application.Quit();
    #endif
    }
}

