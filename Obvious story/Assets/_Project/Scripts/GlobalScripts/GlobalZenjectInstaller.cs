using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GlobalZenjectInstaller : MonoInstaller
{
    [Header("Mobile input")]
    [SerializeField] private Joystick _playerJoystick;
    [SerializeField] private Button _playerAttackButton;
    [Space]
    [Header("Desktop input")]
    [SerializeField] private DesktopInput _desktopInput;

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
    }
    private void StateOffInDecktopInput()
    {
        _playerJoystick.gameObject.SetActive(false);
        _playerAttackButton.gameObject.SetActive(false);
    }
}
