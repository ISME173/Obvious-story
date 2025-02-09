using UnityEngine;

[System.Serializable]
public struct SoundUI
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private UISoundTypes _uISoundTypes;

    public AudioClip AudioClip => _audioClip;
    public UISoundTypes UISoundType => _uISoundTypes;

    public enum UISoundTypes
    {
        ButtonClick
    }
}
