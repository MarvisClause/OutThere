using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidEnemy : AsteroidBaseEnemy
{
    #region Variables

    [Header("Asteroid after destruction")]
    // Sub-Asteroids, which will spawn after this will be destroyed
    [SerializeField] private List<GameObject> _asteroidCrackedPrefabs;
    // Maximum amount of asteroids, which can possible crack from this after being hit
    [SerializeField] private int _maxAsteroidsCrackedFrom;

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        // Speed check
        if (_maxSpeed < _minSpeed)
        {
            _maxSpeed = _minSpeed;
        }
        // Getting sprite renderer
        _spriteRender = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    #endregion

    #region Methods

    // Object was hit
    protected override void Hit(Collision2D collision)
    {
        if (collision.gameObject.tag == Globals.PLAYER_TAG)
        {
            if (_asteroidCrackedPrefabs.Count > 0)
            {
                GameObject crackedAsteroid;
                int randomCrackedAsteroids = Random.Range(0, _maxAsteroidsCrackedFrom);
                for (int i = 0; i < randomCrackedAsteroids; i++)
                { 
                    // Spawning asteroids 
                    crackedAsteroid = SpawnManager.GetInstance().SpawnObject(SpawnManager.PoolType.SubEnemies,
                        _asteroidCrackedPrefabs[Random.Range(0, _asteroidCrackedPrefabs.Count)]);
                    crackedAsteroid.transform.position = transform.position; 
                    SpawnManager.GetInstance().ActiveEnemiesCounter++;
                }
            }
            base.Hit(collision);
        }
    }

    #endregion
}
