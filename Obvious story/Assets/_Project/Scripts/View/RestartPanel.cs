using UnityEngine;
using UnityEngine.UI;

public class RestartPanel : BasePanel
{
    [Header("UI states")]
    [SerializeField] private Button _restartGame;

    public Button RestartButton => _restartGame;

    private void Awake() => Init();

    protected override void Init()
    {
        base.Init();
        _restartGame = GetComponent<Button>();
    }
}
