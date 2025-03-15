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
    [SerializeField] private GameObject CriticalEffect;
    [SerializeField] private GameObject FireEffect;
    [SerializeField] private GameObject IceEffect;
    [SerializeField] private GameObject ShockEffect;
    public float yOffset = 1f;  // 偏移量（增加y轴上的高度）

    private Coroutine repeatingColorCoroutine;

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

    public void CreatHitFX(Transform _target, int HitEffect_id)
    {
        float zRotation = UnityEngine.Random.Range(-90, 90);
        float xPosition = UnityEngine.Random.Range(-.5f, .5f);    
        float yPosition = UnityEngine.Random.Range(-.5f,  .5f);
        GameObject newHitFx;

        yPosition += yOffset; // 将 yPosition 向上偏移 1f

        // 使用 switch 来简化条件判断
        switch (HitEffect_id)
        {
            case 0:
                newHitFx = Instantiate(HitEffect, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
                break;
            case 1:
                newHitFx = Instantiate(CriticalEffect, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
                break;
            case 2:
                newHitFx = Instantiate(FireEffect, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
                break;
            case 3:
                newHitFx = Instantiate(IceEffect, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
                break;
            case 4:
                newHitFx = Instantiate(ShockEffect, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
                break;
            default:
                Debug.LogWarning("Unknown HitEffect_id");
                return;
        }

        newHitFx.transform.Rotate(new Vector3(0, 0, zRotation));       
    }
}
