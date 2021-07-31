using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base asteroid enemy class
public abstract class AsteroidBaseEnemy : BaseActiveObject
{
    #region Variables

    [Header("Sprites")]
    // Sprite renderer
    protected SpriteRenderer _spriteRender;
    // Asteroid sprites (for random type)
    [SerializeField] protected List<Sprite> _asteroidSprites;

    [Header("Asteroid")]
    // Minimum speed
    [SerializeField] protected float _minSpeed;
    // Maximum speed
    [SerializeField] protected float _maxSpeed;

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
        float speed = Random.Range(_minSpeed, _maxSpeed);
        // Moving in random direction, when enabled
        _objectRigidbody.AddForce
            (new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)), ForceMode2D.Impulse);
    }

    // Object was hit
    protected override void Hit(Collision2D collision)
    {
        if (collision.gameObject.tag == Globals.PLAYER_TAG)
        {
            SpawnManager.GetInstance().ActiveEnemiesCounter--;
            gameObject.SetActive(false);
        }
    }

    #endregion
}
