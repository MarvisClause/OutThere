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

    // Player rigidbody
    private Rigidbody2D _playerRigidbody;

    #endregion

    #region Unity

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

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
        _playerRigidbody.AddForce(verticalInput * transform.up.normalized * _playerMovementSpeed, ForceMode2D.Force);
        // Rotating
        transform.Rotate(0.0f, 0.0f, -horizontalInput * _playerRotationSpeed * Time.deltaTime);
    }

    #endregion
}
