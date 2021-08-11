using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Pool types
    public enum PoolType
    {
        // Objects to spawn
        Asteroids,
        MeleeCombatShips,
        RangeCombatShips,
        // Specific objects
        MiniAsteroids, 
        PlayerBullets,
        EnemyHitEffect
    }

    #region Variables

    //Singleton
    private static SpawnManager _instance;
    public static SpawnManager GetInstance()
    {
        if (_instance == null)
        {
            Debug.LogWarning("Instance of spawn manager is null referenced!");
            throw new System.Exception("Instance of spawn manager is null referenced!");
        }
        return _instance;
    }
    // Enemies to spawn
    [SerializeField] private List<GameObject> _enemiesToSpawn;
    // Initial amount of max enemies
    [SerializeField] private int _initialMaxEnemies = 1;
    // Recent max amount of enemies on screen
    private int _recentMaxEnemies;
    // Time before spawn
    [SerializeField] private int _timeToRandomlySpawnFrom;
    [SerializeField] private int _timeToRandomlySpawnTo;
    // Variable, which defines when new enemy will appear, depending on score value
    [SerializeField] private int _newEnemyMultipleValue = 100;
    // Last score value for new enemy multiply
    private int _lastNewEnemyScore;
    // Variable, which defines when new enemy will appear, depending on score value
    [SerializeField] private int _newMaxQuantityOfEnemyMultipleValue = 50;
    // Last score value for new max enemy quantity
    private int _lastNewMaxEnemyQuantityScore;
    // Counter for active enemies on screen
    private int _activeEnemiesCounter;
    public int ActiveEnemiesCounter
    {
        get { return _activeEnemiesCounter; }
        set { _activeEnemiesCounter = value; }
    }
    // Variable for enemy spawn control, which depends on score
    private int _maxSpawnIndex;

    // Pools
    private List<List<GameObject>> _pools;

    #endregion

    #region Unity

    // Start is called before the first frame update
    private void Start()
    {
        _instance = this;
        _pools = new List<List<GameObject>>();
        for (int i = 0; i < PoolType.GetNames(typeof(PoolType)).Length; i++)
        {
            _pools.Add(new List<GameObject>());
        }
        ActiveEnemiesCounter = 0;
        if (_timeToRandomlySpawnFrom > _timeToRandomlySpawnTo)
        {
            _timeToRandomlySpawnFrom = _timeToRandomlySpawnTo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks, if we need to spawn new enemy
        SpawnEnemyCheck();
        // Checks and react to player progression via score
        ProgressionCheck();
    }

    #endregion

    #region Methods

    // Disables all objects
    public void DisableAllObjects()
    {
        // Cancel all invoke methods in this class
        CancelInvoke();
        // Setting active enemies counter to zero
        ActiveEnemiesCounter = 0;
        // Setting max spawn index to zero
        _maxSpawnIndex = 1;
        // Resetting recent enemy quant value
        _recentMaxEnemies = _initialMaxEnemies;
        // Iterate through each pool
        for (int poolIteration = 0; poolIteration < _pools.Count; poolIteration++)
        {
            // Iterate through each element in list
            for (int listIteration = 0; listIteration < _pools[poolIteration].Count; listIteration++)
            {
                _pools[poolIteration][listIteration].SetActive(false);
            }
        }
    }

    // Checks player progression and changes values according to it
    private void ProgressionCheck()
    {
        if (GameManager.GetInstance().IsPlayerActive == true)
        {
            int recentScore = ScoreManager.GetInstance().PlayerScore;
            // Progression system
            if (recentScore != 0)
            {
                // When score reaches this condition, we add new type of enemy
                if (recentScore % _newEnemyMultipleValue == 0
                    && _maxSpawnIndex < _enemiesToSpawn.Count
                    && _lastNewEnemyScore != recentScore)
                {
                    _maxSpawnIndex++;
                    _lastNewEnemyScore = recentScore;
                    Debug.Log("Increase max spawn index to " + _maxSpawnIndex);
                }
                // When score reaches this condition, we increase max amount of enemies on screen
                if (recentScore % _newMaxQuantityOfEnemyMultipleValue == 0
                    && _lastNewMaxEnemyQuantityScore != recentScore)
                {
                    _recentMaxEnemies++;
                    _lastNewMaxEnemyQuantityScore = recentScore;
                    Debug.Log("Increase max amount of enemies to " + _recentMaxEnemies);
                }
            }
        }
    }

    // Spawns random enemy on screen
    private void SpawnEnemyCheck()
    {
        if (GameManager.GetInstance().IsPlayerActive == false)
        {
            // This amount of enemies is used for main menu showcase.
            // It is more for visual appearance, rather than gameplay
            _recentMaxEnemies = 25;
        }
        if (ActiveEnemiesCounter < _recentMaxEnemies)
        {
            ActiveEnemiesCounter++;
            Invoke(nameof(SpawnEnemy), Random.Range(_timeToRandomlySpawnFrom, _timeToRandomlySpawnTo));
        }
    }

    // Spawns enemy
    private void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, _maxSpawnIndex);
        SpawnObject((PoolType)spawnIndex, _enemiesToSpawn[spawnIndex]);
    }

    /// <summary>
    /// Spawns object
    /// </summary>
    /// <param name="poolType">Pool type in which object should be spawned</param>
    /// <param name="gameObject">Object to spawn</param>
    public GameObject SpawnObject(PoolType poolType, GameObject gameObject) 
    {
        List<GameObject> pool = _pools[(int)poolType];
        int objInd = IsObjectInPool(pool, gameObject); // If object is indeed in pool, then reactivate it
        if (objInd != -1)
        {
            pool[objInd].SetActive(true);
            return pool[objInd];
        }
        else
        {
            // Instantiate object
            GameObject toInstantiate = Instantiate(gameObject);
            // Putting this object inside spawn manager
            toInstantiate.transform.parent = transform;
            // Add object to pool
            pool.Add(toInstantiate);
            return toInstantiate;
        }
    }

    // Checking if the object is in the pool
    private int IsObjectInPool(List<GameObject> pool, GameObject obj)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (obj.CompareTag(pool[i].tag))
            {
                if (pool[i].activeSelf == false)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    #endregion
}
