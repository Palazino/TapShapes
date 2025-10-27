using UnityEngine;

public class ShapeScorePenalty : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance == null) return;

        // Enlève des points
        GameManager.Instance.AddScore(-5, transform.position);

        // Affiche un popup rouge
        GameManager.Instance.SpawnScorePopup("-5", transform.position, Color.red);

        Destroy(gameObject);
    }
}
