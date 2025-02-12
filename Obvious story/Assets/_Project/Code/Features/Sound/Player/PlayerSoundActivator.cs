using System;
using UnityEngine;
using Zenject;

public class PlayerSoundActivator : MonoBehaviour
{
    [SerializeField] private PlayerAudioSource[] _playerSoundAudioSources;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerHealthManager _playerHealthManager;

    [Inject] SoundController _soundController;

    private void Awake()
    {
        if (_playerSoundAudioSources.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_playerSoundAudioSources));

        for (int i = 0; i < _playerSoundAudioSources.Length; i++)
            _soundController.AddPlayerSoundAudioSourceToDictionary(_playerSoundAudioSources[i].AudioSource, _playerSoundAudioSources[i].PlayerAudioClipType);
    }
    private void Start()
    {
        AddListenersToEvents();
    }
    private void AddListenersToEvents()
    {
        _playerAttack.PlayerAttackActivate += (_) => _soundController.PlayPlayerSound(PlayerAudioClip.PlayerAudioClipTypes.SwordSwim, true, true, UnityEngine.Random.Range(0.7f, 1.2f));
        _playerHealthManager.DamageTaken += (_) => _soundController.PlayPlayerSound(PlayerAudioClip.PlayerAudioClipTypes.TakeDamage, true, true, UnityEngine.Random.Range(0.8f, 1.2f));
    }
    public void PlayerWalkSoundPlay()
    {
        if (GameEvents.Instance.IsGameStarting == false)
            return;

        _soundController.PlayPlayerSound(PlayerAudioClip.PlayerAudioClipTypes.Walk, true, true, UnityEngine.Random.Range(0.7f, 1.2f));
    }
}
