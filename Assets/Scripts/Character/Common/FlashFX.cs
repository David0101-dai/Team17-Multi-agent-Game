using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FlashFX : MonoBehaviour
{
    public List<Color> igniteColor;
    public List<Color> chillColor;
    public List<Color> shockColor;

    private SpriteRenderer sr;
    private Damageable damageable;

    [Header("Flash FX")]
    [SerializeField] private float flashTime = 0.1f;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        damageable = transform.GetComponent<Damageable>();
    }


    private void Update()
    {
        damageable.OnTakeDamage += (from, to) => UniTask.ToCoroutine(async () =>
       {
           sr.material.SetInt("_Flash", Convert.ToInt32(true));
           await UniTask.WaitForSeconds(flashTime);
           sr.material.SetInt("_Flash", Convert.ToInt32(false));
       });
    }

    public void RedBlink(bool isOn)
    {
        sr.material.SetInt("_Blink", Convert.ToInt32(isOn));
    }

    public void AlimentsFxFor(List<Color> colors, float seconds)
    {
        StartCoroutine(AlimentsFx(colors, seconds));
    }


    private IEnumerator AlimentsFx(List<Color> colors, float seconds)
    {
        var coroutine = StartCoroutine(RepeatingColorFx(colors));
        yield return new WaitForSeconds(seconds);
        StopCoroutine(coroutine);
        sr.color = Color.white;
    }

    private IEnumerator RepeatingColorFx(List<Color> colors)
    {
        while (true)
        {
            if (sr.color != colors[0])
            {
                sr.color = colors[0];
            }
            else
            {
                sr.color = colors[1];
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
