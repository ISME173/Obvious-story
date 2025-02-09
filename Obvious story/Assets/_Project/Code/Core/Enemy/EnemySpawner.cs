using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class EnemySpawner<Data, EnemyType> : Spawner<EnemyType> where EnemyType : Enemy<Data> where Data : EnemyData
{
    [SerializeField] protected List<EnemySpawnState> _enemySpawnStates;
    [Space]
    [SerializeField] protected EnemyType _enemyPrefab;
    [SerializeField] protected bool _spawnOnPlay;

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

    [SerializeField] protected bool _usingMaxCountEnemyInPool;
    [ShowIf(nameof(_usingMaxCountEnemyInPool))]
    [SerializeField] protected int _maxEnemyCountInPool;

    [Inject] protected PlayerMoving _playerMoving;

    private UnityEvent Disable = new();

    private Transform ContainerForEnemyInPool
    {
        get
        {
            return _useSpawnerHowContaierForEnemy ? transform : _containerForEnemyInPool;
        }
    }

    private void Awake()
    {
        AllEnemySpawnersManager.Instance.OnInit.AddListener((() =>
        {
            Init();
        }));

        if (_spawnOnPlay)
        {
            AllEnemySpawnersManager.Instance.OnSpawn.AddListener((() =>
            {
                for (int i = 0; i < _enemySpawnStates.Count; i++)
                {
                    for (int j = 0; j < _enemySpawnStates[i].SpawnCount; j++)
                        Spawn(_enemySpawnStates[i]);
                }
            }));
        }
    }

    private void OnDisable()
    {
        Disable?.Invoke();
    }

    protected virtual void Init()
    {
        _pool = new ObjectPool<EnemyType>(ContainerForEnemyInPool, _enemyPrefab);

        if (_putEnemyToPoolInStart)
        {
            for (int i = 0; i < _enemySpawnStates.Count; i++)
            {
                InstantiateObjectsToPool(_enemySpawnStates[i], _enemySpawnStates[i].SpawnCount);
            }
        }
    }

    protected override void AddListeners(EnemyType enemy)
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
        EnemyType enemy = destroyable as EnemyType;
        enemy.OnDied -= AddEnemyToPool;
        enemy.Destroyed -= RemoveListeners;
    }

    protected void AddEnemyToPool(Enemy<Data> enemy)
    {
        EnemyType putEnemy = enemy as EnemyType;
        _pool.Put(putEnemy);
    }
    protected void InstantiateObjectsToPool(EnemySpawnState enemySpawnState, int instantiateCount)
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

            EnemyType enemy = Instantiate(_enemyPrefab);
            enemy.Init(_playerMoving, enemySpawnState.MovingPoints, true);
            _pool.Put(enemy);
        };
    }

    [SerializeField, Button("Instantiate enemy with random states to pool")]
    protected void InstantiateObjectsToPoolWithRandomStates()
    {
        EnemySpawnState enemySpawnState = _enemySpawnStates[Random.Range(0, _enemySpawnStates.Count)];

        for (int i = 0; i < enemySpawnState.SpawnCount; i++)
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

            EnemyType enemy = Instantiate(_enemyPrefab);
            enemy.Init(_playerMoving, enemySpawnState.MovingPoints, true);
            _pool.Put(enemy);
        };
    }

    public EnemyType Spawn(EnemySpawnState enemySpawnState)
    {
        if (_pool == null)
            _pool = new ObjectPool<EnemyType>(ContainerForEnemyInPool, _enemyPrefab);

        if (_pool.CountObjectInPool == 0 && _addEnemyToPoolIfPoolIsEmpty)
            InstantiateObjectsToPool(enemySpawnState, _addEnemyToPoolCountIfPoolIsEmpty);

        EnemyType enemy = _pool.Get(out bool isInstantiated);
        enemy.transform.SetParent(null);
        enemy.transform.position = enemySpawnState.SpawnPoints[Random.Range(0, enemySpawnState.SpawnPoints.Length)].position;

        enemy.Init(_playerMoving, enemySpawnState.MovingPoints, isInstantiated);

        AddListeners(enemy);

        return enemy;
    }


    [Button("Spawn an enemy with random spawn parameters")]
    public override EnemyType Spawn()
    {
        EnemySpawnState enemySpawnState = _enemySpawnStates[Random.Range(0, _enemySpawnStates.Count)];

        if (_pool == null)
            _pool = new ObjectPool<EnemyType>(ContainerForEnemyInPool, _enemyPrefab);

        if (_pool.CountObjectInPool == 0 && _addEnemyToPoolIfPoolIsEmpty)
            InstantiateObjectsToPool(enemySpawnState, _addEnemyToPoolCountIfPoolIsEmpty);

        EnemyType enemy = _pool.Get(out bool isInstantiated);
        enemy.transform.SetParent(null);
        enemy.transform.position = enemySpawnState.SpawnPoints[Random.Range(0, enemySpawnState.SpawnPoints.Length)].position;

        enemy.Init(_playerMoving, enemySpawnState.MovingPoints, isInstantiated);

        AddListeners(enemy);

        return enemy;
    }
}