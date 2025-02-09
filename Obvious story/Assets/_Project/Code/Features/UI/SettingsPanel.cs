using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : BasePanel
{
    [Header("Settings panel states"), Space]
    [SerializeField] private Button _exitSettings;

    [Header("Sound and music"), Space]
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    public Slider SoundSlider => _soundSlider;
    public Slider MusicSlider => _musicSlider;
    public Button ExitSettings => _exitSettings;
}
