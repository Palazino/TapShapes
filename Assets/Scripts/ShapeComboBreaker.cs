using UnityEngine;

public class ShapeComboBreaker : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2.5f;
    private void Start()
    {
        // Lance la destruction automatique
        Invoke(nameof(AutoDestroy), lifeTime);
    }

    private void AutoDestroy()
    {
        // Si elle n’a pas encore été cliquée
        if (gameObject != null)
            GameManager.Instance?.DestroyShape(gameObject);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.ResetCombo();

        // Popup jaune "Combo cassé"
        GameManager.Instance.SpawnScorePopup("BREAK !", transform.position, Color.yellow);

        Destroy(gameObject);
    }
}
