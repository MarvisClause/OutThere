using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombatEnemyShip : BaseEnemyShip
{
    #region Unity

    private void FixedUpdate()
    {
        // Add force to move towards object
        _objectRigidbody.AddForce((_playerPosition.position - transform.position).normalized * _enemyShipSpeed, ForceMode2D.Force);
    }

    #endregion

    #region Methods

    // Enemy ship moving
    protected override void ShipBehaviour()
    {
        RotateTowardsPlayer();
    }

    #endregion
}
