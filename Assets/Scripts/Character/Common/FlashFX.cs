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
    private Color originalColor;  // 保存原始颜色

    [Header("Flash FX")]
    [SerializeField] private float flashTime = 0.1f;
    public ParticleSystem igniteFx;
    public ParticleSystem chillFx;
    public ParticleSystem shockFx;
    public ParticleSystem dustFx;

    [Header("Hit Fix")]
    [SerializeField] private GameObject HitEffect;
    [SerializeField] private GameObject CriticalEffect;
    [SerializeField] private GameObject FireEffect;
    [SerializeField] private GameObject IceEffect;
    [SerializeField] private GameObject ShockEffect;
    public float yOffset = 1f;  // 偏移量（增加y轴上的高度）

    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePerfab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        damageable = transform.GetComponent<Damageable>();
        originalColor = sr.color;
    }

    private void Update()
    {
        damageable.OnTakeDamage += (from, to) => UniTask.ToCoroutine(async () =>
        {
            sr.material.SetInt("_Flash", Convert.ToInt32(true));
            await UniTask.WaitForSeconds(flashTime);
            sr.material.SetInt("_Flash", Convert.ToInt32(false));
        });

        // 更新冷却计时器
        if (afterImageCooldownTimer > 0)
        {
            afterImageCooldownTimer -= Time.deltaTime;
        }
    }

    // 用于状态效果的颜色闪烁
    public void AlimentsFxFor(List<Color> colors, float seconds)
    {
        StartCoroutine(AlimentsFx(colors, seconds));
    }

    private IEnumerator AlimentsFx(List<Color> colors, float seconds)
    {
        var coroutine = StartCoroutine(RepeatingColorFx(colors));
        yield return new WaitForSeconds(seconds);
        StopCoroutine(coroutine);

        // 恢复原始颜色
        sr.color = originalColor;

        // 停止特效
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

    public void RedBlink(bool isOn)
    {
        sr.material.SetInt("_Blink", Convert.ToInt32(isOn));
    }

    public void CreatAfterImage()
    {
        if (afterImageCooldownTimer <= 0)  // 改为小于等于 0
        {
            afterImageCooldownTimer = afterImageCooldown;

            // 定义分身相对于角色的偏移量
            Vector3 offset = new Vector3(0, 1.5f, 0);  // 1f 是向上的偏移量，可以根据需求调整

            // 创建分身并设置它的位置
            Vector3 afterImagePosition = transform.position + offset;
            //Debug.Log("AfterImage Position: " + afterImagePosition);  // 调试位置
            GameObject newAfterImage = Instantiate(afterImagePerfab, afterImagePosition, transform.rotation);
            newAfterImage.GetComponent<AfterImageFx>().SetupAfterImage(colorLooseRate, sr.sprite);
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

    public void playDust()
    {
        if(dustFx != null)
        {
            dustFx.Play();
        }
    }

    public void MakeTransparent(bool _transparent)
    {
        if(_transparent){
            sr.color = Color.clear;
        }else{
            sr.color = originalColor;
        }
    }
}
