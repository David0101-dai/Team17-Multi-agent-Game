using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(fadeCanvasGroup.transform.root.gameObject); // ����Canvas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float timer = 0;
        fadeCanvasGroup.alpha = startAlpha;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
