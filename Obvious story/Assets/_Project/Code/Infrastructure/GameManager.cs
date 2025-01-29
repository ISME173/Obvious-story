using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _usingDelayBeforeStartLoadingLevel = true;
    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _delayBeforeTheSceneStartLoading = 1;

    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _timeSpreadForLoadLevel = 0;

    [Inject] private PlayerHealthManager _playerHealthManager;

    private static GameManager _instance;

    [HideInInspector] public UnityEvent OnPlay, OnRestart, OnGameOver, RestartSceneLoaoded;
    [HideInInspector] public UnityEvent<Scene> OnSceneLoaoded;
    [HideInInspector] public UnityEvent OnGameOpen;

    private float DelayBeforeTheSceneStartLoading
    {
        get
        {
            if (_usingDelayBeforeStartLoadingLevel)
            {
                return Random.Range(_delayBeforeTheSceneStartLoading - _timeSpreadForLoadLevel, _delayBeforeTheSceneStartLoading
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
        IsFirstGameOpen = true;
        AddListeners();
    }
    private void Start()
    {
        OnGameOpen?.Invoke();
    }
    private void AddListeners()
    {
        UIManager.Instance.ButtonPlayClick.AddListener(Play);
        UIManager.Instance.ButtonRestartClick.AddListener(Restart);

        OnSceneLoaoded.AddListener(((scene) =>
        {
            if (SceneManager.GetActiveScene().name == scene.name)
            {
                RestartSceneLoaoded?.Invoke();
                IsGameStarting = true;

                _playerHealthManager.PlayerDied.AddListener((() =>
                {
                    OnGameOver?.Invoke();
                }));
            }
        }));

        _playerHealthManager.PlayerDied.AddListener((() =>
        {
            OnGameOver?.Invoke();
        }));

        RestartSceneLoaoded.AddListener((() =>
        {
            UIManager.Instance.ButtonRestartClick.AddListener(Restart);

            if (_playerHealthManager == null)
            {
                _playerHealthManager = FindAnyObjectByType<PlayerHealthManager>();
            }
        }));
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

    private void LoadScene(string sceneName, float delayToLoadScene)
    {
        StartCoroutine(LoadSceneStart(delayToLoadScene, sceneName));
    }
    private IEnumerator LoadSceneStart(float delayToLoadScene, string sceneLoadName)
    {
        yield return new WaitForSeconds(delayToLoadScene);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneLoadName);

        while (!asyncOperation.isDone)
            yield return null;

        if (SceneManager.GetActiveScene().isLoaded == false)
        {
            while (SceneManager.GetActiveScene().isLoaded == false)
                continue;
        }

        OnSceneLoaoded?.Invoke(SceneManager.GetSceneByName(sceneLoadName));
    }
}