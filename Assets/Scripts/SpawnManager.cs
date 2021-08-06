using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Pool types
    public enum PoolType
    {
        Enemies,
        SubEnemies, 
        PlayerBullets
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
        SpawnObject(PoolType.Enemies, _enemiesToSpawn[spawnIndex]);
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
            if (obj.tag.Equals(pool[i].tag))
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
