using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base enemy ship class
public abstract class BaseEnemyShip : BaseEnemyObject
{
    #region Variables

    // Player position for enemy ship navigation
    [SerializeField] protected Transform _playerPosition;
    // Amount of damage, which ship can handle
    [SerializeField] protected int _enemyShipHealth;

    #endregion

    #region Unity


    #endregion

    #region Methods


    #endregion
}
