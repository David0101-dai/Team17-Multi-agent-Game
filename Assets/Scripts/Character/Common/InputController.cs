using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    // 移动控制
    public float xAxis = 0;
    public float yAxis = 0;

    // 动作控制（修复所有Pressed状态）
    public bool isJumpDown = false;
    public bool isJumpPressed = false;  // 新增
    public bool isDashDown = false;
    public bool isDashPressed = false;  // 新增
    public bool isAttackDown = false;
    public bool isAttackPressed = false; // 修复
    public bool isCounterDown = false;
    public bool isCounterPressed = false; // 新增
    public bool isAimSwordDown = false;
    public bool isAimSwordPressed = false;

    // 鼠标位置
    public Vector2 mousePosition = Vector2.zero;

    // 输入动作引用
    private InputAction _movementAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _attackAction;
    private InputAction _counterAction;
    private InputAction _aimSwordAction;
    private InputAction _mouseAction;

    // UI输入状态（保持原有）
    private bool _uiJump;
    private bool _uiDash;
    private bool _uiAttack;
    private bool _uiCounter;
    private bool _uiLeftPressed;
    private bool _uiRightPressed;

    private void Start()
    {
        var input = GetComponent<PlayerInput>();
        _movementAction = input.actions["Movement"];
        _jumpAction = input.actions["Jump"];
        _dashAction = input.actions["Dash"];
        _attackAction = input.actions["Attack"];
        _counterAction = input.actions["Counter"];
        _aimSwordAction = input.actions["AimSword"];
        _mouseAction = input.actions["Mouse"];
    }

    private void Update()
    {
        if (!PauseManager.isPaused)
        {
            // 合并移动输入
            var movement = _movementAction.ReadValue<Vector2>();
            xAxis = Mathf.Clamp(movement.x + (_uiRightPressed ? 1 : 0) + (_uiLeftPressed ? -1 : 0), -1f, 1f);
            yAxis = movement.y;

            // 合并动作输入（修复Pressed状态）
            isJumpDown = _jumpAction.triggered || _uiJump;
            isJumpPressed = _jumpAction.IsPressed() || _uiJump;

            isDashDown = _dashAction.triggered || _uiDash;
            isDashPressed = false; // 这里不再使用长按逻辑

            isAttackDown = _attackAction.triggered || _uiAttack;
            isAttackPressed = _attackAction.IsPressed() || _uiAttack;  // 修复

            isCounterDown = _counterAction.triggered || _uiCounter;
            isCounterPressed = _counterAction.IsPressed() || _uiCounter;

            isAimSwordDown = _aimSwordAction.triggered;
            isAimSwordPressed = _aimSwordAction.IsPressed();

            // 鼠标输入
            mousePosition = _mouseAction.ReadValue<Vector2>();

            // 重置瞬时状态
            _uiDash = false;
            _uiJump = false;
            _uiAttack = false;
            _uiCounter = false;
        }
    }

    // 保持原有UI接口不变
    public void UI_Jump() => _uiJump = true;
    public void UI_Dash() => _uiDash = true;
    public void UI_Attack() => _uiAttack = true;
    public void UI_Counter() => _uiCounter = true;
    public void UI_SetMoveLeft(bool pressed) => _uiLeftPressed = pressed;
    public void UI_SetMoveRight(bool pressed) => _uiRightPressed = pressed;
}