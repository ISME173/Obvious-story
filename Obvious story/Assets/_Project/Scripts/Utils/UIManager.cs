using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StartPanel _startPanel;
    [SerializeField] private RestartPanel _restartPanel;

    public Action ButtonPlayClick, ButtonRestartClick, ButtonSettingsClick;

    private void Awake()
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

        GameManager.Instance.OnGameOpen += () =>
        {
            _startPanel.Enable();
        };
        GameManager.Instance.OnGameOver += () =>
        {
            _restartPanel.Enable();
        };
    }


}
