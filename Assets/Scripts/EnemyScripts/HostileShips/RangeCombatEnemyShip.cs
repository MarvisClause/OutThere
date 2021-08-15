using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCombatEnemyShip : BaseEnemyShip
{
    #region Variables

    // Animator variable
    private string STATE_ANIMATION_VARIABLE_NAME = "State";
    // RangeCombatShip states
    private enum EnemyRangeShipState
    {
        Loaded,
        Reloading,
        Shooting
    }
    // Enemy animation
    private Animator _enemyAnimation;
    // Enemy state
    private EnemyRangeShipState _enemyState;

    // Projectile, which enemy will use to shoot
    [SerializeField] private GameObject _enemyProjectile;
    // Projectile reload time
    [SerializeField] private float _reloadTime;
    // Minimum distance to player, before which ship will retreat from player ship
    [SerializeField] private float _minDistToPlayer;
    // Maximum distance to player, after which ship will go forward to player ship
    [SerializeField] private float _maxDistToPlayer;
    // Distance to player
    private float _distance;

    #endregion

    #region Unity
    private void FixedUpdate()
    {
        // Get distance between player and enemy ship
        _distance = (_playerPosition.position - transform.position).magnitude;
        if (_distance < _minDistToPlayer)
        {
            // Try to get away from player
            _objectRigidbody.AddForce(-(_playerPosition.position - transform.position).normalized * _enemyShipSpeed, ForceMode2D.Force);
        }
        else
        if (_distance > _maxDistToPlayer)
        {
            // Get closer to the player
            _objectRigidbody.AddForce((_playerPosition.position - transform.position).normalized * _enemyShipSpeed, ForceMode2D.Force);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // Getting animation controller
        _enemyAnimation = GetComponent<Animator>();
        // Setting enemy condition to reloading by default
        _enemyState = EnemyRangeShipState.Reloading;
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
        RotateTowardsPlayer();

        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, _playerPosition.position - transform.position);
        if (_distance < _maxDistToPlayer 
            && _enemyState == EnemyRangeShipState.Loaded
            && hit.collider.CompareTag(Globals.PLAYER_TAG))
        {
            // Changing state to shooting
            // Unity animator, will call shooting method in the end of the animation
            _enemyState = EnemyRangeShipState.Shooting;
            // Shoot sound
            SoundManager.GetInstance().PlaySound(Globals.ENEMY_SHIP_FIRE);
        }

        // Setting animation state
        _enemyAnimation.SetInteger(STATE_ANIMATION_VARIABLE_NAME, (int)_enemyState);
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
        // Changing state to reloading
        _enemyState = EnemyRangeShipState.Reloading;
        // Invoke reload method
        Invoke(nameof(ReloadProjectile), _reloadTime);
    }

    // Reloads projectile
    protected void ReloadProjectile()
    {
        // Change state to loaded
        _enemyState = EnemyRangeShipState.Loaded;
        CancelInvoke();
    }

    #endregion
}
