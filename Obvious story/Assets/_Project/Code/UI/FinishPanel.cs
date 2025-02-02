using UnityEngine;
using UnityEngine.UI;

public class FinishPanel : BasePanel
{
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _returnButton;

    public Button NextLevelButton => _nextLevelButton;
    public Button ReturnButton => _returnButton;

    private void Awake()
    {
        Init();
    }
}
