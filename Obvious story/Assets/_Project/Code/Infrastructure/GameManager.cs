using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEditor.SearchService;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _usingDelayBeforeStartLoadingLevel = true;
    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _delayBeforeTheSceneStartLoading = 1;

    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _timeSpreadForLoadLevel = 0;

    [Space]
    [SerializeField] private List<string> _levelNames;

    private int _currentLevel = 0;

    private static GameManager _instance;

    [HideInInspector] public UnityEvent OnPlay, OnRestart, OnGameOver, RestartSceneLoaoded, OnGameOpen,
        OnPlayerFinishEnter, OnPlayerFinishExit, OnLoadScene;

    [HideInInspector] public UnityEvent<UnityEngine.SceneManagement.Scene> OnSceneLoaoded;

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
    public static bool IsFirstGameOpen { get; private set; } = true;
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

        if (_levelNames.Count == 0)
            throw new ArgumentNullException($"{nameof(_levelNames)} is empty");

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

        OnSceneLoaoded.AddListener(((scene) =>
        {
            AddListeners();

            if (SceneManager.GetActiveScene().name == scene.name)
            {
                RestartSceneLoaoded?.Invoke();
                IsGameStarting = true;
            }
        }));
    }
    private void AddListeners()
    {
        UIManager.Instance.ButtonPlayClick.AddListener(Play);
        UIManager.Instance.ButtonRestartClick.AddListener(Restart);
        UIManager.Instance.ButtonNextLevelClick.AddListener(NextLevel);

        FindAnyObjectByType<PlayerHealthManager>().PlayerDied.AddListener((() =>
        {
            OnGameOver?.Invoke();
        }));

        AddListenersToFinishTrigger();
    }
    private void AddListenersToFinishTrigger()
    {
        FinishInLevelTrigger finishInLevelTrigger = FindAnyObjectByType<FinishInLevelTrigger>();
        if (finishInLevelTrigger != null)
        {
            finishInLevelTrigger.OnPlayerFinishTriggerEnter.AddListener((() =>
            {
                OnPlayerFinishEnter?.Invoke();
                //Debug.Log("On player finish enter");
            }));
            finishInLevelTrigger.OnPlayerFinishTriggerExit.AddListener((() =>
            {
                OnPlayerFinishExit?.Invoke();
                //Debug.Log("On player finish exit");
            }));
        }
        else
        {
            //Debug.Log("Null");
        }
    }
    private void Play()
    {
        OnPlay?.Invoke();
        IsGameStarting = true;
    }
    private void Restart()
    {
        OnRestart?.Invoke();
        IsGameStarting = false;
        IsFirstGameOpen = false;
        LoadScene(SceneManager.GetActiveScene().name, DelayBeforeTheSceneStartLoading);
    }
    private void NextLevel()
    {
        _currentLevel++;

        if (_currentLevel + 1 > _levelNames.Count)
        {
            throw new ArgumentOutOfRangeException($"{nameof(_levelNames)} out of tolerance");
        }

        LoadScene(_levelNames[_currentLevel], DelayBeforeTheSceneStartLoading);
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