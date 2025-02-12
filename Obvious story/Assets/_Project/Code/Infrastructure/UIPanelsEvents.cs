using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class UIPanelsEvents : MonoBehaviour
{
    [SerializeField] private UIPanels _uIPanels;

    private static UIPanelsEvents _instance;
    [Inject] private IUserInput _userInput;

    [HideInInspector]
    public UnityEvent ButtonPlayClick, ButtonRestartClick, ButtonSettingsClick, ButtonExitSettingsClick, ButtonNextLevelClick, ButtonContinueClick,
        ButtonMenuClick, ButtonPauseClick, ButtonExitGameClick, ButtonNoLoadNextLevelClick;

    public static UIPanelsEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIPanelsEvents>();

                if (_instance == null)
                {
                    GameObject instance = new GameObject();
                    _instance = instance.AddComponent<UIPanelsEvents>();
                    _instance.gameObject.name = typeof(UIPanelsEvents).ToString();
                }
            }
            return _instance;
        }
    }
    public UIPanels UIPanels => _uIPanels;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        AddListenersToPanels();
        AddListenersToGameEvents();
    }
    private void AddListenersToPanels()
    {
        _userInput.OnPauseActivate += () =>
        {
            if (GameEvents.Instance.IsGameStarting == false)
                return;

            _uIPanels.PauseActivate();
            ButtonPauseClick?.Invoke();
        };

        _uIPanels.StartPanel.StartGame.onClick.AddListener(() =>
        {
            ButtonPlayClick?.Invoke();
            _uIPanels.StartPanel.Disable();
        });
        _uIPanels.StartPanel.Settings.onClick.AddListener(() =>
        {
            _uIPanels.SettingsPanel.Enable();
            ButtonSettingsClick?.Invoke();
        });
        _uIPanels.StartPanel.ExitGame.onClick.AddListener(() => { ButtonExitGameClick?.Invoke(); });

        _uIPanels.RestartPanel.RestartButton.onClick.AddListener(() =>
        {
            _uIPanels.RestartPanel.Disable();
            ButtonRestartClick?.Invoke();
        });
        _uIPanels.RestartPanel.SettingsButton.onClick.AddListener(() =>
        {
            _uIPanels.SettingsPanel.Enable();
            ButtonSettingsClick?.Invoke();
        });
        _uIPanels.RestartPanel.MenuButton.onClick.AddListener(() => { ButtonMenuClick?.Invoke(); });

        _uIPanels.SettingsPanel.ExitSettings.onClick.AddListener(() =>
        {
            _uIPanels.SettingsPanel.Disable(_uIPanels.SettingsPanel.DisableAnimTime, () => { _uIPanels.SettingsPanel.gameObject.SetActive(false); });
            ButtonExitSettingsClick?.Invoke();
        });

        _uIPanels.VictoryPanel.MenuButton.onClick.AddListener(() => { ButtonMenuClick?.Invoke(); });

        _uIPanels.PauseButton.onClick.AddListener(() =>
        {
            if (GameEvents.Instance.IsGameStarting == false)
                return;

            _uIPanels.PauseActivate();
            ButtonPauseClick?.Invoke();
        });
        _uIPanels.PausePanel.MenuButton.onClick.AddListener(() => { ButtonMenuClick?.Invoke(); });
        _uIPanels.PausePanel.SettingsButton.onClick.AddListener(() =>
        {
            _uIPanels.SettingsPanel.Enable();
            ButtonSettingsClick?.Invoke();
        });
        _uIPanels.PausePanel.ContinueButton.onClick.AddListener(() =>
        {
            if (_uIPanels.PausePanel.IsEnable == false)
                return;

            ButtonContinueClick?.Invoke();
            _uIPanels.PausePanel.Disable();
        });

        _uIPanels.FinishPanel.NextLevelButton.onClick.AddListener(() => { ButtonNextLevelClick?.Invoke(); });
        _uIPanels.FinishPanel.ReturnButton.onClick.AddListener(() =>
        {
            _uIPanels.FinishPanel.Disable();
            ButtonNoLoadNextLevelClick?.Invoke();
        });
    }
    private void AddListenersToGameEvents()
    {
        GameEvents.Instance.OnGameOpen.AddListener(() => { _uIPanels.StartPanel.Enable(); });
        GameEvents.Instance.OnGameOver.AddListener(() => { _uIPanels.RestartPanel.Enable(); });
        GameEvents.Instance.OnRestart.AddListener(() => { _uIPanels.LoadingPanel.Activate(); });
        GameEvents.Instance.OnVictory.AddListener(() =>
        {
            _uIPanels.VictoryPanel.Enable();
            _uIPanels.FinishPanel.Disable();
        });
        GameEvents.Instance.OnPlayerFinishEnter.AddListener(() => { _uIPanels.FinishPanel.Enable(); });
        GameEvents.Instance.OnPlayerFinishExit.AddListener(() =>
        {
            if (_uIPanels.FinishPanel.IsEnable)
                _uIPanels.FinishPanel.Disable();
        });

        SceneLoaoder.Instance.OnSceneLoad.AddListener((scene) => { _uIPanels.LoadingPanel.Activate(); });
        SceneLoaoder.Instance.OnMenuSceneLoaded.AddListener(() => { _uIPanels.StartPanel.Enable(); });
    }
}