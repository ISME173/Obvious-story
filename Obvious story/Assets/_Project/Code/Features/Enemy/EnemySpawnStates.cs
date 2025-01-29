using System;
using UnityEngine;

[Serializable]
public struct EnemySpawnStates
{
    [SerializeField] private int _spawnCount;
    [SerializeField] private Transform[] _movingPoints;
    [SerializeField] private Transform[] _spawnPoints;

    public Transform[] MovingPoints => _movingPoints;
    public Transform[] SpawnPonts => _spawnPoints;
    public int SpawnCount => _spawnCount;
}
