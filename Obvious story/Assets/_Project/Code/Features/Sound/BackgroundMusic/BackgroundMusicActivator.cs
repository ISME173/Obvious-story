using UnityEngine;
using System;
using Zenject;

public class BackgroundMusicActivator : MonoBehaviour
{
    [SerializeField] private BackgroundMusicAudioSource[] _backgroundAudioSource;
    [SerializeField] private BackgroundMusic.BackgroundMusicTypes _backgroundMusicTypeInLevel;

    [Inject] SoundController _soundController;

    private void Awake()
    {
        if (_backgroundAudioSource.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_backgroundAudioSource));

        for (int i = 0; i < _backgroundAudioSource.Length; i++)
            _soundController.AddMusicAudioSourceToDictionary(_backgroundAudioSource[i].AudioSource, _backgroundAudioSource[i].BackgroundMusicType);

        AddListeners();
    }
    private void AddListeners()
    {
        GameEvents.Instance.OnGameOpen.AddListener(() => _soundController.PlayBackgroundMusic(_backgroundMusicTypeInLevel));
        SceneLoaoder.Instance.OnNextLevelLoaoded.AddListener(() => _soundController.PlayBackgroundMusic(_backgroundMusicTypeInLevel));
        SceneLoaoder.Instance.OnRestartLevelLoaded.AddListener(() => _soundController.PlayBackgroundMusic(_backgroundMusicTypeInLevel));
        SceneLoaoder.Instance.OnMenuSceneLoaded.AddListener(() => _soundController.PlayBackgroundMusic(_backgroundMusicTypeInLevel));
    }
}