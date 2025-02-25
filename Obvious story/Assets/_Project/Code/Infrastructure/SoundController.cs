using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static PlayerAudioClip;
using static SoundUI;
using static BackgroundMusic;

public class SoundController : MonoBehaviour
{
    [SerializeField] private SoundSettings _soundSettings;

    private static SoundController _instance;

    private List<AudioSource> _soundAudioSources = new();
    private List<AudioSource> _musicAudioSources = new();

    private Dictionary<PlayerAudioClipTypes, AudioSource> _playerAudioSources = new();
    private Dictionary<UISoundTypes, AudioSource> _uIAudioSources = new();
    private Dictionary<BackgroundMusicTypes, AudioSource> _backgroundMusicSources = new();

    public static SoundController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<SoundController>();

                if (_instance == null)
                {
                    GameObject instance = new GameObject();
                    _instance = instance.AddComponent<SoundController>();
                    _instance.gameObject.name = typeof(SoundController).ToString();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        AddListeners();
    }
    private void Start()
    {
        _soundSettings.SetSoundSlider(UIPanelsEvents.Instance.UIPanels.SoundSlider);
        _soundSettings.SetMusicSlider(UIPanelsEvents.Instance.UIPanels.MusicSlider);
    }
    private void AddListeners()
    {
        _soundSettings.OnSoundVolumeChanged.AddListener((float value) =>
        {
            if (_soundAudioSources == null || _soundAudioSources.Count == 0)
                return;

            for (int i = 0; i < _soundAudioSources.Count; i++)
                _soundAudioSources[i].volume = value;
        });

        _soundSettings.OnMusicVolumeChanged.AddListener((float value) =>
        {
            if (_musicAudioSources == null || _musicAudioSources.Count == 0)
                return;

            for (int i = 0; i < _musicAudioSources.Count; i++)
                _musicAudioSources[i].volume = value;
        });
    }

    private void PlayAnySoundInAnyAudioSource(AudioSource audioSource, AudioClip audioClip, bool playOneShot, bool toPlayEvenIfThePreviousOneIsStillPlaying, float pitch = 1)
    {
        audioSource.pitch = pitch;

        if (toPlayEvenIfThePreviousOneIsStillPlaying)
        {
            if (playOneShot)
            {
                audioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying == false)
            {
                if (playOneShot)
                {
                    audioSource.PlayOneShot(audioClip);
                }
                else
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
            }
        }
    }

    public void PlayPlayerSound(PlayerAudioClipTypes playerAudioClipType, bool playOneShot = true, bool toPlayEvenIfThePreviousOneIsStillPlaying = true, float pitch = 1)
    {
        PlayerAudioClip PlayerAudioClip = _soundSettings.PlayerSounds.FirstOrDefault(x => x.PlayerAudioClipType == playerAudioClipType);
        if (playerAudioClipType == PlayerAudioClip.PlayerAudioClipType && _playerAudioSources.TryGetValue(playerAudioClipType, out AudioSource audioSource))
        {
            PlayAnySoundInAnyAudioSource(audioSource, PlayerAudioClip.AudioClip, playOneShot, toPlayEvenIfThePreviousOneIsStillPlaying, pitch);
        }
    }

    public void PlayUISound(UISoundTypes soundType, bool playOneShot = true, bool toPlayEvenIfThePreviousOneIsStillPlaying = true, float pitch = 1)
    {
        SoundUI uISound = _soundSettings.SoundsUI.FirstOrDefault(x => x.UISoundType == soundType);
        if (soundType == uISound.UISoundType && _uIAudioSources.TryGetValue(soundType, out AudioSource audioSource))
        {
            PlayAnySoundInAnyAudioSource(audioSource, uISound.AudioClip, playOneShot, toPlayEvenIfThePreviousOneIsStillPlaying, pitch);
        }
    }

    public void PlayBackgroundMusic(BackgroundMusicTypes backgroundMusicTypeToPlay, bool playOneShot = true, bool toPlayEvenIfThePreviousOneIsStillPlaying = true, float pitch = 1)
    {
        BackgroundMusic backgroundMusic = _soundSettings.BackgroundMusics.FirstOrDefault(x => x.BackgroundMusicType == backgroundMusicTypeToPlay);
        if (backgroundMusicTypeToPlay == backgroundMusic.BackgroundMusicType && _backgroundMusicSources.TryGetValue(backgroundMusicTypeToPlay, out AudioSource audioSource))
        {
            PlayAnySoundInAnyAudioSource(audioSource, backgroundMusic.AudioClip, playOneShot, toPlayEvenIfThePreviousOneIsStillPlaying, pitch);
        }
    }

    public void AddPlayerSoundAudioSourceToDictionary(AudioSource audioSourceToAdd, PlayerAudioClipTypes playerAudioClipType)
    {
        if (_playerAudioSources.TryGetValue(playerAudioClipType, out AudioSource audioSource))
            return;

        _playerAudioSources.Add(playerAudioClipType, audioSourceToAdd);

        audioSourceToAdd.volume = _soundSettings.SoundVolume;
        _soundAudioSources.Add(audioSourceToAdd);
        //Debug.Log($"Add {audioSourceToAdd} to dictionary");
    }

    public void AddUIAudioSourceInDictionary(AudioSource audioSourceToAdd, UISoundTypes uISoundType)
    {
        if (_uIAudioSources.TryGetValue(uISoundType, out AudioSource audioSource))
            return;

        _uIAudioSources.Add(uISoundType, audioSourceToAdd);

        audioSourceToAdd.volume = _soundSettings.SoundVolume;
        _soundAudioSources.Add(audioSourceToAdd);

        //Debug.Log($"Add {audioSourceToAdd} to dictionary");
    }

    public void AddMusicAudioSourceToDictionary(AudioSource audioSourceToAdd, BackgroundMusicTypes backgroundMusicType)
    {
        if (_backgroundMusicSources.TryGetValue(backgroundMusicType, out AudioSource source))
            return;

        _backgroundMusicSources.Add(backgroundMusicType, audioSourceToAdd);

        audioSourceToAdd.volume = _soundSettings.MusicVolume;
        _musicAudioSources.Add(audioSourceToAdd);

        // Debug.Log($"Add {audioSourceToAdd} to dictionary");
    }

    public bool CheckAudioSourceInPlayerAudioSources(AudioSource audioSource)
    {
        return _playerAudioSources.Any(x => x.Value == audioSource);
    }
    public bool CheckAudioSourceInBackgroundMusicAudioSources(AudioSource audioSource)
    {
        return _uIAudioSources.Any(x => x.Value == audioSource);
    }
    public bool CheckAudioSourceInUIAudioSources(AudioSource audioSource)
    {
        return _backgroundMusicSources.Any(x => x.Value == audioSource);
    }
}