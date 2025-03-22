using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro; // Ҫ����������ռ�

public class ConfirmPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;      // ��ȷ�ϡ���ť
    [SerializeField] private Button noButton;       // ��ȡ������ť

    private UnityAction onConfirmAction;            // �����ȷ�ϡ�ʱ�Ļص�

    private void Awake()
    {
        // ����ȷ�ϡ���ť�󶨻ص�
        yesButton.onClick.AddListener(() =>
        {
            onConfirmAction?.Invoke();  // �����Ϊ null���͵�����
            Hide();                     // �رյ���
        });

        // ����ȡ������ť�󶨻ص�
        noButton.onClick.AddListener(() =>
        {
            Hide();  // ���رյ�������ִ���κ���������
        });
    }

    /// <summary>
    /// ��ʾȷ�ϵ���
    /// </summary>
    /// <param name="message">��ʾ�ı�</param>
    /// <param name="onConfirm">�����ȷ�ϡ�ʱִ�еĻص�</param>
    public void Show(string message, UnityAction onConfirm)
    {
        messageText.text = message;
        onConfirmAction = onConfirm;
        gameObject.SetActive(true);  // ��ʾ����
    }

    /// <summary>
    /// ����ȷ�ϵ���
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false); // ���ص���
    }
}
