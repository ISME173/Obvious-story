using System;
using UnityEngine;
using Zenject;

public class GameEvents : MonoBehaviour
{
    [Inject] private PlayerHealthManager _playerHealthManager;

    private static GameEvents _instance;

    private IInvokableEvent OnGameOpen = new SecureUnityEvent(), OnGameOver = new SecureUnityEvent(), OnVictory = new SecureUnityEvent(),
        OnPlay = new SecureUnityEvent(), OnRestart = new SecureUnityEvent(), OnExitGame = new SecureUnityEvent(), OnPlayerFinishEnter = new SecureUnityEvent(),
        OnPlayerFinishExit = new SecureUnityEvent(), OnNextLevel = new SecureUnityEvent(), OnMenuOpen = new SecureUnityEvent(),
        OnGamePause = new SecureUnityEvent();

    #region ReadOnlyEvents

    public IReadOnlyEvent OnGameOpenReadOnly => OnGameOpen;
    public IReadOnlyEvent OnGameOverReadOnly => OnGameOver;
    public IReadOnlyEvent OnVictoryReadOnly => OnVictory;
    public IReadOnlyEvent OnPlayReadOnly => OnPlay;
    public IReadOnlyEvent OnRestartReadOnly => OnRestart;
    public IReadOnlyEvent OnExitGameReadOnly => OnExitGame;
    public IReadOnlyEvent OnPlayerFinishEnterReadOnly => OnPlayerFinishEnter;
    public IReadOnlyEvent OnPlayerFinishExitReadOnly => OnPlayerFinishExit;
    public IReadOnlyEvent OnNextLevelReadOnly => OnNextLevel;
    public IReadOnlyEvent OnMenuOpenReadOnly => OnMenuOpen;
    public IReadOnlyEvent OnGamePauseReadOnly => OnGamePause;

    #endregion

    public static GameEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GameEvents>();

                if (_instance == null)
                {
                    GameObject instance = new GameObject();
                    _instance = instance.AddComponent<GameEvents>();
                    _instance.gameObject.name = typeof(GameEvents).ToString();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }

    public bool IsGameStarting { get; private set; } = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneLoaoder.Instance.OnSceneLoaodedReadOnly.AddListener((scene) =>
        {
            Init();
            AddListeners();
        });
        SceneLoaoder.Instance.OnRestartLevelLoadedReadOnly.AddListener(() => IsGameStarting = true);
        SceneLoaoder.Instance.OnMenuSceneLoadedReadOnly.AddListener(() => { IsGameStarting = false; });
        SceneLoaoder.Instance.EndOfLevelsReadOnly.AddListener(Victory);
    }
    private void Start() => OnGameOpen?.Invoke();

    private void Init()
    {
        if (_playerHealthManager == null)
        {
            _playerHealthManager = FindAnyObjectByType<PlayerHealthManager>();

            if (_playerHealthManager == null)
                throw new ArgumentNullException(nameof(_playerHealthManager));
        }
    }
    private void AddListeners()
    {
        _playerHealthManager.PlayerDied.AddListener(GameOver);

        UIPanelsEvents.Instance.ButtonPlayClickReadOnly.AddListener(Play);
        UIPanelsEvents.Instance.ButtonRestartClickReadOnly.AddListener(Restart);
        UIPanelsEvents.Instance.ButtonExitGameClickReadOnly.AddListener(ExitGame);
        UIPanelsEvents.Instance.ButtonMenuClickReadOnly.AddListener(Menu);
        UIPanelsEvents.Instance.ButtonPauseClickReadOnly.AddListener(Pause);
        UIPanelsEvents.Instance.ButtonNextLevelClickReadOnly.AddListener(NextLevel);
        UIPanelsEvents.Instance.ButtonContinueClickReadOnly.AddListener(() => IsGameStarting = true);

        FinishInLevelTrigger finishInLevelTrigger = FindAnyObjectByType<FinishInLevelTrigger>();
        if (finishInLevelTrigger != null)
        {
            finishInLevelTrigger.OnPlayerFinishTriggerEnter.AddListener(() => { OnPlayerFinishEnter?.Invoke(); });
            finishInLevelTrigger.OnPlayerFinishTriggerExit.AddListener(() => { OnPlayerFinishExit?.Invoke(); });
        }
    }

    #region InvokeEvents
    private void Pause()
    {
        OnGamePause?.Invoke();
        IsGameStarting = false;
    }
    private void Menu()
    {
        OnMenuOpen?.Invoke();
        IsGameStarting = false;
    }
    private void NextLevel()
    {
        OnNextLevel?.Invoke();
    }
    private void GameOver()
    {
        OnGameOver?.Invoke();
        IsGameStarting = false;
    }
    private void Play()
    {
        OnPlay?.Invoke();
        IsGameStarting = true;
    }
    private void ExitGame()
    {
        OnExitGame?.Invoke();
    }
    private void Victory()
    {
        OnVictory?.Invoke();
        IsGameStarting = false;
    }
    private void Restart()
    {
        OnRestart?.Invoke();
        IsGameStarting = false;
    }
    #endregion
}