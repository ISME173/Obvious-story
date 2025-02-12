using UnityEngine;
using static PlayerAudioClip;

[System.Serializable]
public struct PlayerAudioSource
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayerAudioClipTypes _playerSoundType;

    public AudioSource AudioSource => _audioSource;
    public PlayerAudioClipTypes PlayerAudioClipType => _playerSoundType;
}
