using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] playlist;
    public AudioSource audioSource;

    public AudioMixerGroup soundEffectMixer;

    public static AudioManager instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than 1 instances of AudioManager in scene");
            return;
        }

        instance = this;
    }

    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }
    void Update()
    {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    public AudioSource PlaySoundAt(AudioClip clip, Vector3 pos) {
        GameObject tmp = new GameObject("TempAudio");
        tmp.transform.position = pos;
        AudioSource audioSource = tmp.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = soundEffectMixer;
        audioSource.Play();
        Destroy(tmp, clip.length);
        return audioSource;
    }
}
