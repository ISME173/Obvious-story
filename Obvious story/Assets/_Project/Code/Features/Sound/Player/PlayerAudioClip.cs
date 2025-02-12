using UnityEngine;

[System.Serializable]
public struct PlayerAudioClip
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private PlayerAudioClipTypes _playerAudioClipType;

    public AudioClip AudioClip => _audioClip;
    public PlayerAudioClipTypes PlayerAudioClipType => _playerAudioClipType;

    public enum PlayerAudioClipTypes
    {
        SwordSwim, TakeDamage, Walk
    }
}
