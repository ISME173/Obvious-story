using UnityEngine;

public class BackgoundMusicList : MonoBehaviour
{
    [SerializeField] private BackgroundMusicAudioSource[] _backgroundAudioSource;

    private void Awake()
    {
        for (int i = 0; i < _backgroundAudioSource.Length; i++)
            SoundController.Instance.AddMusicAudioSourceToDictionary(_backgroundAudioSource[i].AudioSource, _backgroundAudioSource[i].BackgroundMusicType);
    }
}
