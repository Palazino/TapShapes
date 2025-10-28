using UnityEngine;

public class ShapeScorePenalty : MonoBehaviour
{
    [SerializeField] private int penaltyAmount = -5;
    [SerializeField] private bool breakCombo = false;

    private void OnMouseDown()
    {
        if (GameManager.Instance == null) return;

        // Applique une pénalité fixe, sans combo ni multiplicateur
        GameManager.Instance.AddPenalty(penaltyAmount, transform.position, showPopup: false, breakCombo: breakCombo);

        // Popup rouge distinct
        GameManager.Instance.SpawnScorePopup(penaltyAmount.ToString(), transform.position, Color.red);

        Destroy(gameObject);
    }
}
