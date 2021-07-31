using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombatEnemyShip : BaseEnemyShip
{
    #region Variables



    #endregion

    #region Unity

    // Awake is called in initialization phase
    protected override void Awake()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    #endregion

    #region Methods

    protected override void Hit(Collision2D collision)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
