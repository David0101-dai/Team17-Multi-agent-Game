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

    private int currency;

    private int player_currency;

    private IEnumerator Start()
    {
        // Wait until PlayerManager is properly initialized and data is loaded
        yield return new WaitUntil(() => PlayerManager.Instance != null && PlayerManager.Instance.currency != 0);

        // Now that PlayerManager is initialized, we can safely proceed with the rest of the initialization
        InitializeUI();

        currency = 0;
    }

    // Initialize UI elements here after PlayerManager is initialized
    private void InitializeUI()
    {
        dashCoolDown = SkillManager.Instance.Dash.cooldown;
        crystalCoolDown = SkillManager.Instance.Crystal.multiStackCooldown;
        swordCoolDown = SkillManager.Instance.Sword.cooldown;
        blackholeCoolDown = SkillManager.Instance.Blackhole.cooldown;
    }

    void Update()
    {
        player_currency = PlayerManager.Instance.currency;

        //Debug.Log("player currency " + player_currency);

        if(currency < player_currency){
            currency++;
        }else if(currency > player_currency){
            currency--;
        }
        
        souls.text = currency.ToString("#,#");  // 获取并显示当前金币数量
        
        if (SkillManager.Instance.Dash.dashUnlocked)
        {
            float dashCooldown = SkillManager.Instance.Dash.cooldown;
            float dashCurrent = SkillManager.Instance.Dash.cooldownTimer;
            dashImage.fillAmount = dashCurrent / dashCooldown;
            //SetCoolDownOf(dashImage);
        }

        if (SkillManager.Instance.Crystal.canMove)
        {
            float crystalCooldown = SkillManager.Instance.Crystal.cooldown;
            float crystalCurrent = SkillManager.Instance.Crystal.cooldownTimer;
            crystalImage.fillAmount = crystalCurrent / crystalCooldown;
            //SetCoolDownOf(crystalImage);
        }

        if (SkillManager.Instance.Sword.swordUnlocked)
        {
            float swordCooldown = SkillManager.Instance.Sword.cooldown;
            float swordCurrent = SkillManager.Instance.Sword.cooldownTimer;
            swordImage.fillAmount = swordCurrent / swordCooldown;
            //SetCoolDownOf(swordImage);
        }

        if (SkillManager.Instance.Blackhole.blackHole)
        {
            float blackholeCooldown = SkillManager.Instance.Blackhole.cooldown;
            float blackholeCurrent = SkillManager.Instance.Blackhole.cooldownTimer;
            blackholeImage.fillAmount = blackholeCurrent / blackholeCooldown;
            //SetCoolDownOf(blackholeImage);
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
