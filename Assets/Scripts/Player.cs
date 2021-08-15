using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseActiveObject
{
    #region Variables

    // Animator  
    private Animator _playerAnimator;
    // Animator variable
    private string STATE_ANIMATION_VARIABLE_NAME = "PlayerState";
    // Player states
    private enum PlayerState
    {
        Idle,
        Moving,
        IsHit
    }
    // Player state
    private PlayerState _playerState;

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
    private bool _isPlayerHit;
    public bool IsPlayerHit { get { return _isPlayerHit; } }

    [Header("Player hit detection")]
    // Player hit cooldown time
    [SerializeField] private int _afterHitCooldownTime;
    // Sprite health 
    [SerializeField] private Image[] health;
    // Sprite full heart 
    [SerializeField] private Sprite fullHealth;
    // Sprite empty heart 
    [SerializeField] private Sprite emptyHealth;

    // Player input
    private float _horizontalInput;
    private float _verticalInput;

    #endregion

    #region Unity

    // On player enable
    protected void OnEnable()
    {
        // Getting player animator
        _playerAnimator = GetComponent<Animator>();
        // Setting player state
        _playerState = 0;
        // Setting player hit condition to false
        _isPlayerHit = false;
        // Setting player health back to maximum value
        _playerRecentHealth = _playerHealthMaxCapacity;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.GetInstance().IsOnPause == false && _playerRecentHealth > 0)
        {
            base.Update();
            // Moving
            Movement();
            // Shooting
            if (!IsPlayerHit)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Fire sound
                    SoundManager.GetInstance().PlaySound(Globals.PLAYER_FIRE);
                    ShootBullet();
                }
            }
            //Animation  
            AnimationStateCheck();
        }
    }

    // Fixed update
    private void FixedUpdate()
    {
        // Moving
        _objectRigidbody.AddForce(_verticalInput * transform.up.normalized * _playerMovementSpeed, ForceMode2D.Force);

        // Heath system
        if (_playerRecentHealth > _playerHealthMaxCapacity)
        {
            _playerRecentHealth = _playerHealthMaxCapacity;
        }
        // When heart full or not
        for (int i = 0; i < health.Length; i++)
        {
            if (i < Mathf.RoundToInt(_playerRecentHealth))
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
                GameManager.GetInstance().GameOver();
            }
        }
    }

    #endregion

    #region Methods

    // Player movement
    private void Movement()
    {
        // Horizontal and vertical input
        _horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime;
        _verticalInput = Input.GetAxis("Vertical");
        // Rotating
        transform.Rotate(0.0f, 0.0f, -_horizontalInput * _playerRotationSpeed);
        // Animation
        if (!_isPlayerHit)
        {
            if (Mathf.Abs(_horizontalInput + _verticalInput) > 0)
            {
                _playerState = PlayerState.Moving;
            }
            else
            {
                _playerState = PlayerState.Idle;
            }
        }
    }

    public void AnimationStateCheck()
    {
        if (_playerAnimator)
        {
            // Setting animation state
            _playerAnimator.SetInteger(STATE_ANIMATION_VARIABLE_NAME, (int)_playerState);
        }
    }

    // Object reaction to being hit
    protected override void Hit(Collision2D collision)
    {
        // If it is not player bullet
        if (!collision.gameObject.CompareTag(Globals.PLAYER_BULLET_TAG))
        {
            // Player pushed away after hit
            _objectRigidbody.AddForce(_objectRigidbody.velocity * _playerHitForce * _playerHitForce, ForceMode2D.Force);
            // Checking if player was hit or not
            if (_isPlayerHit == false)
            {
                // Setting animation state
                _playerState = PlayerState.IsHit;
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
    }

    // After player was hit there is time gap, where he can't be hit again
    private void HitCooldown()
    {
        // Disabling player hit condition
        _isPlayerHit = false;
        // Setting player state to idle
        _playerState = PlayerState.Idle;
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

    #endregion
}