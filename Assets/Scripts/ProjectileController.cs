using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : BaseActiveObject
{
    #region Variables

    // Hit effect
    [SerializeField] protected GameObject _hitEffect;
    // Projectile speed
    [SerializeField]protected float _projectileSpeed;
    // Variable for checking if bullet was shot 
    private bool _wasShot;

    #endregion

    #region Unity

    private void OnEnable()
    {
        // Marking bullet as not shot
        _wasShot = false; 
    }

    protected override void Update()
    {
        // When bullet reaches screen edges, it must be disabled
        Vector3 constraintCheck = CheckConstraints();
        if (constraintCheck != Vector3.zero)
        {
            gameObject.SetActive(false);
        }
        if(_wasShot==false)
        {
            // Changing bullet velocity
            _objectRigidbody.velocity = transform.up * _projectileSpeed;
            _wasShot = true;
        }
    }

    #endregion

    #region Methods

    protected override void Hit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Globals.ENEMY_BULLET_TAG)
            || collision.gameObject.CompareTag(Globals.PLAYER_BULLET_TAG)
            || collision.gameObject.CompareTag(Globals.PLAYER_TAG))
        {
            GameObject effect = SpawnManager.GetInstance().SpawnObject(SpawnManager.PoolType.BulletHitEffect, _hitEffect);
            effect.transform.position = transform.position;
            effect.transform.rotation = transform.rotation;
        }
        gameObject.SetActive(false);
    }

    #endregion
}
