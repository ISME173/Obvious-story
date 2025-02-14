using System;
using UnityEngine;

public class PlayerSoundActivator : MonoBehaviour
{
    [SerializeField] private PlayerAudioSource[] _playerSoundAudioSources;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerHealthManager _playerHealthManager;

    private void Awake()
    {
        if (_playerSoundAudioSources.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_playerSoundAudioSources));

        for (int i = 0; i < _playerSoundAudioSources.Length; i++)
            SoundController.Instance.AddPlayerSoundAudioSourceToDictionary(_playerSoundAudioSources[i].AudioSource, _playerSoundAudioSources[i].PlayerAudioClipType);
        
        AddListenersToEvents();
    }

    private void AddListenersToEvents()
    {
        _playerAttack.PlayerAttackActivate += (_) => SoundController.Instance.PlayPlayerSound(PlayerAudioClip.PlayerAudioClipTypes.SwordSwim, true, true, UnityEngine.Random.Range(0.7f, 1.2f));
        _playerHealthManager.DamageTaken += (_) => SoundController.Instance.PlayPlayerSound(PlayerAudioClip.PlayerAudioClipTypes.TakeDamage, true, true, UnityEngine.Random.Range(0.8f, 1.2f));
    }
    public void PlayerWalkSoundPlay()
    {
        if (GameEvents.Instance.IsGameStarting == false)
            return;

        SoundController.Instance.PlayPlayerSound(PlayerAudioClip.PlayerAudioClipTypes.Walk, true, true, UnityEngine.Random.Range(0.7f, 1.2f));
    }
}
