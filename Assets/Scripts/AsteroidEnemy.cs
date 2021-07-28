using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidEnemy : BaseActiveObject
{
    #region Variables

    // Enemy speed
    [SerializeField] private int _enemySpeed;

    #endregion

    #region Unity

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    protected void OnEnable()
    {
        // Spawning object in specific height
        transform.position = new Vector2(Random.Range(0, _rightConstraint), _topConstraint + 2);
        // Move function
        Move();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // OnCollisionCheck
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnManager.GetInstance().ActiveEnemiesCounter--;
        gameObject.SetActive(false);
    }

    #endregion

    #region Methods

    // Move method
    private void Move()
    {
        // Moving in random direction, when enabled
        _objectRigidbody.AddForce
            (new Vector2(Random.Range(-_enemySpeed, _enemySpeed), Random.Range(-_enemySpeed, _enemySpeed)), ForceMode2D.Impulse);
    }

    #endregion
}
