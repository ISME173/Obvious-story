using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SceneZenjectInstaller : MonoInstaller
{
    [Header("Mobile input"), Space]
    [SerializeField] private Joystick _playerJoystick;
    [SerializeField] private Button _playerAttackButton;
    [SerializeField] private Button _pauseButton;

    [Header("Desktop input"), Space]
    [SerializeField] private DesktopInput _desktopInput;

    [Header("Player components"), Space]
    [SerializeField] private PlayerMoving _playerMoving;
    [SerializeField] private PlayerHealthManager _playerHealthManager;

    public override void InstallBindings()
    {
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Handheld:
                Container.BindInterfacesAndSelfTo<MobileInput>().FromInstance(new MobileInput(_playerJoystick, _playerAttackButton, _pauseButton));
                break;
            case DeviceType.Desktop:
                Container.BindInterfacesAndSelfTo<DesktopInput>().FromInstance(_desktopInput);
                break;
        }

        Container.Bind<PlayerMoving>().FromInstance(_playerMoving);
        Container.Bind<PlayerHealthManager>().FromInstance(_playerHealthManager);
    }
}