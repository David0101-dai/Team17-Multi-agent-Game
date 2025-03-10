using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // 销毁重复的实例，确保只有一个实例
        }
    }

    public void ReStartGame(){
        Scene  scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ReturnHome(){
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        AudioManager.instance.PlayBGM(0);
        Debug.Log($"播放背景音乐");
    }

}
