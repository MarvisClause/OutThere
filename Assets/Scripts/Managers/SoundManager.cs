using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Sound manager will control all music in game
public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        // Name of the sound
        public string name;
        // Audio clip
        public AudioClip audioClip;
        // Volume of the sound
        [Range(0f, 1f)]
        public float volume;
        // Pitch of the sound
        [Range(0.1f, 3f)]
        public float pitch;
        // Audio mixer
        public AudioMixerGroup mixerGroup;
        // Audio clip
        [HideInInspector] public AudioSource audioSource;
    }

    #region Variables

    // Sounds
    [SerializeField] private Sound[] _sounds; 
    // Singleton
    private static SoundManager _instance;
    public static SoundManager GetInstance()
    {
        if (_instance == null)
        {
            Debug.LogWarning("Instance of sound manager is null referenced!");
            throw new System.Exception("Instance of sound manager is null referenced!");
        }
        return _instance;
    }

    #endregion

    #region Unity

    // Start is called before the first frame update
    private void Awake()
    {
        _instance = this;
        for (int i = 0; i < _sounds.Length; i++)
        {
            _sounds[i].audioSource = gameObject.AddComponent<AudioSource>();
            // Setting up the sound
            _sounds[i].audioSource.outputAudioMixerGroup = _sounds[i].mixerGroup;
            _sounds[i].audioSource.clip = _sounds[i].audioClip;
            _sounds[i].audioSource.volume = _sounds[i].volume;
            _sounds[i].audioSource.pitch = _sounds[i].pitch;
        }
    }

    #endregion

    #region Methods

    // Find sound by name
    private Sound FindSoundByName(string soundName)
    {
        foreach (Sound sound in _sounds)
        {
            if (sound.name.Equals(soundName))
            {
                // Return sound
                return sound;
            }
        }
        return null;
    }

    // Play entered name one time
    public void PlaySound(string soundName, bool isLoop = false)
    {
        // Find sound
        Sound soundToPlay = FindSoundByName(soundName);
        if (soundToPlay == null)
        {
            Debug.LogError("There is no " + soundName + " in array");
        }
        // Play sound one time
        soundToPlay.audioSource.loop = isLoop;
        soundToPlay.audioSource.Play();
    }

    public void StopSound(string soundName)
    {
        // Find sound
        Sound soundToPlay = FindSoundByName(soundName);
        if (soundToPlay == null)
        {
            Debug.LogError("There is no " + soundName + " in array");
        }
        // Stop sound
        soundToPlay.audioSource.Stop();
    }

    #endregion
}
