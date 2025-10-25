using UnityEngine;
using TMPro;

public class ScorePop : MonoBehaviour
{
    [Header("Animation")]
    public float moveUpSpeed = 30f;
    public float fadeSpeed = 2f;

    [Header("Référence")]
    public TextMeshProUGUI textMesh; // à glisser dans l'inspecteur

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // Mouvement vertical
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Disparition progressive
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
        {
            textMesh.text = value;
            ApplyColorByValue(value);
        }
    }

    private void ApplyColorByValue(string textValue)
    {
        // On essaie d'extraire la valeur numérique (ex : "+5" -> 5)
        if (int.TryParse(textValue.Replace("+", ""), out int value))
        {
            Color chosenColor = Color.white;

            if (value <= 1)
                chosenColor = Color.white;
            else if (value <= 4)
                chosenColor = new Color(0.3f, 1f, 0.3f); // Vert clair
            else if (value <= 9)
                chosenColor = new Color(1f, 0.9f, 0.3f); // Jaune doré
            else
                chosenColor = new Color(1f, 0.3f, 0.4f); // Rouge / Magenta

            textMesh.color = chosenColor;
        }
    }
}
