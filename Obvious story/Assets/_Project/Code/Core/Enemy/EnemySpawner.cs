using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class EnemySpawner<T> : Spawner<Enemy> where T : Enemy
{
    [SerializeField] protected List<Transform> _enemySpawnPoints;

    [SerializeField] protected bool _useSpawnPointsHowMoving = true;
    [HideIf(nameof(_useSpawnPointsHowMoving))]
    [SerializeField] protected List<Transform> _movingPoints;
 
    [Space]
    [SerializeField] protected T _enemyPrefab;

    [Header("Enemy pool states"), Space]

    [SerializeField] protected bool _useSpawnerHowContaierForEnemy = true;
    [HideIf(nameof(_useSpawnerHowContaierForEnemy))]
    [SerializeField, Min(0)] protected Transform _containerForEnemyInPool;

    [Space]

    [SerializeField] protected bool _putEnemyToPoolInStart = true;
    [ShowIf(nameof(_putEnemyToPoolInStart))]
    [SerializeField, Min(0)] protected int _startPutEnemyToPoolCount;

    [Space]

    [SerializeField] protected bool _addEnemyToPoolIfPoolIsEmpty;
    [ShowIf(nameof(_addEnemyToPoolIfPoolIsEmpty))]
    [SerializeField] protected int _addEnemyToPoolCountIfPoolIsEmpty;

    [Space]

    [SerializeField] protected bool _spawnOnPlay;
    [ShowIf(nameof(_spawnOnPlay))]
    [SerializeField, Min(0)] protected int _spawnCountOnPlay;

    [Space]

    [SerializeField] protected bool _usingMaxCountEnemyInPool;
    [ShowIf(nameof(_usingMaxCountEnemyInPool))]
    [SerializeField] protected int _maxEnemyCountInPool;

    [Inject] protected PlayerMoving _playerMoving;

    private UnityEvent Disable = new();

    private Transform[] MovingPoints
    {
        get
        {
            return _useSpawnPointsHowMoving ? _enemySpawnPoints.ToArray() : _movingPoints.ToArray();
        }
    }
    private Transform ContainerForEnemyInPool
    {
        get
        {
            return _useSpawnerHowContaierForEnemy ? transform : _containerForEnemyInPool;
        }
    }

    private void Start()
    {
        AllEnemySpawnersManager.Instance.OnInit.AddListener((() =>
        {
            Init();
        }));

        if (_spawnOnPlay)
        {
            AllEnemySpawnersManager.Instance.OnSpawn.AddListener((() =>
            {
                for (int i = 0; i < _spawnCountOnPlay; i++)
                    Spawn();
            }));
        }
    }

    private void OnDisable()
    {
        Disable?.Invoke();
    }

    protected void Init()
    {
        _pool = new ObjectPool<Enemy>(ContainerForEnemyInPool, _enemyPrefab);

        if (_putEnemyToPoolInStart)
            InstantiateObjectsToPool(_startPutEnemyToPoolCount);
    }

    protected override void AddListeners(Enemy enemy)
    {
        enemy.OnDied += AddEnemyToPool;
        enemy.Destroyed += RemoveListeners;

        Disable.AddListener((() =>
        {
            enemy.OnDied -= AddEnemyToPool;
            enemy.Destroyed -= RemoveListeners;
        }));
    }
    protected override void RemoveListeners(IDestroyable destroyable)
    {
        Enemy enemy = destroyable as Enemy;
        enemy.OnDied -= AddEnemyToPool;
        enemy.Destroyed -= RemoveListeners;
    }

    protected void AddEnemyToPool(Enemy enemy)
    {
        _pool.Put(enemy);
    }

    [SerializeField, Button("Instantiate enemy to pool")]
    protected void InstantiateObjectsToPool(int instantiateCount = 1)
    {
        for (int i = 0; i < instantiateCount; i++)
        {
            if (_usingMaxCountEnemyInPool)
            {
                if (_pool.CountObjectInPool > _maxEnemyCountInPool)
                {
                    while (_pool.CountObjectInPool > _maxEnemyCountInPool)
                        Destroy(_pool.Get().gameObject);
                }
                else if (_pool.CountObjectInPool == _maxEnemyCountInPool)
                {
                    return;
                }
            }

            Enemy enemy = Instantiate(_enemyPrefab);
            enemy.Init(_playerMoving, _enemySpawnPoints.ToArray(), true);
            _pool.Put(enemy);
        };
    }

    [Button("Spawn enemy")]
    public override Enemy Spawn()
    {
        if (_pool == null)
            _pool = new ObjectPool<Enemy>(ContainerForEnemyInPool, _enemyPrefab);

        if (_pool.CountObjectInPool == 0 && _addEnemyToPoolIfPoolIsEmpty)
            InstantiateObjectsToPool(_addEnemyToPoolCountIfPoolIsEmpty);

        Enemy enemy = _pool.Get(out bool isInstantiated);
        enemy.transform.SetParent(null);
        enemy.transform.position = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)].position;
        enemy.Init(_playerMoving, MovingPoints, isInstantiated);
        AddListeners(enemy);

        return enemy;
    }
}