using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
	public AudioMixerGroup mixerGroup;

	[Serializable]
	public class Sound
    {
		public string name;
		public AudioClip clip;

		[Range(0f, 1f)]
		public float volume = 0.75f;
        [Range(0f, 1f)]
        public float volumeVariance = 0.1f;

        [Range(0f, 1f)]
        public float pitch = 1f;
        [Range(0f, 1f)]
        public float pitchVariance = 0.1f;

		public bool loop = false;

		public AudioMixerGroup mixerGroup = null;

		[HideInInspector]
		public AudioSource source;

    }
	[SerializeField] private Sound[] sounds;

    private void Awake()
	{
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public static void PlaySound(string sound)
    {
        if (Instance == null || Instance.sounds == null)
            return;
		Sound s = Array.Find(Instance.sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public static void StopSound(string sound)
    {
		if (Instance == null || Instance.sounds == null)
			return;
		Sound s = Array.Find(Instance.sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }
		if (s.source.isPlaying)
			s.source.Stop();
    }
}
