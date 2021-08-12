using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyObject : BaseActiveObject
{
    #region Variables

    // Enemy hit effect
    [SerializeField] protected GameObject _enemyHitEffect;
    // Score for enemy kill
    [SerializeField] protected int _scoreForKill;

    #endregion

    #region Unity

    protected virtual void OnEnable()
    {
        // Randomized spawn from map edges
        switch (Random.Range(1, 5))
        {
            case 1:
                 // Random x, static TOP y
                 transform.position = new Vector2(Random.Range(_leftConstraint, _rightConstraint), _topConstraint + _screenEdgeBuffer);
                break;
            case 2:
                // Random x, static BOTTOM y
                transform.position = new Vector2(Random.Range(_leftConstraint, _rightConstraint), _bottomConstraint - _screenEdgeBuffer);
                break;
            case 3:
                // Static LEFT x, random y
                transform.position = new Vector2(_leftConstraint - _screenEdgeBuffer, Random.Range(_bottomConstraint, _topConstraint));
                break;
            case 4:
                // Static RIGHT x, random y
                transform.position = new Vector2(_rightConstraint + _screenEdgeBuffer, Random.Range(_bottomConstraint, _topConstraint));
                break;
        }
    }

    #endregion

    #region Methods

    protected override void Hit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Globals.PLAYER_TAG) 
            || collision.gameObject.CompareTag(Globals.PLAYER_BULLET_TAG))
        {
            // Checking, if player is invincible
            if (!GameManager.GetInstance().IsPlayerInvincible)
            {
                // Spawn effect
                GameObject effect = SpawnManager.GetInstance().SpawnObject(SpawnManager.PoolType.EnemyHitEffect, _enemyHitEffect);
                effect.transform.position = transform.position;
                effect.transform.rotation = transform.rotation;
                // Play hit sound
                SoundManager.GetInstance().PlaySound(Globals.ENEMY_HIT_SOUND);
                // Execute effect
                HitByPlayerEffect(collision);
            }
            }
        }

    // Method, which decribes, what happes after collision with player or bullet
    protected abstract void HitByPlayerEffect(Collision2D collision);

    #endregion
}
