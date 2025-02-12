using UnityEngine;
using System;
using Zenject;

public class UISoundController : MonoBehaviour
{
    [SerializeField] private UIAudioSource[] _uIAudioSources;

    [Inject] SoundController _soundController;

    private void Awake()
    {
        if (_uIAudioSources.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_uIAudioSources));

        for (int i = 0; i < _uIAudioSources.Length; i++)
            _soundController.AddUIAudioSourceInDictionary(_uIAudioSources[i].AudioSource, _uIAudioSources[i].SoundTypes);

        AddListeners();
    }
    private void AddListeners()
    {
        UIPanelsEvents.Instance.ButtonContinueClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonMenuClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonNextLevelClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonPlayClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonPauseClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonRestartClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonSettingsClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonExitGameClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonNoLoadNextLevelClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonExitSettingsClick.AddListener(() => _soundController.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
    }
}
