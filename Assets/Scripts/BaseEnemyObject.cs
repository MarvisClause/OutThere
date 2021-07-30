using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyObject : BaseActiveObject
{
    #region Unity

    // OnCollisionCheck
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals(Globals.PLAYER_TAG) == true)
        {
            Hit();
        }
    }

    #endregion

    #region Methods

    // Behaviour of an object, after it was hit
    protected abstract void Hit();

    #endregion
}
