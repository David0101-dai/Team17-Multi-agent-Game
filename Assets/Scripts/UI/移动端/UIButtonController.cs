using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandler : MonoBehaviour
{
    public InputController inputController;

    public enum ButtonType
    {
        Jump,
        Dash,
        Attack,
        Counter,
        AimSword
    }

    public ButtonType buttonType;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            switch (buttonType)
            {
                case ButtonType.Jump:
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
                case ButtonType.AimSword:
                    inputController.UI_AimSword();
                    break;
            }
        });
    }
}