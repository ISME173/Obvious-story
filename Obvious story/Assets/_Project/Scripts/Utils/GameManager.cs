using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;

    private static GameManager _instance;

    [Inject] private PlayerHealthManager _playerHealthManager;

    public event Action OnPlay, OnRestart, OnGameOpen, OnGameOver;

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

        AddListeners();

    }
    private void Start()
    {
        OnGameOpen?.Invoke();
    }
    private void AddListeners()
    {
        _uiManager.ButtonPlayClick += Play;
        _uiManager.ButtonRestartClick += Restart;
        _playerHealthManager.PlayerDied += () =>
        {
            OnGameOver?.Invoke();
        };
    }

    private void Play()
    {
        OnPlay?.Invoke();
        IsGameStarting = true;
    }
    private void Restart()
    {
        OnRestart?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
