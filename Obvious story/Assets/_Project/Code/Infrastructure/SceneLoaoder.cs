using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoaoder : MonoBehaviour
{
    [SerializeField] private bool _usingDelayBeforeStartLoadingLevel = true;
    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _delayBeforeTheSceneStartLoading = 1;

    [ShowIf(nameof(_usingDelayBeforeStartLoadingLevel))]
    [SerializeField, Min(0)] private float _timeSpreadForLoadLevel = 0;

    private static SceneLoaoder _instance;

    [Inject] private GameSettings _gameSettings;
    private bool _thisSceneIsMenu = true;

    [HideInInspector] public UnityEvent<Scene> OnSceneLoad, OnSceneLoaoded;
    [HideInInspector] public UnityEvent EndOfLevels, OnMenuSceneLoaded, OnRestartLevelLoaded, OnNextLevelLoaoded;

    public static SceneLoaoder Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<SceneLoaoder>();

                if (_instance == null)
                {
                    GameObject instance = new GameObject();
                    _instance = instance.AddComponent<SceneLoaoder>();
                    _instance.gameObject.name = typeof(SceneLoaoder).ToString();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }

    public int CurrentLevel { get; private set; } = 0;
    public bool IsLastLevel { get; private set; } = false;
    public bool IsFirstLevel { get; private set; } = true;
    public bool ThisSceneIsMenu
    {
        get
        {
            if (GameEvents.Instance.IsGameStarting)
                return false;

            _thisSceneIsMenu = SceneManager.GetActiveScene().name == SceneManager.GetSceneByName(_gameSettings.LevelNames[0]).name;
            return _thisSceneIsMenu;
        }
    }

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

        GameEvents.Instance.OnRestart.AddListener(() => { LoadScene(_gameSettings.LevelNames[CurrentLevel], DelayBeforeTheSceneStartLoading, OnRestartLevelLoaded); });
        GameEvents.Instance.OnNextLevel.AddListener(NextLevel);
        GameEvents.Instance.OnMenuOpen.AddListener(() =>
        {
            CurrentLevel = 0;
            LoadScene(_gameSettings.LevelNames[CurrentLevel], DelayBeforeTheSceneStartLoading, OnMenuSceneLoaded);
        });
    }

    private void LoadScene(string sceneName, float delayToLoadScene, UnityEvent unityEventToInvokeBeforeLoaoding = null)
    {
        OnSceneLoad?.Invoke(SceneManager.GetSceneByName(sceneName));
        StartCoroutine(LoadSceneStart(delayToLoadScene, sceneName, unityEventToInvokeBeforeLoaoding));
    }

    private void NextLevel()
    {
        CurrentLevel++;

        if (CurrentLevel >= _gameSettings.LevelNames.Length)
        {
            IsLastLevel = true;
            EndOfLevels?.Invoke();
            return;
        }

        LoadScene(_gameSettings.LevelNames[CurrentLevel], DelayBeforeTheSceneStartLoading, OnNextLevelLoaoded);
    }

    private IEnumerator LoadSceneStart(float delayToLoadScene, string sceneLoadName, UnityEvent unityEvent = null)
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

        if (unityEvent != null)
            unityEvent?.Invoke();
    }
}
