using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    [Header("UI states")]
    [SerializeField] private Button _startGame;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _exitGame;

    public Button StartGame => _startGame;
    public Button Settings => _settings;
    public Button ExitGame => _exitGame;

    protected override void Init()
    {
        base.Init();
        _startGame = GetComponent<Button>();
    }
}
