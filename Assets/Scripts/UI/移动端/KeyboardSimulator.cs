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
        // ��ȡ�򴴽������豸
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            Debug.LogWarning("δ�ҵ������豸");
            return;
        }

        // ���Ͱ��������¼�
        SendKeyEvent(keyboard, targetKey, true);

        // ����Э�̷���̧���¼�
        StartCoroutine(ReleaseKey(keyboard));
    }

    private IEnumerator ReleaseKey(Keyboard keyboard)
    {
        yield return new WaitForSeconds(0.1f);
        SendKeyEvent(keyboard, targetKey, false);
    }

    private void SendKeyEvent(Keyboard keyboard, Key key, bool isPressed)
    {
        // ��������״̬�¼�
        var state = new KeyboardState();

        if (isPressed)
            state.Press(key);
        else
            state.Release(key);

        // ���������¼�
        InputSystem.QueueStateEvent(keyboard, state);
    }
}