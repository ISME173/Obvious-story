using UnityEngine;
using static SoundUI;

[System.Serializable]
public struct UIAudioSource
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private UISoundTypes _soundType;

    public AudioSource AudioSource => _audioSource;
    public UISoundTypes SoundTypes => _soundType;
}
