using UnityEngine;
using System.Collections;

public class ShapeGhost : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float lifeTime = 2.5f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(GhostRoutine());
        Invoke(nameof(AutoDestroy), lifeTime);
    }

    private void AutoDestroy()
    {
        // Si elle n’a pas encore été cliquée
        if (gameObject != null)
            GameManager.Instance?.DestroyShape(gameObject);
    }

    IEnumerator GhostRoutine()
    {
        while (true)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.AddScore(2, transform.position);
        GameManager.Instance.SpawnScorePopup("+2", transform.position, Color.cyan);

        Destroy(gameObject);
    }
}
