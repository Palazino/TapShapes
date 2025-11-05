using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;
    public bool loop = false;

    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Liste des sons")]
    public List<Sound> sounds = new List<Sound>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Création automatique des AudioSources
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    public void Play(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning($"❌ AudioManager : son '{name}' introuvable !");
            return;
        }

        if (s.clip == null)
        {
            Debug.LogWarning($"⚠️ Le son '{name}' n'a pas de clip assigné !");
            return;
        }

        Debug.Log($"▶️ Lecture du son : {s.name}");

        // Applique des variations légères
        s.source.pitch = s.pitch * Random.Range(0.95f, 1.05f);
        s.source.volume = s.volume * Random.Range(0.9f, 1.1f);

        try
        {
            s.source.Play();
        }
        catch
        {
            Debug.LogWarning($"⚠️ Erreur lors de la lecture de '{s.name}', utilisation du fallback.");
            AudioSource.PlayClipAtPoint(s.clip, Camera.main?.transform.position ?? Vector3.zero);
        }
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s != null && s.source.isPlaying)
        {
            s.source.Stop();
            Debug.Log($"⏹️ Son arrêté : {s.name}");
        }
    }
}
