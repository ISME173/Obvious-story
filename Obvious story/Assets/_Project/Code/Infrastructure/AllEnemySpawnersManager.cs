using UnityEngine;
using UnityEngine.Events;

public class AllEnemySpawnersManager : MonoBehaviour
{
    private static AllEnemySpawnersManager _instance;

    public static AllEnemySpawnersManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<AllEnemySpawnersManager>();

                if (_instance == null)
                {
                    GameObject allEnemySpawnerManager = new GameObject();
                    _instance = allEnemySpawnerManager.AddComponent<AllEnemySpawnersManager>();
                    _instance.name = typeof(AllEnemySpawnersManager).Name;
                    return _instance;
                }
            }
            return _instance;
        }
    }

    public UnityEvent OnSpawn { get; private set; } = new();
    public UnityEvent OnInit { get; private set; } = new();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        AddListeners();
    }


    private void AddListeners()
    {
        GameEvents.Instance.OnGameOpenReadOnly.AddListener(() => OnInit?.Invoke());
        GameEvents.Instance.OnPlayReadOnly.AddListener(() => OnSpawn?.Invoke());

        SceneLoaoder.Instance.OnRestartLevelLoadedReadOnly.AddListener(() => OnSpawn?.Invoke());
        SceneLoaoder.Instance.OnNextLevelLoaodedReadOnly?.AddListener(() => OnSpawn?.Invoke());
    }
}
