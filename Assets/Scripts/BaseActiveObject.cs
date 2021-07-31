using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract object for all interactable objects in game
public abstract class BaseActiveObject : MonoBehaviour
{
    #region Variables

    // Active object rigidbody
    protected Rigidbody2D _objectRigidbody;
    // Screen borders
    protected float _leftConstraint;
    protected float _rightConstraint;
    protected float _bottomConstraint;
    protected float _topConstraint;
    protected float _buffer = 1.0f;
    protected Camera _mainCamera;

    #endregion

    #region Unity

    protected virtual void Awake()
    {
        // Getting object rigidbody
        _objectRigidbody = GetComponent<Rigidbody2D>();
        // Getting our camera
        _mainCamera = Camera.main;
        // Calculating camera constraints
        _leftConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x;
        _rightConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x;
        _bottomConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y;
        _topConstraint = _mainCamera.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, 0.0f)).y;
    }

    protected virtual void Update()
    {
        CheckConstraints();
    }

    // OnCollisionCheck
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Hit(collision);
    }

    #endregion

    #region Methods

    // Checking, if object went of the screen
    protected void CheckConstraints()
    {
        if (transform.position.x < _leftConstraint - _buffer)
        {
            transform.position = new Vector3(_rightConstraint + _buffer, transform.position.y, transform.position.z);
        }
        if (transform.position.x > _rightConstraint + _buffer)
        {
            transform.position = new Vector3(_leftConstraint - _buffer, transform.position.y, transform.position.z);
        }
        if (transform.position.y < _bottomConstraint - _buffer)
        {
            transform.position = new Vector3(transform.position.x, _topConstraint + _buffer, transform.position.z);
        }
        if (transform.position.y > _topConstraint + _buffer)
        {
            transform.position = new Vector3(transform.position.x, _bottomConstraint - _buffer, transform.position.z);
        }
    }

    // Behaviour of an object, after it was hit by some collider
    // Class, which inherit this class must implement and describe
    // how object should react to being hit
    protected abstract void Hit(Collision2D collision);

    #endregion
}