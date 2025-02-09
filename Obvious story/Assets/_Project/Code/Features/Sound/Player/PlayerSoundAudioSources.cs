using System;
using UnityEngine;

public class PlayerSoundAudioSources : MonoBehaviour
{
    [SerializeField] private PlayerAudioSource[] _playerAudioSources;

    private void Awake()
    {
        if (_playerAudioSources.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_playerAudioSources));

        for (int i = 0; i < _playerAudioSources.Length; i++)
            SoundController.Instance.AddPlayerSoundAudioSourceToDictionary(_playerAudioSources[i].AudioSource, _playerAudioSources[i].PlayerSoundType);
    }
    public void PlayerWalkSoundPlay()
    {
        if (GameManager.Instance.IsGameStarting == false)
            return;

        SoundController.Instance.PlayPlayerSound(PlayerSound.PlayerSoundTypes.Walk, true, false, UnityEngine.Random.Range(0.7f, 1.2f));
    }
}
