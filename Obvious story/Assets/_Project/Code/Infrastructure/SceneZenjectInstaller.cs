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
                StatesInMobileInput();
                break;
            case DeviceType.Desktop:
                Container.BindInterfacesAndSelfTo<DesktopInput>().FromInstance(_desktopInput);
                StatesInDecktopInput();
                break;
        }

        Container.Bind<PlayerMoving>().FromInstance(_playerMoving).NonLazy();
        Container.Bind<PlayerHealthManager>().FromInstance(_playerHealthManager);
    }
    private void StatesInDecktopInput()
    {
        _playerJoystick.gameObject.SetActive(false);
        _playerAttackButton.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(false);
    }
    private void StatesInMobileInput()
    {
        _playerJoystick.gameObject.SetActive(true);
        _playerAttackButton.gameObject.SetActive(true);
        _pauseButton.gameObject.SetActive(true);
    }
}
