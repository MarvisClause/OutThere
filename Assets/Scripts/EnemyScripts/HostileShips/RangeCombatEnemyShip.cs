using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCombatEnemyShip : BaseEnemyShip
{
    #region Variables

    // Projectile, which enemy will use to shoot
    [SerializeField] private GameObject _enemyProjectile;
    // Projectile reload time
    [SerializeField] private float _reloadTime;
    // Minimum distance to player, before which ship will retreat from player ship
    [SerializeField] private float _minDistToPlayer;
    // Maximum distance to player, after which ship will go forward to player ship
    [SerializeField] private float _maxDistToPlayer;
    // Is ship reloading
    protected bool _isReloaded;

    #endregion

    #region Unity

    protected override void Awake()
    {
        base.Awake();
        // Reloading projectile
        Invoke(nameof(ReloadProjectile), _reloadTime);
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

        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, _playerPosition.position - transform.position);
        Debug.DrawRay(transform.position + transform.up, _playerPosition.position - transform.position);
        if (distance < _maxDistToPlayer && _isReloaded == true && hit.collider.CompareTag(Globals.PLAYER_TAG))
        {
            ShootProjectile();
            Invoke(nameof(ReloadProjectile), _reloadTime);
        }
    }

    // Shoots projectile in front of the ship
    protected void ShootProjectile()
    {
        // Setting bullet position and its rotation
        Vector3 bulletPos = transform.position + transform.up;
        Quaternion bulletRotate = transform.rotation;
        // Requesting an object from spawn manager
        GameObject bulletInstance = SpawnManager.GetInstance().SpawnObject(SpawnManager.PoolType.PlayerBullets, _enemyProjectile);
        // Setting its position and rotation
        bulletInstance.transform.position = bulletPos;
        bulletInstance.transform.rotation = bulletRotate;
        // Setting projectile as fired
        _isReloaded = false;
    }

    // Reloads projectile
    protected void ReloadProjectile()
    {
        _isReloaded = true;
    }

    #endregion
}
