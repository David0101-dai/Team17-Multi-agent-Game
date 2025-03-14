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
    public ParticleSystem igniteFx;
    public ParticleSystem chillFx;
    public ParticleSystem shockFx;

    [Header("Hit Fix")]
    [SerializeField] private GameObject HitEffect;
    public float yOffset = 1f;  // 偏移量（增加y轴上的高度）


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
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
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

    public void CreatHitFX(Transform _target){
       float zRotation = UnityEngine.Random.Range(-90,90);
       float xPosition = UnityEngine.Random.Range(-.5f, .5f);    
       float yPosition = UnityEngine.Random.Range(-.5f,  .5f);

        // 修改 yPosition，增加一个固定的偏移量，比如 1f
        yPosition += yOffset; // 将 yPosition 向上偏移 1f

       GameObject newHitFx = Instantiate(HitEffect, _target.position + new Vector3(xPosition,yPosition),Quaternion.identity);

       newHitFx.transform.Rotate(new Vector3(0,0,zRotation));       
    }
}
