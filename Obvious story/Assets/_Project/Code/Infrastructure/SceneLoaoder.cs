using NaughtyAttributes;
using System.Collections;
using UnityEngine;
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
    private bool _isFirstGameLoad = true;
    private bool _isSceneLoading = false;

    private IInvokableEvent<Scene> OnSceneLoad = new SecureUnityEvent<Scene>(), OnSceneLoaoded = new SecureUnityEvent<Scene>();
    private IInvokableEvent EndOfLevels = new SecureUnityEvent(), OnMenuSceneLoaded = new SecureUnityEvent(),
        OnRestartLevelLoaded = new SecureUnityEvent(), OnNextLevelLoaoded = new SecureUnityEvent();

    #region ReadOnly events
    public IReadOnlyEvent<Scene> OnSceneLoadReadOnly => OnSceneLoad;
    public IReadOnlyEvent<Scene> OnSceneLoaodedReadOnly => OnSceneLoaoded;

    public IReadOnlyEvent EndOfLevelsReadOnly => EndOfLevels;
    public IReadOnlyEvent OnMenuSceneLoadedReadOnly => OnMenuSceneLoaded;
    public IReadOnlyEvent OnRestartLevelLoadedReadOnly => OnRestartLevelLoaded;
    public IReadOnlyEvent OnNextLevelLoaodedReadOnly => OnNextLevelLoaoded;
    #endregion

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

    private float DelayBeforeTheSceneStartLoading
    {
        get
        {
            return _usingDelayBeforeStartLoadingLevel ? _delayBeforeTheSceneStartLoading.WithDeviation(_timeSpreadForLoadLevel) : 0;
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

        GameEvents.Instance.OnRestartReadOnly.AddListener(Restart);
        GameEvents.Instance.OnNextLevelReadOnly.AddListener(NextLevel);
        GameEvents.Instance.OnMenuOpenReadOnly.AddListener(Menu);
    }
    private void Start()
    {
        if (_isFirstGameLoad)
        {
            OnSceneLoaoded?.Invoke(SceneManager.GetActiveScene());
            _isFirstGameLoad = false;
        }
    }

    private void LoadScene(string sceneName, float delayToLoadScene, IInvokableEvent unityEventToInvokeBeforeLoaoding = null)
    {
        if (_isSceneLoading == true)
            return;

        OnSceneLoad?.Invoke(SceneManager.GetSceneByName(sceneName));
        StartCoroutine(LoadSceneStart(delayToLoadScene, sceneName, unityEventToInvokeBeforeLoaoding));
    }

    private void Restart()
    {
        LoadScene(_gameSettings.LevelNames[CurrentLevel], DelayBeforeTheSceneStartLoading, OnRestartLevelLoaded);
    }
    private void Menu()
    {
        CurrentLevel = 0;
        LoadScene(_gameSettings.LevelNames[CurrentLevel], DelayBeforeTheSceneStartLoading, OnMenuSceneLoaded);
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

    private IEnumerator LoadSceneStart(float delayToLoadScene, string sceneLoadName, IInvokableEvent unityEvent = null)
    {
        _isSceneLoading = true;
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
        _isSceneLoading = false;

        if (unityEvent != null)
            unityEvent?.Invoke();
    }
}
