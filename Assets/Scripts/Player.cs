using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseActiveObject
{
    #region Variables

    // Player max health 
    [SerializeField] private int _playerHealthMaxCapacity;
    // Player health for game session
    private int _playerRecentHealth;
    // Player movement speed
    [SerializeField] private float _playerMovementSpeed;
    // Player rotation speed
    [SerializeField] private float _playerRotationSpeed;
    // Player hit force
    [SerializeField] private float _playerHitForce;
    // Player bullet
    [SerializeField] protected GameObject _playerProjectile;
    // Is player hit
    protected bool _isPlayerHit;
    // Player hit cooldown time
    [SerializeField] protected int _afterHitCooldownTime;
    // Sprite health 
    [SerializeField] protected Image[] health;
    // Sprite full heart 
    [SerializeField] protected Sprite fullHealth;
    // Sprite empty heart 
    [SerializeField] protected Sprite emptyHealth;
    // heel 
    //[protected int heel;] 

    #endregion

    #region Unity

    // On player enable
    protected void OnEnable()
    {
        // Setting player hit condition to false
        _isPlayerHit = false;
        // Setting player health back to maximum value
        _playerRecentHealth = _playerHealthMaxCapacity;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // This is temporary method of checking if game is on pause.
        // Might be better to make it event based.
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (GameManager.GetInstance().IsOnPause == false)
        {
            base.Update();
            // Moving
            Movement();
            // Shooting
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootBullet();
            }
        }
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

    // Object reaction to being hit
    protected override void Hit(Collision2D collision)
    {
        // Player pushed away after hit
        _objectRigidbody.AddForce(_objectRigidbody.velocity * _playerHitForce * _playerHitForce, ForceMode2D.Force);
        // Checking if player was hit or not
        if (_isPlayerHit == false)
        {
            // Was player hit
            _isPlayerHit = true;
            // Decrease player health
            _playerRecentHealth--;
            // Invoke method
            Invoke(nameof(HitCooldown), _afterHitCooldownTime);
            // Logging damage
            Debug.Log("Player health after hit: " + _playerRecentHealth);
        }
    }
     
    // After player was hit there is time gap, where he can't be hit again
    private void HitCooldown()
    {
        // Disabling player hit condition
        _isPlayerHit = false;
    }

    // Player fire
    private void ShootBullet()
    {
        // Setting bullet position and its rotation
        Vector3 bulletPos = transform.position + transform.up;
        Quaternion bulletRotate = transform.rotation;
        // Requesting an object from spawn manager
        GameObject bulletInstance = SpawnManager.GetInstance().SpawnObject(SpawnManager.PoolType.PlayerBullets, _playerProjectile);
        // Setting its position and rotation
        bulletInstance.transform.position = bulletPos;
        bulletInstance.transform.rotation = bulletRotate;
    } 
    //
    private void FixedUpdate()
    {
        if (_playerRecentHealth > _playerHealthMaxCapacity)
        {
            _playerRecentHealth = _playerHealthMaxCapacity;
        }
        //Test heel [_playerRecentHealth *= Time.deltaTime * heel;]
        //When heart full or not
        for(int i=0;i<health.Length;i++)
        {
            if(i<Mathf.RoundToInt(_playerRecentHealth))
            {
                health[i].sprite = fullHealth;
            }
            else
            {
                health[i].sprite = emptyHealth;
            }
            if (i < _playerHealthMaxCapacity)
            {
                health[i].enabled = true;
            }
            else
            {
                health[i].enabled = false;
            }
            if (_playerRecentHealth < 1)
            {
                Time.timeScale = 0;
            }
        }
    }
    #endregion
}
