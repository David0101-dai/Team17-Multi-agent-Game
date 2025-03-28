using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    // �ƶ�����
    public float xAxis = 0;
    public float yAxis = 0;

    // �������ƣ��޸�����Pressed״̬��
    public bool isJumpDown = false;
    public bool isJumpPressed = false;  // ����
    public bool isDashDown = false;
    public bool isDashPressed = false;  // ����
    public bool isAttackDown = false;
    public bool isAttackPressed = false; // �޸�
    public bool isCounterDown = false;
    public bool isCounterPressed = false; // ����
    public bool isAimSwordDown = false;
    public bool isAimSwordPressed = false;

    // ���λ��
    public Vector2 mousePosition = Vector2.zero;

    // ���붯������
    private InputAction _movementAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _attackAction;
    private InputAction _counterAction;
    private InputAction _aimSwordAction;
    private InputAction _mouseAction;

    // UI����״̬������ԭ�У�
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
            // �ϲ��ƶ�����
            var movement = _movementAction.ReadValue<Vector2>();
            xAxis = Mathf.Clamp(movement.x + (_uiRightPressed ? 1 : 0) + (_uiLeftPressed ? -1 : 0), -1f, 1f);
            yAxis = movement.y;

            // �ϲ��������루�޸�Pressed״̬��
            isJumpDown = _jumpAction.triggered || _uiJump;
            isJumpPressed = _jumpAction.IsPressed() || _uiJump;

            isDashDown = _dashAction.triggered || _uiDash;
            isDashPressed = false; // ���ﲻ��ʹ�ó����߼�

            isAttackDown = _attackAction.triggered || _uiAttack;
            isAttackPressed = _attackAction.IsPressed() || _uiAttack;  // �޸�

            isCounterDown = _counterAction.triggered || _uiCounter;
            isCounterPressed = _counterAction.IsPressed() || _uiCounter;

            isAimSwordDown = _aimSwordAction.triggered;
            isAimSwordPressed = _aimSwordAction.IsPressed();

            // �������
            mousePosition = _mouseAction.ReadValue<Vector2>();

            // ����˲ʱ״̬
            _uiDash = false;
            _uiJump = false;
            _uiAttack = false;
            _uiCounter = false;
        }
    }

    // ����ԭ��UI�ӿڲ���
    public void UI_Jump() => _uiJump = true;
    public void UI_Dash() => _uiDash = true;
    public void UI_Attack() => _uiAttack = true;
    public void UI_Counter() => _uiCounter = true;
    public void UI_SetMoveLeft(bool pressed) => _uiLeftPressed = pressed;
    public void UI_SetMoveRight(bool pressed) => _uiRightPressed = pressed;
}