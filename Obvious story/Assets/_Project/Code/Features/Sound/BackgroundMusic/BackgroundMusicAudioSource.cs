using UnityEngine;
using static BackgroundMusic;

[System.Serializable]
public struct BackgroundMusicAudioSource
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private BackgroundMusicTypes _backgroundMusicType;

    public AudioSource AudioSource => _audioSource;
    public BackgroundMusicTypes BackgroundMusicType => _backgroundMusicType;
}
