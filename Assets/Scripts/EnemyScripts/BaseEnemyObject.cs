using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyObject : BaseActiveObject
{
    #region Unity

    // OnCollisionCheck
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Hit(collision);
    }

    #endregion

    #region Methods

    // Behaviour of an object, after it was hit by some collider
    // Class, which inherit this class must implement and describe
    // how object should react to being hit
    protected abstract void Hit(Collision2D collision);

    #endregion
}
