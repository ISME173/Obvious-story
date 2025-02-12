using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameEvents : MonoBehaviour
{
    [Inject, SerializeField] private PlayerHealthManager _playerHealthManager;

    private static GameEvents _instance;

    [HideInInspector]
    public UnityEvent OnGameOpen, OnGameOver, OnVictory, OnPlay, OnRestart, OnExitGame, OnPlayerFinishEnter, OnPlayerFinishExit,
        OnNextLevel, OnMenuOpen, OnGamePause, OnRestartLevelLoaoded;

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

        if (_playerHealthManager == null)
        {
            _playerHealthManager = FindAnyObjectByType<PlayerHealthManager>();

            if (_playerHealthManager == null)
                throw new ArgumentNullException(nameof(_playerHealthManager));
        }

        AddListeners();

        SceneLoaoder.Instance.OnSceneLoaoded.AddListener((scene) =>
        {
            if (_playerHealthManager == null)
            {
                _playerHealthManager = FindAnyObjectByType<PlayerHealthManager>();

                if (_playerHealthManager == null)
                    throw new ArgumentNullException(nameof(_playerHealthManager));
            }

            AddListeners();
        });
        SceneLoaoder.Instance.OnRestartLevelLoaded.AddListener(() =>
        {
            OnRestartLevelLoaoded?.Invoke();
            IsGameStarting = true;
        });
        SceneLoaoder.Instance.OnMenuSceneLoaded.AddListener(() => { IsGameStarting = false; });
        SceneLoaoder.Instance.EndOfLevels.AddListener(Victory);
    }
    private void Start()
    {
        OnGameOpen?.Invoke();
    }
    private void AddListeners()
    {
        _playerHealthManager.PlayerDied.AddListener(GameOver);

        UIPanelsEvents.Instance.ButtonPlayClick.AddListener(Play);
        UIPanelsEvents.Instance.ButtonRestartClick.AddListener(Restart);
        UIPanelsEvents.Instance.ButtonExitGameClick.AddListener(ExitGame);
        UIPanelsEvents.Instance.ButtonMenuClick.AddListener(Menu);
        UIPanelsEvents.Instance.ButtonPauseClick.AddListener(Pause);
        UIPanelsEvents.Instance.ButtonNextLevelClick.AddListener(NextLevel);
        UIPanelsEvents.Instance.ButtonContinueClick.AddListener(() =>
        {
            IsGameStarting = true;
        });

        FinishInLevelTrigger finishInLevelTrigger = FindAnyObjectByType<FinishInLevelTrigger>();
        if (finishInLevelTrigger != null)
        {
            finishInLevelTrigger.OnPlayerFinishTriggerEnter.AddListener(() =>
            {
                OnPlayerFinishEnter?.Invoke();
            });
            finishInLevelTrigger.OnPlayerFinishTriggerExit.AddListener(() =>
            {
                OnPlayerFinishExit?.Invoke();
            });
        }
    }

    #region EventsInvoke 
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