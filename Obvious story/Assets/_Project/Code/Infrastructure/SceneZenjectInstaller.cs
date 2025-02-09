using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SceneZenjectInstaller : MonoInstaller
{
    [Header("Mobile input")]
    [SerializeField] private Joystick _playerJoystick;
    [SerializeField] private Button _playerAttackButton;
    [SerializeField] private Button _pauseButton;

    [Header("Desktop input")]
    [SerializeField] private DesktopInput _desktopInput;

    [Header("Other")]
    [SerializeField] private PlayerMoving _playerMoving;
    [SerializeField] private PlayerHealthManager _playerHealthManager;
    [SerializeField] private UIManager _uiManager;

    public override void InstallBindings()
    {
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Handheld:
                Container.BindInterfacesAndSelfTo<MobileInput>().FromInstance(new MobileInput(_playerJoystick, _playerAttackButton, _pauseButton));
                StatesInMobileInput();
                break;
            case DeviceType.Desktop:
                Container.BindInterfacesAndSelfTo<DesktopInput>().FromInstance(_desktopInput).NonLazy();
                StatesInDecktopInput();
                break;
        }

        if (_uiManager == null)
            _uiManager = FindAnyObjectByType<UIManager>();

        Container.Bind<PlayerMoving>().FromInstance(_playerMoving);
        Container.Bind<PlayerHealthManager>().FromInstance(_playerHealthManager).NonLazy();
        Container.Bind<UIManager>().FromInstance(_uiManager);
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
