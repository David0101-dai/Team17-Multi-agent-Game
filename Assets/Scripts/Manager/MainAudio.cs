using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

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


    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
        else if (!bgm[bgmIndex].isPlaying)
        {
            PlayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if (_sfxIndex < sfx.Length && !sfx[_sfxIndex].isPlaying)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index)
    {
        if (_index < sfx.Length)
        {
            sfx[_index].Stop();
        }
    }

    public void PlayRandomBGM()
    {
        if (bgm.Length == 0) return;

        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        if (_bgmIndex < bgm.Length)
        {
            bgmIndex = _bgmIndex;
            StopAllBGM();
            bgm[bgmIndex].Play();
        }
    }

    public void StopAllBGM()
    {
        foreach (var source in bgm)
        {
            source.Stop();
        }
    }

    // ³¡¾°ÇÐ»»Ê±×Ô¶¯Í£Ö¹ËùÓÐÒôÀÖ
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllBGM();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // È¡Ïû¼àÌý£¬·ÀÖ¹ÄÚ´æÐ¹Â©
    }
}
