using UnityEngine;

[System.Serializable]
public struct PlayerSound
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private PlayerSoundTypes _playerSoundType;

    public AudioClip AudioClip => _audioClip;
    public PlayerSoundTypes PlayerSoundType => _playerSoundType;
         
    public enum PlayerSoundTypes
    {
        SwordSwim, TakeDamage, Walk
    }
}
