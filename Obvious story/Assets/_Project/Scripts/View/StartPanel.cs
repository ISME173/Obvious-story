using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    [Header("UI states")]
    [SerializeField] private Button _startGame;

    public Button StartGame => _startGame;

    private void Awake() => Init();

    protected override void Init()
    {
        base.Init();
        _startGame = GetComponent<Button>();
    }
}
