using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    // 保持所有原有public变量不变
    public float xAxis = 0;
    public float yAxis = 0;
    public bool isJumpDown = false;
    public bool isDashDown = false;
    public bool isAttackDown = false;
    public bool isCounterDown = false;
    public bool isAimSwordDown = false;
    public bool isJumpPressed = false;
    public bool isDashPressed = false;
    public bool isAttackPressed = false;
    public bool isCounterPressed = false;
    public bool isAimSwordPressed = false;
    public Vector2 mousePosition = Vector2.zero;

    // 仅添加UI状态变量
    private bool _uiJump;
    private bool _uiDash;
    private bool _uiAttack;
    private bool _uiCounter;
    private bool _uiAimSword;

    private void Update()
    {
        if (!PauseManager.isPaused)
        {
            // 保持原有输入处理逻辑不变
            var movement = GetComponent<PlayerInput>().actions["Movement"].ReadValue<Vector2>();
            xAxis = movement.x;
            yAxis = movement.y;

            // 合并UI按钮状态（唯一新增逻辑）
            isJumpDown = GetComponent<PlayerInput>().actions["Jump"].triggered || _uiJump;
            isDashDown = GetComponent<PlayerInput>().actions["Dash"].triggered || _uiDash;
            isAttackDown = GetComponent<PlayerInput>().actions["Attack"].triggered || _uiAttack;
            isCounterDown = GetComponent<PlayerInput>().actions["Counter"].triggered || _uiCounter;
            isAimSwordDown = GetComponent<PlayerInput>().actions["AimSword"].triggered || _uiAimSword;

            // 每帧重置状态
            _uiJump = false;
            _uiDash = false;
            _uiAttack = false;
            _uiCounter = false;
            _uiAimSword = false;

            // 保持原有鼠标逻辑不变
            mousePosition = GetComponent<PlayerInput>().actions["Mouse"].ReadValue<Vector2>();
        }
    }

    // 新增纯UI接口方法
    public void UI_Jump() => _uiJump = true;
    public void UI_Dash() => _uiDash = true;
    public void UI_Attack() => _uiAttack = true;
    public void UI_Counter() => _uiCounter = true;
    public void UI_AimSword() => _uiAimSword = true;
}