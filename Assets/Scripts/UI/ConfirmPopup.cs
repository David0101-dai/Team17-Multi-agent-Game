using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro; // 要加这个命名空间

public class ConfirmPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;      // “确认”按钮
    [SerializeField] private Button noButton;       // “取消”按钮

    private UnityAction onConfirmAction;            // 点击“确认”时的回调

    private void Awake()
    {
        // 给“确认”按钮绑定回调
        yesButton.onClick.AddListener(() =>
        {
            onConfirmAction?.Invoke();  // 如果不为 null，就调用它
            Hide();                     // 关闭弹窗
        });

        // 给“取消”按钮绑定回调
        noButton.onClick.AddListener(() =>
        {
            Hide();  // 仅关闭弹窗，不执行任何其他操作
        });
    }

    /// <summary>
    /// 显示确认弹窗
    /// </summary>
    /// <param name="message">提示文本</param>
    /// <param name="onConfirm">点击“确认”时执行的回调</param>
    public void Show(string message, UnityAction onConfirm)
    {
        messageText.text = message;
        onConfirmAction = onConfirm;
        gameObject.SetActive(true);  // 显示弹窗
    }

    /// <summary>
    /// 隐藏确认弹窗
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false); // 隐藏弹窗
    }
}
