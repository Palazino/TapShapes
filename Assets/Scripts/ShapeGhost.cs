using UnityEngine;
using System.Collections;

public class ShapeGhost : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(GhostRoutine());
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
