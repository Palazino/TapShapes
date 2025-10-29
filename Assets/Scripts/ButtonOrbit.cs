using UnityEngine;

public class ButtonOrbit : MonoBehaviour
{
    [Header("Vitesse de rotation (degrés par seconde)")]
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Sens de rotation (horaire = positif, antihoraire = négatif)")]
    [SerializeField] private bool clockwise = true;

    void Update()
    {
        float direction = clockwise ? -1f : 1f;
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime * direction);

        // ✅ Garde les boutons droits (réinitialise leur rotation locale)
        foreach (Transform child in transform)
        {
            child.rotation = Quaternion.identity;
        }
    }
}
