using UnityEngine;
using System;

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
        UIPanelsEvents.Instance.ButtonContinueClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonMenuClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonMenuClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonPlayClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonMenuClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonRestartClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonSettingsClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonExitGameClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonNoLoadNextLevelClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
        UIPanelsEvents.Instance.ButtonExitSettingsClickReadOnly.AddListener(() => SoundController.Instance.PlayUISound(SoundUI.UISoundTypes.ButtonClick));
    }
}
