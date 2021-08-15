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
    // Enemy ship dodge speed
    [SerializeField] protected float _enemyShipDodgeSpeed;
    // Enemy ship rotation speed
    [SerializeField] protected float _enemyShipRotationSpeed;

    #endregion

    #region Unity

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // Ship behaviour
        ShipBehaviour();
    }

    protected override void OnEnable()
    {
        // Find player object and track its position
        _playerPosition = GameObject.Find("Player").transform;
        // Spawning object in specific height
        _enemyShipRecentHealth = _enemyShipHealthInitial;
        // Base on enable
        base.OnEnable();
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        // Keep distance from other objects to prevent collision
        if (collision.transform.CompareTag(Globals.ENEMY_TAG))
        {
            _objectRigidbody.AddForce((-(collision.transform.position - transform.position).normalized) * _enemyShipDodgeSpeed / 2, ForceMode2D.Force);
        }
        // Try to dodge bullets
        if ( collision.transform.CompareTag(Globals.PLAYER_BULLET_TAG) 
            || collision.transform.CompareTag(Globals.ENEMY_BULLET_TAG))
        {
            _objectRigidbody.AddForce((-(collision.transform.position - transform.position).normalized) * _enemyShipDodgeSpeed * 2, ForceMode2D.Force);
        }
    }

    #endregion

    #region Methods

    // Ship behaviour in the game
    protected abstract void ShipBehaviour();

    // Rotate ship towards player
    protected void RotateTowardsPlayer()
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
    }

    // Hit by player reaction
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
                ScoreManager.GetInstance().AddToScore(_scoreForKill);
            }
        }
    }

    #endregion
}
