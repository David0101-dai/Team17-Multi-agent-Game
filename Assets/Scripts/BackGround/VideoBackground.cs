// VideoBackground.cs
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage), typeof(VideoPlayer))]
public class VideoBackground : MonoBehaviour
{
    [Header("��Ƶ��Դ")]
    public VideoClip videoClip;

    [Header("��������")]
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnAwake = true;

    private RawImage rawImage;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        videoPlayer = GetComponent<VideoPlayer>();

        SetupVideoPlayer();
    }

    private void SetupVideoPlayer()
    {
        // ������Ƶ���
        videoPlayer.playOnAwake = playOnAwake;
        videoPlayer.isLooping = loop;
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = VideoRenderMode.APIOnly;

        // ������Ⱦ����
        var renderTexture = new RenderTexture(1920, 1080, 24);
        videoPlayer.targetTexture = renderTexture;
        rawImage.texture = renderTexture;

        // �Զ�����ߴ�
        rawImage.rectTransform.anchorMin = Vector2.zero;
        rawImage.rectTransform.anchorMax = Vector2.one;
        rawImage.rectTransform.sizeDelta = Vector2.zero;
    }

    public void Play() => videoPlayer.Play();
    public void Pause() => videoPlayer.Pause();
}