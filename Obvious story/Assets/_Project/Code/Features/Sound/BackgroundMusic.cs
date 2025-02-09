using UnityEngine;

[System.Serializable]
public struct BackgroundMusic
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private BackgroundMusicTypes _backgroundMusicTypes;

    public AudioClip AudioClip => _audioClip;
    public BackgroundMusicTypes BackgroundMusicType => _backgroundMusicTypes;

    public enum BackgroundMusicTypes
    {
        Menu, Win, GameOver, Level1, Level2, Level3
    }
}
