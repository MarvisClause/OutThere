using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombatEnemyShip : BaseEnemyShip
{
    #region Variables


    #endregion

    #region Unity

    // On enable
    protected override void OnEnable()
    {
        base.OnEnable();
        _playerPosition = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Move();
    }

    #endregion

    #region Methods

    // Enemy ship moving
    protected void Move()
    {
        // Add force to move towards object
        _objectRigidbody.AddForce((_playerPosition.position - transform.position).normalized * _enemyShipSpeed, ForceMode2D.Force);
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

    #endregion
}
