using UnityEngine;

public class ShapeComboBreaker : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.ResetCombo();

        // Popup jaune "Combo cassé"
        GameManager.Instance.SpawnScorePopup("Combo cassé !", transform.position, Color.yellow);

        Destroy(gameObject);
    }
}
