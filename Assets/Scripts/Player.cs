using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseActiveObject
{
    #region Variables

    [Header("Player characteristics")]
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
    
    [Header("Player bullet")]
    // Player bullet
    [SerializeField] private GameObject _playerProjectile;
    // Is player hit
    protected bool _isPlayerHit;

    [Header("Player hit detection")]
    // Player hit cooldown time
    [SerializeField] private int _afterHitCooldownTime;
    // Sprite health 
    [SerializeField] private Image[] health;
    // Sprite full heart 
    [SerializeField] private Sprite fullHealth;
    // Sprite empty heart 
    [SerializeField] private Sprite emptyHealth;
    // heal 
    //[protected int heal;] 

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
        if (GameManager.GetInstance().IsOnPause == false && _playerRecentHealth > 0)
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
            // Play hit sound
            SoundManager.GetInstance().PlaySound(Globals.PLAYER_HIT_SOUND);
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
