using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShapeFade : MonoBehaviour
{
    protected SpriteRenderer sr;

    [Header("Durées")]
    public float fadeDuration = 0.5f;
    public float lifeTime = 3f;

    protected bool isFadingOut = false;

    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        StartCoroutine(FadeIn());
        Invoke(nameof(AutoDestroy), lifeTime);
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            yield return null;
        }
    }

    protected virtual void OnMouseDown()
    {
        // Empêche le comportement de base sur les formes spéciales
        if (GetComponent<ShapeScorePenalty>() != null
            || GetComponent<ShapeComboBreaker>() != null
            || GetComponent<ShapeGhost>() != null)
            return;

        if (!isFadingOut)
        {
            isFadingOut = true; // anti double-clic instantané
            StartCoroutine(FadeOutWithPulse());
            GameManager.Instance?.AddScore(1, transform.position);
        }
    }


    protected virtual void AutoDestroy()
    {
        if (!isFadingOut)
        {
            GameManager.Instance?.LoseLife(); 
            StartCoroutine(FadeOutWithRotation());
        }
    }


    protected IEnumerator FadeOutWithRotation()
    {
        isFadingOut = true;

        float t = 0;
        Color startColor = sr.color;
        float rotationSpeed = 0f;
        float maxRotationSpeed = 720f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;

            // Fade
            float alpha = Mathf.Lerp(1f, 0f, progress);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Rotation accélérée
            rotationSpeed = Mathf.Lerp(0f, maxRotationSpeed, progress);
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(gameObject);
    }

    public IEnumerator FadeOutWithPulse()
    {
        isFadingOut = true;

        float t = 0;
        Color startColor = sr.color;
        Vector3 baseScale = transform.localScale;
        float pulseStrength = 1.2f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;

            // Fade
            float alpha = Mathf.Lerp(1f, 0f, progress);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Pulsation
            float scaleFactor = 1 + Mathf.Sin(progress * Mathf.PI) * (pulseStrength - 1);
            transform.localScale = baseScale * scaleFactor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
