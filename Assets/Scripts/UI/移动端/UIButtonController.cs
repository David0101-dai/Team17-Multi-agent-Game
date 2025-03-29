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

    [Header("按钮类型")]
    public ButtonType buttonType;

    [Header("跳跃冷却")]
    [SerializeField] private float jumpCooldown = 0.2f;
    private float _lastJumpTime;

    // 新增变量
    [Header("Dash冷却")]
    [SerializeField] private float dashCooldown = 0.5f;
    private float _lastDashTime;

    // 删除Start方法中的onClick绑定

    // 统一通过Pointer事件处理
    public void OnPointerDown(PointerEventData eventData)
    {
        switch(buttonType)
        {
            case ButtonType.Jump:
                if(Time.time - _lastJumpTime < jumpCooldown) return;
                _lastJumpTime = Time.time;
                inputController.UI_Jump();
                break;
            case ButtonType.Dash:
                if (Time.time - _lastDashTime < dashCooldown) return; // 新增冷却
                _lastDashTime = Time.time;
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
        switch(buttonType)
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