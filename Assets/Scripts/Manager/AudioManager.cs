using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;  // 引入 UI 命名空间

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    // 添加两个滑条引用，分别控制背景音乐和音效的音量
    [SerializeField] public Slider bgmSlider;
    [SerializeField] public Slider sfxSlider;

    public bool playBgm;
    private int bgmIndex;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {
        // 初始化背景音乐滑条
        if (bgmSlider != null)
        {
            // 可设置默认值为当前背景音乐音量
            bgmSlider.value = bgm[bgmIndex].volume;
            // 注册滑条数值变化事件
            bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
        }

        // 初始化音效滑条
        if (sfxSlider != null)
        {
            if (sfx.Length > 0)
            {
                sfxSlider.value = sfx[0].volume;
            }
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }


    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (sfx[_sfxIndex].isPlaying)
            return;

        if(_source != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    // 当背景音乐滑条变化时，更新所有 BGM 的音量
    private void UpdateBGMVolume(float value)
    {
        foreach (AudioSource source in bgm)
        {
            source.volume = value;
        }
    }

    // 当音效滑条变化时，更新所有 SFX 的音量
    private void UpdateSFXVolume(float value)
    {
        foreach (AudioSource source in sfx)
        {
            source.volume = value;
        }
    }
}
