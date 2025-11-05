using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Vitesse de rotation en degrés par seconde")]
    public float rotationSpeed = 20f;

    [Tooltip("Axe de rotation (par défaut Z pour un objet 2D)")]
    public Vector3 rotationAxis = Vector3.forward;

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
