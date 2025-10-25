using UnityEngine;
using TMPro;

public class ScorePop : MonoBehaviour
{
    public float moveUpSpeed = 30f;
    public float fadeSpeed = 2f;
    private CanvasGroup canvasGroup;

    public TextMeshProUGUI textMesh; // ← à glisser dans l'inspecteur

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        if (canvasGroup != null)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0f)
                Destroy(gameObject);
        }
    }

    public void SetText(string value)
    {
        if (textMesh != null)
            textMesh.text = value;
    }
}
