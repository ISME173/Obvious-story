using System;
using UnityEngine;

[Serializable]
public struct EnemySpawnState
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField, Min(1)] private int _spawnCount;
    [Space]
    [SerializeField] private bool _useSpawnPointsHowMovingPoints;
    [SerializeField] private Transform[] _movingPoints;

    public Transform[] SpawnPoints => _spawnPoints;
    public Transform[] MovingPoints => _useSpawnPointsHowMovingPoints ? _spawnPoints : _movingPoints;
    public int SpawnCount => _spawnCount;
}
