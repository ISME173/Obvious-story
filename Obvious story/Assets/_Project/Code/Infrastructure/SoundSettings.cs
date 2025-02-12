using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "ScriptableObject/SoundSettings")]
public class SoundSettings : ScriptableObject
{
    [Header("Sound saving states")]
    [SerializeField] private string _soundSavingParameterName;
    [SerializeField] private string _musicSavingParameterName;
    [Space]
    [SerializeField] private bool _saveVolumeImmediatelyAfterChanges = true;

    [Header("Sounds"), Space]
    [SerializeField] private List<PlayerAudioClip> _playerSounds = new();
    [SerializeField] private List<SoundUI> _soundsUI = new();
    [SerializeField] private List<BackgroundMusic> _backgroundMusic = new();

    private Slider _musicSlider;
    private Slider _soundSlider;

    public UnityEvent<float> OnMusicVolumeChanged { get; private set; } = new();
    public UnityEvent<float> OnSoundVolumeChanged { get; private set; } = new();

    public float MusicVolume => _musicSlider != null ? _musicSlider.value : 0f;
    public float SoundVolume => _soundSlider != null ? _soundSlider.value : 0f;

    public List<SoundUI> SoundsUI => _soundsUI;
    public List<PlayerAudioClip> PlayerSounds => _playerSounds;
    public List<BackgroundMusic> BackgroundMusics => _backgroundMusic;

    private void OnSoundVolumeChangedInvoke(float value)
    {
        OnSoundVolumeChanged?.Invoke(value);

        if (_saveVolumeImmediatelyAfterChanges)
            PlayerPrefs.SetFloat(_soundSavingParameterName, SoundVolume);

        //Debug.Log($"Sound volume: {SoundVolume}");
    }
    private void OnMusicVolumeChangedInvoke(float value)
    {
        OnMusicVolumeChanged?.Invoke(value);

        if (_saveVolumeImmediatelyAfterChanges)
            PlayerPrefs.SetFloat(_musicSavingParameterName, MusicVolume);

        //Debug.Log($"Music volume {MusicVolume}");
    }

    public void SetMusicSlider(Slider slider)
    {
        if (_musicSlider != null)
            _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChangedInvoke);
        _musicSlider = slider;

        if (PlayerPrefs.HasKey(_musicSavingParameterName))
            _musicSlider.value = PlayerPrefs.GetFloat(_musicSavingParameterName);

        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChangedInvoke);
        OnMusicVolumeChangedInvoke(_musicSlider.value);
    }
    public void SetSoundSlider(Slider slider)
    {
        if (_soundSlider != null)
            _soundSlider.onValueChanged.RemoveListener(OnSoundVolumeChangedInvoke);
        _soundSlider = slider;

        if (PlayerPrefs.HasKey(_soundSavingParameterName))
            _soundSlider.value = PlayerPrefs.GetFloat(_soundSavingParameterName);

        _soundSlider.onValueChanged.AddListener(OnSoundVolumeChangedInvoke);
        OnSoundVolumeChangedInvoke(_soundSlider.value);
    }
    public void SaveAllSettings()
    {
        PlayerPrefs.SetFloat(_musicSavingParameterName, MusicVolume);
        PlayerPrefs.SetFloat(_soundSavingParameterName, SoundVolume);
    }
}
