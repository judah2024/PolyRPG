using System;
using System.Collections;
using System.Collections.Generic;
using EnumDef;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class ClipData<T>
{
    public T type;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Source")]
    public AudioSource kMusicAudio;
    public AudioSource kSFXAudio;
    
    [Header("Clip List")]
    [SerializeField] List<ClipData<BGMType>> listBGM;
    [SerializeField] List<ClipData<SFXType>> listSFX;

    Dictionary<BGMType, AudioClip> _dictBGM;
    Dictionary<SFXType, AudioClip> _dictSFX;
    
    void Awake()
    {
        InitializeSounds();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);

    }

    void InitializeSounds()
    {
        _dictBGM = new Dictionary<BGMType, AudioClip>();
        foreach (var data in listBGM)
        {
            _dictBGM[data.type] = data.clip;
        }

        _dictSFX = new Dictionary<SFXType, AudioClip>();
        foreach (var data in listSFX)
        {
            _dictSFX[data.type] = data.clip;
        }

        PlayBGM(BGMType.Title);
    }

    public void PlayBGM(BGMType bgmType)
    {
        kMusicAudio.clip = _dictBGM[bgmType];
        kMusicAudio.Play();
    }

    public void PlaySFX(SFXType sfxType)
    {
        kSFXAudio.PlayOneShot(_dictSFX[sfxType]);
    }
}
