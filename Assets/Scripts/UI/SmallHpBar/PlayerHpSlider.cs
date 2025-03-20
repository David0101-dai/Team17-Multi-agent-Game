using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSlider : MonoBehaviour
{
    public GameObject Player;
    private Slider slider;
    private Damageable damageable;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        damageable = Player.GetComponent<Damageable>();

        slider.maxValue = damageable.MaxHp.GetValue();
        slider.value = damageable.currentHp;
    }
    
     private void Update(){
        slider.maxValue = damageable.MaxHp.GetValue();
        slider.value = damageable.currentHp;
     }
     
    private void OnEnable()
    {
        damageable.OnTakeDamage += UpdateHpBar;
    }

    private void OnDisable()
    {
        damageable.OnTakeDamage -= UpdateHpBar;
    }

    private void UpdateHpBar(GameObject object1, GameObject object2)
    {
        slider.maxValue = damageable.MaxHp.GetValue();
        slider.value = damageable.currentHp;

        // Optionally, you can also add some logic here to update the UI when the player dies.
        if (damageable.currentHp <= 0)
        {
            // For example, set the value to 0 when the player is dead.
            slider.value = 0;
        }
    }
}
