using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCrackedEnemy : AsteroidEnemy
{
    #region Unity

    protected override void OnEnable()
    {
        // Random sprite choosing
        _spriteRender.sprite = _asteroidSprites[Random.Range(0, _asteroidSprites.Count)];
        // Move function
        Move();
    }

    #endregion

    #region Methods

    // Object was hit
    protected override void Hit()
    {
        SpawnManager.GetInstance().ActiveEnemiesCounter--;
        gameObject.SetActive(false);
    }

    #endregion
}
