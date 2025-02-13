using UnityEngine;
using System;
using Zenject;

public class BackgroundMusicActivator : MonoBehaviour
{
    [SerializeField] private BackgroundMusicAudioSource[] _backgroundAudioSource;
    [SerializeField] private BackgroundMusic.BackgroundMusicTypes _backgroundMusicTypeInLevel;

    private void Awake()
    {
        if (_backgroundAudioSource.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_backgroundAudioSource));

        for (int i = 0; i < _backgroundAudioSource.Length; i++)
            SoundController.Instance.AddMusicAudioSourceToDictionary(_backgroundAudioSource[i].AudioSource, _backgroundAudioSource[i].BackgroundMusicType);

        AddListeners();
    }
    private void AddListeners()
    {
        GameEvents.Instance.OnGameOpenReadOnly.AddListener(() => SoundController.Instance.PlayBackgroundMusic(_backgroundMusicTypeInLevel));

        SceneLoaoder.Instance.OnNextLevelLoaodedReadOnly.AddListener(() => SoundController.Instance.PlayBackgroundMusic(_backgroundMusicTypeInLevel));
        SceneLoaoder.Instance.OnRestartLevelLoadedReadOnly.AddListener(() => SoundController.Instance.PlayBackgroundMusic(_backgroundMusicTypeInLevel));
        SceneLoaoder.Instance.OnMenuSceneLoadedReadOnly.AddListener(() => SoundController.Instance.PlayBackgroundMusic(_backgroundMusicTypeInLevel));
    }
}