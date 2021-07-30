using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidEnemy : BaseEnemyObject
{
    #region Variables

    // Sprite renderer
    private SpriteRenderer _spriteRender;
    // Asteroid sprites (for random type)
    [SerializeField] private List<Sprite> _asteroidSprites;

    // Minimum speed
    [SerializeField] private int _minSpeed;
    // Maximum speed
    [SerializeField] private int _maxSpeed;

    // Defines, if asteroid was cracked
    private bool _isCracked;

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

    protected void OnEnable()
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
    private void Move()
    {
        int speed = Random.Range(_minSpeed, _maxSpeed);
        // Moving in random direction, when enabled
        _objectRigidbody.AddForce
            (new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)), ForceMode2D.Impulse);
    }

    // Object was hit
    protected override void Hit()
    {
        SpawnManager.GetInstance().ActiveEnemiesCounter--;
        gameObject.SetActive(false);
    }

    #endregion
}
