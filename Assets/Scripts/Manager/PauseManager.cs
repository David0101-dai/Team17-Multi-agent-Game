using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;  // �����Ϸ�Ƿ�����ͣ

    void Update()
    {
        // ���� "Escape" �л���ͣ״̬
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // �л�ʱ������
        Time.timeScale = isPaused ? 0 : 1;

        // ��ʾ/������ͣ�˵�
        if (isPaused)
        {
            Debug.Log("Game Paused");
        }
        else
        {
            Debug.Log("Game Resumed");
        }
    }
}
