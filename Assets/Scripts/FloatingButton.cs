using UnityEngine;

public class FloatingButton : MonoBehaviour
{
    [Header("Amplitude du mouvement")]
    [SerializeField] private float amplitude = 10f;  // distance max du mouvement

    [Header("Vitesse du mouvement")]
    [SerializeField] private float speed = 2f;       // vitesse de l’oscillation

    private Vector3 startPos;
    private float phaseOffset; // décalage unique par bouton

    void Start()
    {
        startPos = transform.localPosition;
        // Donne un décalage aléatoire à chaque bouton
        phaseOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed + phaseOffset) * amplitude;
        float offsetX = Mathf.Cos(Time.time * speed * 0.8f + phaseOffset) * amplitude * 0.5f;
        transform.localPosition = startPos + new Vector3(offsetX, offsetY, 0);
    }
}
