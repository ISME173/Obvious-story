using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GlobalZenjectInstaller : MonoInstaller
{
    [Header("Mobile input")]
    [SerializeField] private Joystick _playerJoystick;
    [SerializeField] private Button _playerAttackButton;

    [Header("Desktop input")]
    [SerializeField] private DesktopInput _desktopInput;

    [Header("Other")]
    [SerializeField] private PlayerMoving _playerMoving;

    public override void InstallBindings()
    {
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Handheld:
                Container.BindInterfacesAndSelfTo<MobileInput>().FromInstance(new MobileInput(_playerJoystick, _playerAttackButton));
                break;
            case DeviceType.Desktop:
                Container.BindInterfacesAndSelfTo<DesktopInput>().FromInstance(_desktopInput);
                StateOffInDecktopInput();
                break;
        }

        Container.Bind<PlayerMoving>().FromInstance(_playerMoving);
    }
    private void StateOffInDecktopInput()
    {
        _playerJoystick.gameObject.SetActive(false);
        _playerAttackButton.gameObject.SetActive(false);
    }
}
