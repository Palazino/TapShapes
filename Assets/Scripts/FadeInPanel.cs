using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInPanel : MonoBehaviour
{
    public CanvasGroup panel;
    public float fadeDuration = 0.5f;

    void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        panel.alpha = 0;
        while (panel.alpha < 1f)
        {
            panel.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
