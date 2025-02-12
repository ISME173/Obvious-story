using System;
using UnityEngine;
using Zenject;

public class ProjectZenjectInstaller : MonoInstaller
{
    [Header("Game settings")]
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private SoundSettings _soundSettings;

    public override void InstallBindings()
    {
        if (_gameSettings == null)
            throw new NullReferenceException(nameof(_gameSettings));

        if (_soundSettings == null)
            throw new NullReferenceException(nameof(_soundSettings));

        Container.Bind<GameSettings>().FromInstance(_gameSettings);
        Container.Bind<SoundSettings>().FromInstance(_soundSettings);
    }
}
