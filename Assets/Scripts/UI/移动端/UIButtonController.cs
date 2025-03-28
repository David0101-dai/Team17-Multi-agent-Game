using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InputController inputController;

    public enum ButtonType
    {
        Jump,
        Dash,
        Attack,
        Counter,
        MoveLeft,
        MoveRight
    }

    [Header("��ť����")]
    public ButtonType buttonType;

    [Header("��Ծ��ȴ")]
    [SerializeField] private float jumpCooldown = 0.2f;
    private float _lastJumpTime;

    // ɾ��Start�����е�onClick��

    // ͳһͨ��Pointer�¼�����
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Jump:
                if (Time.time - _lastJumpTime < jumpCooldown) return;
                _lastJumpTime = Time.time;
                inputController.UI_Jump();
                break;
            case ButtonType.Dash:
                inputController.UI_Dash();
                break;
            case ButtonType.Attack:
                inputController.UI_Attack();
                break;
            case ButtonType.Counter:
                inputController.UI_Counter();
                break;
            case ButtonType.MoveLeft:
                inputController.UI_SetMoveLeft(true);
                break;
            case ButtonType.MoveRight:
                inputController.UI_SetMoveRight(true);
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.MoveLeft:
                inputController.UI_SetMoveLeft(false);
                break;
            case ButtonType.MoveRight:
                inputController.UI_SetMoveRight(false);
                break;
        }
    }
}