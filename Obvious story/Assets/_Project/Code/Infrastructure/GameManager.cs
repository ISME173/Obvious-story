using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _usingDelayBeforeStartLoadingLevel = true;
    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _delayBeforeTheSceneStartLoading = 1;

    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _timeSpreadForLoadLevel = 0;

    private static GameManager _instance;

    [Inject] private GameSettings _gameSettings;
    [Inject] private PlayerHealthManager _playerHealthManager;
    private int _currentLevel = 0;

    [HideInInspector]
    public UnityEvent OnPlay, OnRestart, OnGameOver, RestartSceneLoaoded, OnGameOpen,
        OnPlayerFinishEnter, OnPlayerFinishExit, OnLoadScene, OnGamePause, OnVictory;

    [HideInInspector] public UnityEvent<Scene> OnSceneLoaoded;

    private float DelayBeforeTheSceneStartLoading
    {
        get
        {
            if (_usingDelayBeforeStartLoadingLevel)
            {
                return UnityEngine.Random.Range(_delayBeforeTheSceneStartLoading - _timeSpreadForLoadLevel, _delayBeforeTheSceneStartLoading
                    + _timeSpreadForLoadLevel);
            }
            else
            {
                return 0;
            }
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GameManager>();

                if (_instance == null)
                {
                    GameObject gameManager = new GameObject();
                    _instance = gameManager.AddComponent<GameManager>();
                    gameManager.name = typeof(GameManager).ToString();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }
    public bool IsFirstGameOpen { get; private set; } = true;

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

        if (_gameSettings.LevelNames.Length == 0)
            throw new ArgumentNullException($"{nameof(_gameSettings.LevelNames)} is empty");

        IsFirstGameOpen = true;
        AddListenersOnFirstGameOpen();
    }
    private void Start()
    {
        OnGameOpen?.Invoke();
    }
    private void AddListenersOnFirstGameOpen()
    {
        AddListeners();

        OnSceneLoaoded.AddListener((scene) =>
        {
            AddListeners();

            if (IsFirstGameOpen == false && SceneManager.GetActiveScene().name == scene.name)
            {
                RestartSceneLoaoded?.Invoke();
                IsGameStarting = true;
            }
        });
    }
    private void AddListeners()
    {
        UIManager.Instance.ButtonPlayClick.AddListener(Play);
        UIManager.Instance.ButtonRestartClick.AddListener(Restart);
        UIManager.Instance.ButtonNextLevelClick.AddListener(NextLevel);
        UIManager.Instance.ButtonPauseClick.AddListener(() =>
        {
            IsGameStarting = false;
            OnGamePause?.Invoke();
        });
        UIManager.Instance.ButtonContinueClick.AddListener(() => { IsGameStarting = true; });
        UIManager.Instance.MenuButtonClick.AddListener(() =>
        {
            _currentLevel = 0;
            IsGameStarting = false;
            IsFirstGameOpen = true;
            LoadScene(_gameSettings.LevelNames[_currentLevel], DelayBeforeTheSceneStartLoading);
        });

        if (_playerHealthManager == null)
            _playerHealthManager = FindAnyObjectByType<PlayerHealthManager>();

        _playerHealthManager.PlayerDied.AddListener(() =>
        {
            OnGameOver?.Invoke();
            IsGameStarting = false;
        });

        AddListenersToFinishTrigger();
    }
    private void AddListenersToFinishTrigger()
    {
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
    private void Play()
    {
        OnPlay?.Invoke();
        IsGameStarting = true;
    }
    private void Restart()
    {
        IsGameStarting = false;
        IsFirstGameOpen = false;
        OnRestart?.Invoke();
        LoadScene(SceneManager.GetActiveScene().name, DelayBeforeTheSceneStartLoading);
    }
    private void Victory()
    {
        IsGameStarting = false;
        OnVictory?.Invoke();
    }
    private void NextLevel()
    {
        IsFirstGameOpen = false;
        _currentLevel++;

        if (_currentLevel + 1 > _gameSettings.LevelNames.Length)
        {
            Victory();
            return;
        }

        LoadScene(_gameSettings.LevelNames[_currentLevel], DelayBeforeTheSceneStartLoading);
    }
    private void LoadScene(string sceneName, float delayToLoadScene)
    {
        OnLoadScene?.Invoke();
        StartCoroutine(LoadSceneStart(delayToLoadScene, sceneName));
    }
    private IEnumerator LoadSceneStart(float delayToLoadScene, string sceneLoadName)
    {
        yield return new WaitForSeconds(delayToLoadScene);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneLoadName);

        while (asyncOperation.isDone == false)
            yield return null;

        if (SceneManager.GetActiveScene().isLoaded == false)
        {
            while (SceneManager.GetActiveScene().isLoaded == false)
                yield return null;
        }

        OnSceneLoaoded?.Invoke(SceneManager.GetSceneByName(sceneLoadName));
    }
}