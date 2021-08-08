using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCombatEnemyShip : BaseEnemyShip
{
    #region Variables

    // Projectile, which enemy will use to shoot
    [SerializeField] private GameObject _enemyProjectile;
    // Minimum distance to player, before which ship will retreat from player ship
    [SerializeField] private float _minDistToPlayer;
    // Maximum distance to player, after which ship will go forward to player ship
    [SerializeField] private float _maxDistToPlayer;

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        // Checking, if min and max dist to player was correctly entered
        if (_maxDistToPlayer < _minDistToPlayer)
        {
            _maxDistToPlayer = _minDistToPlayer;
        }
    }

    #endregion

    #region Methods

    protected override void ShipBehaviour()
    {
        // Rotate towards player
        // Calculate direction = destination - source
        Vector3 direction = _playerPosition.position - transform.position;
        // Calculate the angle using the inverse tangent method
        // We also subtract 90 degrees from 180, because we use sprites, which are Y-axis oriented
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        // Define the rotation along a specific axis using the angle
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        // Slerp from our current rotation to the new specific rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * _enemyShipRotationSpeed);

        // Get distance between player and enemy ship
        float distance = (_playerPosition.position - transform.position).magnitude;
        if (distance < _minDistToPlayer)
        {
            // Try to get away from player
            _objectRigidbody.AddForce(-(_playerPosition.position - transform.position).normalized * _enemyShipSpeed, ForceMode2D.Force);
        }
        else
        if(distance > _maxDistToPlayer)
        {
            // Get closer to the player
            _objectRigidbody.AddForce((_playerPosition.position - transform.position).normalized * _enemyShipSpeed, ForceMode2D.Force);
        }
    }

    #endregion
}
