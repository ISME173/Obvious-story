using UnityEngine;
using System;
using Zenject;

public class UISoundController : MonoBehaviour
{
    [SerializeField] private UIAudioSource[] _uIAudioSources;

    private void Awake()
    {
        if (_uIAudioSources.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_uIAudioSources));

        for (int i = 0; i < _uIAudioSources.Length; i++)
            SoundController.Instance.AddUIAudioSourceInDictionary(_uIAudioSources[i].AudioSource, _uIAudioSources[i].SoundTypes);

        AddListeners();
    }
    private void AddListeners()
    {
        UIPanelsEvents.Instance.ButtonContinueClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonMenuClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonNextLevelClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonPlayClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonPauseClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonRestartClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonSettingsClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonExitGameClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonNoLoadNextLevelClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonExitSettingsClick.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
    }
}
