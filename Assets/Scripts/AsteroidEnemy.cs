using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidEnemy : BaseEnemyObject
{
    #region Variables

    // Sprite renderer
    protected SpriteRenderer _spriteRender;
    // Asteroid sprites (for random type)
    [SerializeField] protected List<Sprite> _asteroidSprites;

    // Sub-Asteroids, which will spawn after this will be destroyed
    [SerializeField] private List<GameObject> _asteroidCrackedPrefabs;
    // Maximum amount of asteroids, which can possible crack from this after being hit
    [SerializeField] private int _maxAsteroidsCrackedFrom;

    // Minimum speed
    [SerializeField] protected int _minSpeed;
    // Maximum speed
    [SerializeField] protected int _maxSpeed;

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

    protected virtual void OnEnable()
    {
        // Random sprite choosing
        _spriteRender.sprite = _asteroidSprites[Random.Range(0, _asteroidSprites.Count)];
        // Spawning object in specific height
        transform.position = new Vector2(Random.Range(0, _rightConstraint), _topConstraint + 1);
        // Move function
        Move();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    #endregion

    #region Methods

    // Move method
    protected void Move()
    {
        int speed = Random.Range(_minSpeed, _maxSpeed);
        // Moving in random direction, when enabled
        _objectRigidbody.AddForce
            (new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)), ForceMode2D.Impulse);
    }

    // Object was hit
    protected override void Hit()
    {
        if (_asteroidCrackedPrefabs.Count > 0)
        {
            int randomCrackedAsteroids = Random.Range(0, _maxAsteroidsCrackedFrom);
            for (int i = 0; i < randomCrackedAsteroids; i++)
            {
                // Spawning asteroids
                SpawnManager.GetInstance().SpawnObject(SpawnManager.PoolType.SubEnemies,
                    _asteroidCrackedPrefabs[Random.Range(0, _asteroidCrackedPrefabs.Count)], true,
                    transform.position);
            }
        }
        SpawnManager.GetInstance().ActiveEnemiesCounter--;
        gameObject.SetActive(false);
    }

    #endregion
}
