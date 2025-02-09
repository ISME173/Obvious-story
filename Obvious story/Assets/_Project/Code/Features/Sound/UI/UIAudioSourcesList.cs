using UnityEngine;

public class UIAudioSourcesList : MonoBehaviour
{
    [SerializeField] private UIAudioSource[] _uIAudioSources;

    private void Awake()
    {
        for (int i = 0; i < _uIAudioSources.Length; i++)
            SoundController.Instance.AddUIAudioSourceInDictionary(_uIAudioSources[i].AudioSource, _uIAudioSources[i].SoundTypes);
    }
}
