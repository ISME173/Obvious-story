using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StartPanel _startPanel;
    [Space]
    [SerializeField] private RestartPanel _restartPanel;
    [SerializeField, Min(0)] private float _timeToActivateRestartPanelBeforePlayerDied = 1.5f;
    [Space]
    [SerializeField] private LoadingPanel _loadingPanel;

    private static UIManager _instance;

    [HideInInspector] public UnityEvent ButtonPlayClick, ButtonRestartClick, ButtonSettingsClick;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();

                if (_instance == null)
                {
                    GameObject uIManager = new GameObject();
                    _instance = uIManager.AddComponent<UIManager>();
                    _instance.name = typeof(UIManager).ToString();
                    return _instance;
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        AddListeners();
    }

    private void AddListeners()
    {
        _startPanel.StartGame.onClick.AddListener((() =>
        {
            ButtonPlayClick?.Invoke();
            _startPanel.Disable();
        }));

        _restartPanel.RestartButton.onClick.AddListener((() =>
        {
            ButtonRestartClick?.Invoke();
            _restartPanel.Disable();
        }));

        GameManager.Instance.OnGameOpen.AddListener((() =>
        {
            if (GameManager.IsFirstGameOpen)
                _startPanel.Enable();
        }));

        GameManager.Instance.OnGameOver.AddListener((() =>
        {
            StartCoroutine(TimerToMethod(_timeToActivateRestartPanelBeforePlayerDied, _restartPanel.Enable));
        }));

        GameManager.Instance.OnRestart.AddListener((() =>
        {
            _loadingPanel.Activate();
        }));
    }
    private IEnumerator TimerToMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}