using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    private class Sound
    {
        public string Name = "";
        public AudioClip AudioClip;
        [Range(0f, 1f)] public float Volume = .15f;
        [Range(-3f, 3f)] public float Pitch = 1f;
        public bool IsLoop = false;
        public AudioSource AudioSource { get; set; }
    }

    [SerializeField] List<Sound> _sounds;

    private void Awake()
    {
        if (InstanceManager.AudioManager != null)
        {
            Destroy(gameObject);
        }
        InstanceManager.AudioManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeAllSounds();
    }

    void InitializeAllSounds()
    {
        foreach (Sound sound in _sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.AudioClip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.loop = sound.IsLoop;
            sound.AudioSource = source;
        }
    }

    public void PlayClip(string clipName) => _sounds.FirstOrDefault(s => s.Name == clipName)?.AudioSource?.Play();

    public void StopClip(string clipName) => _sounds.FirstOrDefault(s => s.Name == clipName)?.AudioSource?.Stop();

    public void StopAllClips() => _sounds.ForEach(s => { if (s.AudioSource.isPlaying) s.AudioSource?.Stop(); });
}
