using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base enemy ship class
public abstract class BaseEnemyShip : BaseEnemyObject
{
    #region Variables

    // Player position for enemy ship navigation
    protected Transform _playerPosition;
    // Ship initial health
    [SerializeField] protected float _enemyShipHealthInitial;
    // Enemy ship recent health
    protected float _enemyShipRecentHealth;
    // Enemy ship speed
    [SerializeField] protected float _enemyShipSpeed;
    // Enemy ship rotation speed
    [SerializeField] protected float _enemyShipRotationSpeed;

    #endregion

    #region Unity

    protected override void OnEnable()
    {
        // Spawning object in specific height
        _enemyShipRecentHealth = _enemyShipHealthInitial;
        base.OnEnable();
    }

    #endregion

    #region Methods

    protected override void HitByPlayerEffect(Collision2D collision)
    {
        if (collision.gameObject.tag == Globals.PLAYER_BULLET_TAG)
        {
            _enemyShipRecentHealth--;
            // If enemy ship health equal zero, than it will explode
            if (_enemyShipRecentHealth <= 0)
            {
                SpawnManager.GetInstance().ActiveEnemiesCounter--;
                gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
