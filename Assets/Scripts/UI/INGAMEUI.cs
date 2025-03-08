using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INGAMEUI : MonoBehaviour
{
    [SerializeField] private Image dashImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;
    [SerializeField] private float dashCoolDown;
    [SerializeField] private float crystalCoolDown;
    [SerializeField] private float swordCoolDown;
    [SerializeField] private float blackholeCoolDown;
    [SerializeField] private float flaskCoolDown;

    [SerializeField] private TextMeshProUGUI souls;

    void Start()
    {
        dashCoolDown = SkillManager.Instance.Dash.cooldown;
        crystalCoolDown = SkillManager.Instance.Crystal.multiStackCooldown;
        swordCoolDown = SkillManager.Instance.Sword.cooldown;
        blackholeCoolDown = SkillManager.Instance.Blackhole.cooldown;
    }
    void Update()
    {
        souls.text = PlayerManager.Instance.currentCurrencyAmount().ToString("#,#");


        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.Dash.dashUnlocked)
        {
            SetCoolDownOf(dashImage);
        }

        if (Input.GetKeyDown(KeyCode.Q) && SkillManager.Instance.Crystal.canMove)
        {
            SetCoolDownOf(crystalImage);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && SkillManager.Instance.Sword.swordUnlocked)
        {
            SetCoolDownOf(swordImage);
        }

        if (Input.GetKeyDown(KeyCode.F) && SkillManager.Instance.Blackhole.blackHole)
        {
            SetCoolDownOf(blackholeImage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.Instance.GetEquipmentByType(EquipmentType.Flask) != null)
        {
            SetCoolDownOf(flaskImage);
        }

        checkCoolDownOf(dashImage, dashCoolDown);
        checkCoolDownOf(crystalImage, crystalCoolDown);
        checkCoolDownOf(swordImage, swordCoolDown);
        checkCoolDownOf(blackholeImage, blackholeCoolDown);
        checkCoolDownOf(flaskImage, Inventory.Instance.flaskCooldown);
    }

    private void SetCoolDownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void checkCoolDownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
