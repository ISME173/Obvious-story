using System;
using UnityEngine;
using Zenject;

public class ProjectZenjectInstaller : MonoInstaller
{
    [Header("Game settings")]
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private SoundSettings _soundSettings;

    [Header("Servises")]
    [SerializeField] private SoundController _soundController;

    public override void InstallBindings()
    {
        if (_gameSettings == null)
            throw new NullReferenceException(nameof(_gameSettings));

        if (_soundSettings == null)
            throw new NullReferenceException(nameof(_soundSettings));

        if (_soundController == null)
            throw new NullReferenceException(nameof(_soundController));

        Container.Bind<GameSettings>().FromInstance(_gameSettings);
        Container.Bind<SoundSettings>().FromInstance(_soundSettings);
        Container.Bind<SoundController>().FromInstance(_soundController);
    }
}
