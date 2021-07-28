using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    // Enemies to spawn
    [SerializeField] private List<GameObject> _enemies;
    // Maximum amount of enemies on screen
    [SerializeField] private int _maxEnemies;
    // Time before spawn
    [SerializeField] private int _timeToRandomlySpawnFrom;
    [SerializeField] private int _timeToRandomlySpawnTo;
    // Counter for active enemies on screen
    private int _activeEnemiesCounter;

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        _activeEnemiesCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks, if we need to spawn new enemy
        SpawnEnemyCheck();
    }

    #endregion

    #region Methods

    // Spawns random enemy on screen
    private void SpawnEnemyCheck()
    {
        if (_activeEnemiesCounter < _maxEnemies)
        {
            _activeEnemiesCounter++;
            Invoke(nameof(SpawnEnemy), Random.Range(_timeToRandomlySpawnFrom, _timeToRandomlySpawnTo));
        }
    }

    // Spawns enemy
    private void SpawnEnemy()
    {
        GameObject toInstantiate = Instantiate(_enemies[Random.Range(0, _enemies.Count)]);
        // Putting this object inside spawn manager
        toInstantiate.transform.parent = transform;
    }

    #endregion
}
