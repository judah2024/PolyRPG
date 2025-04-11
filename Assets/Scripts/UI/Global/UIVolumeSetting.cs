using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSetting : MonoBehaviour
{
    public AudioMixer kAudioMixer;
    
    [Header("Sound Slider")]
    public Slider kMasterSlider;
    public Slider kMusicSlider;
    public Slider kEffectSlider;
    
    void Awake()
    {
        kMasterSlider.onValueChanged.AddListener(volume => SetVolume(StrDef.NAME_MASTER_VOLUME, volume));
        kMusicSlider.onValueChanged.AddListener(volume => SetVolume(StrDef.NAME_MUSIC_VOLUME, volume));
        kEffectSlider.onValueChanged.AddListener(volume => SetVolume(StrDef.NAME_EFFECT_VOLUME, volume));
        
        kMasterSlider.value = 100;
        kMusicSlider.value = 100;
        kEffectSlider.value = 100;
    }
    
    // // InGame Pause 창에 필요함
    // void OnEnable()
    // {
    //     Time.timeScale = 0f;
    // }
    //
    // void OnDisable()
    // {
    //     Time.timeScale = 1f;
    // }

    public void SetVolume(string sourceName, float volume)
    {
        float normalVolume = Mathf.Clamp(volume * 0.01f, 0.0001f, 1f);
        float dB = Mathf.Log10(normalVolume) * 20f;
        kAudioMixer.SetFloat(sourceName, dB);
        Debug.Log($"{sourceName} : {volume}");
    }
}
