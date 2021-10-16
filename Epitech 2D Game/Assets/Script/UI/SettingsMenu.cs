using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetMusicVolume(float volume) {
        if (volume == -40)
            volume = -80;
        mixer.SetFloat("Music",volume);
    }

    public void SetSoundVolume(float volume) {
        if (volume == -40)
            volume = -80;
        mixer.SetFloat("Sound",volume);
    }
}
