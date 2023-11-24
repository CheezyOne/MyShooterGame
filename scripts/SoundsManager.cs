using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup Mixer; //Put Master here
    [Header("Music button")]
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Sprite MutedMusic, UnmutedMusic;
    [Header("Sounds button")]
    [SerializeField] private Slider SoundsSlider;
    [SerializeField] private Sprite MutedSounds, UnmutedSounds;
    [Header("Music slider")]
    [SerializeField] private Button musicButton;
    [Header("Sounds slider")]
    [SerializeField] private Button soundsButton;



    public void ChangeSounds(float volume)//Slider
    {
        if (volume != 0)
        {
            soundsButton.GetComponent<Image>().sprite = UnmutedSounds;
        }
        else
        {
            soundsButton.GetComponent<Image>().sprite = MutedSounds;
        }
        Mixer.audioMixer.SetFloat("Sounds", Mathf.Lerp(-80, 0, volume));
    }
    public void ChangeMusic(float volume)//Slider
    { 
        if(volume!=0)
        {
            musicButton.GetComponent<Image>().sprite = UnmutedMusic;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = MutedMusic;
        }
        Mixer.audioMixer.SetFloat("Music", Mathf.Lerp(-80, 0, volume));
    }
    public void MusicButton()
    {
        
        Mixer.audioMixer.GetFloat("Music", out float volume);
        if(volume==-80)
        {
            GetComponent<Image>().sprite = UnmutedMusic;
            MusicSlider.value = 1;
            Mixer.audioMixer.SetFloat("Music", 0);
        }
        else
        {
            GetComponent<Image>().sprite = MutedMusic;
            MusicSlider.value= 0;
            Mixer.audioMixer.SetFloat("Music", -80);
        }
    }
    public void SoundsButton()
    {
        Mixer.audioMixer.GetFloat("Sounds", out float volume);
        if (volume == -80)
        {
            GetComponent<Image>().sprite = UnmutedSounds;
            SoundsSlider.value = 1;
            Mixer.audioMixer.SetFloat("Sounds", 0);
        }
        else
        {
            GetComponent<Image>().sprite = MutedSounds;
            SoundsSlider.value = 0;
            Mixer.audioMixer.SetFloat("Sounds", -80);
        }
    }
}
