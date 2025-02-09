using UnityEngine;
using static PlayerSound;

[System.Serializable]
public struct PlayerAudioSource
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayerSoundTypes _playerSoundType;

    public AudioSource AudioSource => _audioSource;
    public PlayerSoundTypes PlayerSoundType => _playerSoundType;
}
