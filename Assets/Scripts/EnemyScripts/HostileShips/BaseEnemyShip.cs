using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base enemy ship class
public abstract class BaseEnemyShip : BaseActiveObject
{
    #region Variables

    // Player position for enemy ship navigation
    protected Transform _playerPosition;
    // Amount of damage, which ship can handle
    [SerializeField] protected float _enemyShipHealth;
    // Enemy ship speed
    [SerializeField] protected float _enemyShipSpeed;
    // Enemy ship rotation speed
    [SerializeField] protected float _enemyShipRotationSpeed;

    #endregion

    #region Unity

    protected virtual void OnEnable()
    {
        // Spawning object in specific height
        transform.position = new Vector2(Random.Range(0, _rightConstraint), _topConstraint + 1);
    }

    #endregion

    #region Methods

    protected override void Hit(Collision2D collision)
    {

    }

    #endregion
}
