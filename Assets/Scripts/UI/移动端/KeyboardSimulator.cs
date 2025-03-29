using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;
using System.Collections;

public class KeyboardSimulator : MonoBehaviour, IPointerDownHandler
{
    public Key targetKey = Key.Escape;

    public void OnPointerDown(PointerEventData eventData)
    {
        // 获取或创建键盘设备
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            Debug.LogWarning("未找到键盘设备");
            return;
        }

        // 发送按键按下事件
        SendKeyEvent(keyboard, targetKey, true);

        // 启动协程发送抬起事件
        StartCoroutine(ReleaseKey(keyboard));
    }

    private IEnumerator ReleaseKey(Keyboard keyboard)
    {
        yield return new WaitForSeconds(0.1f);
        SendKeyEvent(keyboard, targetKey, false);
    }

    private void SendKeyEvent(Keyboard keyboard, Key key, bool isPressed)
    {
        // 创建键盘状态事件
        var state = new KeyboardState();

        if (isPressed)
            state.Press(key);
        else
            state.Release(key);

        // 创建输入事件
        InputSystem.QueueStateEvent(keyboard, state);
    }
}