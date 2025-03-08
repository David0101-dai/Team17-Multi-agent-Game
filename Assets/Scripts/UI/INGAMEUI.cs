using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INGAMEUI : MonoBehaviour
{
    [SerializeField] private Image dashImage;
    //[SerializeField] private Image counterImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;


    [SerializeField] private float dashCoolDown;
    //[SerializeField] private float counterCoolDown;
    [SerializeField] private float crystalCoolDown;
    [SerializeField] private float swordCoolDown;
    [SerializeField] private float blackholeCoolDown;
    [SerializeField] private float flaskCoolDown;

    [SerializeField] private TextMeshProUGUI souls;


    //private SkillManager skills;

    // Start is called before the first frame update
    void Start()
    {
        dashCoolDown = SkillManager.Instance.Dash.cooldown;
        //counterCoolDown = SkillManager.Instance.Counter.cooldown;
        crystalCoolDown = SkillManager.Instance.Crystal.multiStackCooldown;
        swordCoolDown = SkillManager.Instance.Sword.cooldown;
        blackholeCoolDown = SkillManager.Instance.Blackhole.cooldown;
        //blackholeCoolDown = SkillManager.Instance.Blackhole.cooldown;
        //skills = SkillManager.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        souls.text = PlayerManager.Instance.currentCurrencyAmount().ToString("#,#");


        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.Dash.dashUnlocked)
        {
            SetCoolDownOf(dashImage);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetCoolDownOf(counterImage);
        }*/

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
        //checkCoolDownOf(counterImage, counterCoolDown);
        checkCoolDownOf(crystalImage, crystalCoolDown);
        checkCoolDownOf(swordImage, swordCoolDown);
        checkCoolDownOf(blackholeImage, blackholeCoolDown);

        checkCoolDownOf(flaskImage, Inventory.Instance.flaskCooldown);

        //checkCoolDownOf(dashImage, skills.Dash.cooldown);
        //checkCoolDownOf(counterImage, skills.Counter.cooldown);
        //checkCoolDownOf(crystalImage, skills.Crystal.cooldown);
        //checkCoolDownOf(swordImage, skills.Sword.cooldown);
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
