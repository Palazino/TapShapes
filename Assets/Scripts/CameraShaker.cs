using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    Vector3 originalPosition;

    void Awake()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.5f, 0.5f);
        }
    }


    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
