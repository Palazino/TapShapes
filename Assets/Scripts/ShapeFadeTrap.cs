using System.Collections;
using UnityEngine;

public class ShapeFadeTrap : ShapeFade
{
    [Header("Effet Piège")]
    public AudioClip trapSound; 
    public float fallForce = 15f;

    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnMouseDown()
    {
        if (isFadingOut) return;
        StartCoroutine(TriggerTrapEffect());
    }

    protected override void AutoDestroy()
    {
        StartCoroutine(FadeOutWithRotation());
    }


    IEnumerator TriggerTrapEffect()
    {
        isFadingOut = true;

        if (trapSound && audioSource)
            audioSource.PlayOneShot(trapSound);

        yield return StartCoroutine(FadeOutWithPulse());

        CameraShaker.Instance?.Shake(0.2f, 0.3f);

        if (UpgradeEffects.Instance != null && UpgradeEffects.Instance.trapShieldActive)
        {
            // Bouclier consommé
            UpgradeEffects.Instance.trapShieldActive = false;
            Debug.Log("💥 Piège évité grâce au bouclier !");
        }
        else
        {
            // Pas de bouclier → mort
            GameManager.Instance?.InstantGameOver();

            ShapeSpawner spawner = UnityEngine.Object.FindFirstObjectByType<ShapeSpawner>();
            if (spawner != null)
            {
                spawner.TriggerMassFall(fallForce);
            }
        }
    }


}
