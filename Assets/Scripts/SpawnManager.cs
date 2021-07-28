using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    //Singleton
    private static SpawnManager _instance;
    public static SpawnManager GetInstance()
    {
        if (_instance == null)
        {
            throw new System.Exception();
        }
        return _instance;
    }

    // Enemies to spawn
    [SerializeField] private List<GameObject> _enemiesToSpawn;
    // Maximum amount of enemies on screen
    [SerializeField] private int _maxEnemies;
    // Time before spawn
    [SerializeField] private int _timeToRandomlySpawnFrom;
    [SerializeField] private int _timeToRandomlySpawnTo;
    // Counter for active enemies on screen
    private int _activeEnemiesCounter;
    public int ActiveEnemiesCounter 
    { 
        get { return _activeEnemiesCounter; }
        set { _activeEnemiesCounter = value; }
    }

    // Object pool
    private List<GameObject> _objectPool;

    #endregion

    #region Unity

    // Start is called before the first frame update
    private void Start()
    {
        _instance = this;
        _objectPool = new List<GameObject>();
        ActiveEnemiesCounter = 0;
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
        if (ActiveEnemiesCounter < _maxEnemies)
        {
            ActiveEnemiesCounter++;
            Invoke(nameof(SpawnEnemy), Random.Range(_timeToRandomlySpawnFrom, _timeToRandomlySpawnTo));
        }
    }

    // Spawns enemy
    private void SpawnEnemy()
    {
        // Getting index of enemy, we want to spawn
        int spawnIndex = Random.Range(0, _enemiesToSpawn.Count);
        int objInd = IsObjectInPool(_enemiesToSpawn[spawnIndex]);
        // If object is indeed in pool, then reactivate it
        if (objInd != -1)
        {
            _objectPool[objInd].SetActive(true);
            return;
        }
        GameObject toInstantiate = Instantiate(_enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Count)]);
        // Putting this object inside spawn manager
        toInstantiate.transform.parent = transform;
        // Add object to pool
        _objectPool.Add(toInstantiate);
    }

    // Checking if the object is in the pool
    private int IsObjectInPool(GameObject obj)
    {
        for (int i = 0; i < _objectPool.Count; i++)
        {
            if (obj.tag.Equals(_objectPool[i].tag))
            {
                if (_objectPool[i].activeSelf == false)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    #endregion
}
