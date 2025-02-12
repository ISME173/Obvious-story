using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using static PlayerAudioClip;
using static SoundUI;
using static BackgroundMusic;

public class SoundController : MonoBehaviour
{
    [Inject] private SoundSettings _soundSettings;

    private List<AudioSource> _soundAudioSources = new();
    private List<AudioSource> _musicAudioSources = new();

    private Dictionary<PlayerAudioClipTypes, AudioSource> _playerAudioSources = new();
    private Dictionary<UISoundTypes, AudioSource> _uIAudioSources = new();
    private Dictionary<BackgroundMusicTypes, AudioSource> _backgroundMusicSources = new();

    private void Awake() =>  AddListeners();
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

    private void PlayAnySoundInAnyAudioSource(AudioSource audioSource, AudioClip audioClip, bool playOneShot, bool ToPlayEvenIfThePreviousOneIsStillPlaying, float pitch = 1)
    {
        audioSource.pitch = pitch;

        if (ToPlayEvenIfThePreviousOneIsStillPlaying)
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

    public void PlayPlayerSound(PlayerAudioClipTypes PlayerAudioClipType, bool playOneShot = true, bool ToPlayEvenIfThePreviousOneIsStillPlaying = true, float pitch = 1)
    {
        PlayerAudioClip PlayerAudioClip = _soundSettings.PlayerSounds.FirstOrDefault(x => x.PlayerAudioClipType == PlayerAudioClipType);
        if (PlayerAudioClipType == PlayerAudioClip.PlayerAudioClipType && _playerAudioSources.TryGetValue(PlayerAudioClipType, out AudioSource audioSource))
        {
            PlayAnySoundInAnyAudioSource(audioSource, PlayerAudioClip.AudioClip, playOneShot, ToPlayEvenIfThePreviousOneIsStillPlaying, pitch);
        }
    }

    public void PlayUISound(UISoundTypes soundType, bool playOneShot = true, bool ToPlayEvenIfThePreviousOneIsStillPlaying = true, float pitch = 1)
    {
        SoundUI uISound = _soundSettings.SoundsUI.FirstOrDefault(x => x.UISoundType == soundType);
        if (soundType == uISound.UISoundType && _uIAudioSources.TryGetValue(soundType, out AudioSource audioSource))
        {
            PlayAnySoundInAnyAudioSource(audioSource, uISound.AudioClip, playOneShot, ToPlayEvenIfThePreviousOneIsStillPlaying, pitch);
        }
    }

    public void PlayBackgroundMusic(BackgroundMusicTypes backgroundMusicTypeToPlay, bool playOneShot = true, bool ToPlayEvenIfThePreviousOneIsStillPlaying = true, float pitch = 1)
    {
        BackgroundMusic backgroundMusic = _soundSettings.BackgroundMusics.FirstOrDefault(x => x.BackgroundMusicType == backgroundMusicTypeToPlay);
        if (backgroundMusicTypeToPlay == backgroundMusic.BackgroundMusicType && _backgroundMusicSources.TryGetValue(backgroundMusicTypeToPlay, out AudioSource audioSource))
        {
            PlayAnySoundInAnyAudioSource(audioSource, backgroundMusic.AudioClip, playOneShot, ToPlayEvenIfThePreviousOneIsStillPlaying, pitch);
        }
    }

    public void AddPlayerSoundAudioSourceToDictionary(AudioSource audioSourceToAdd, PlayerAudioClipTypes PlayerAudioClipType)
    {
        if (_playerAudioSources.TryGetValue(PlayerAudioClipType, out AudioSource audioSource))
            return;

        _playerAudioSources.Add(PlayerAudioClipType, audioSourceToAdd);

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