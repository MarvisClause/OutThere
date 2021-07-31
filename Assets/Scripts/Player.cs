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

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Movement();
    }

    // OnCollisionCheck
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 hitVelocity = _objectRigidbody.velocity;
        _objectRigidbody.velocity = Vector2.zero;
        _objectRigidbody.angularVelocity = 0;
        _objectRigidbody.AddForce(hitVelocity * _playerHitForce, ForceMode2D.Impulse);
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

    #endregion
}
