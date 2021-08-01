using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActiveObject
{
    #region Variables

    // Player movement speed
    [SerializeField] private float _playerMovementSpeed;
    // Player rotation speed
    [SerializeField] private float _playerRotationSpeed;
    // Player hit force
    [SerializeField] private float _playerHitForce;

    #endregion

    #region Unity

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Movement();
    }

    #endregion

    #region Methods

    // Player movement
    private void Movement()
    {
        // Horizontal and vertical input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Moving
        _objectRigidbody.AddForce(verticalInput * transform.up.normalized * _playerMovementSpeed, ForceMode2D.Force);
        // Rotating
        transform.Rotate(0.0f, 0.0f, -horizontalInput * _playerRotationSpeed * Time.deltaTime);
    }

    protected override void Hit(Collision2D collision)
    {
        _objectRigidbody.AddForce(_objectRigidbody.velocity * _playerHitForce * _playerHitForce, ForceMode2D.Force);
    }

    #endregion
}
