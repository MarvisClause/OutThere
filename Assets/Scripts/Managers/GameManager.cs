using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Class for holding global variables
public static class Globals
{
    #region Tags

    // Player tag
    public const string PLAYER_TAG = "Player";
    // Enemy tag
    public const string ENEMY_TAG = "Enemy";
    // Player bullet tag
    public const string PLAYER_BULLET_TAG = "PlayerBullet";
    // Player bullet tag
    public const string ENEMY_BULLET_TAG = "EnemyBullet";

    #endregion

    #region SoundNames

    // Enemy is hit sound
    public const string ENEMY_HIT_SOUND = "EnemyHit";
    // Player is hit sound
    public const string PLAYER_HIT_SOUND = "PlayerHit";
    // Player fire sound
    public const string PLAYER_FIRE = "PlayerFire";
    // Background music sound
    public const string BACKGROUND_MUSIC = "BackgroundMusic";
    // Enemy ship reload sound
    public const string ENEMY_SHIP_FIRE = "EnemyShipFire";

    #endregion
}

public class GameManager : MonoBehaviour
{
    #region Variables

    //Singleton
    private static GameManager _instance;
    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            Debug.LogWarning("Instance of game manager is null referenced!");
            throw new System.Exception("Instance of game manager is null referenced!");
        }
        return _instance;
    }

    // Is game on pause
    private bool _isOnPause;
    public bool IsOnPause
    {
        get { return _isOnPause; }
    }

    // Player object
    [SerializeField] protected GameObject _player;
    [SerializeField] protected GameObject _gameOverScreen;
    [SerializeField] protected GameObject _hud;
    [SerializeField] protected Text _scoreResGameOver;
    [SerializeField] protected Text _timeResGameOver;

    // Player username
    protected string _username;
    public string Username { get { return _username; } set { _username = value; } }

    //Time
    protected TimeRes _timeResult;
    [SerializeField] Text _gameTime;
    public TimeRes TimeResult { get { return _timeResult; } }

    // Player script
    private Player _playerScript;
    public bool IsPlayerActive { get { return _player.activeSelf; } }
    public bool IsPlayerInvincible { get { return _playerScript.IsPlayerHit; } }

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        // Get player script
        _playerScript = _player.GetComponent<Player>();
        // Play background music in loop
        SoundManager.GetInstance().PlaySound(Globals.BACKGROUND_MUSIC, true);
    }

    private void FixedUpdate()
    {
        // Check, if player is here
        if (_player.activeSelf == true)
        {
            // Timer work
            Timer(); 
        }
    }
    #endregion

    #region Methods

    // Start the game
    public void StartGame()
    {
        // Disable all objects on screen
        SpawnManager.GetInstance().DisableAllObjects();
        // Activating player
        _player.SetActive(true);
        // Set player rotation to upwards
        _player.transform.rotation = new Quaternion(0, 0, 0, 0);
        // Setting his position to vector zero
        _player.transform.position = Vector3.zero;
        // Setting time scale to one in case of its being frozen
        Time.timeScale = 1.0f;
        // Setting pause state to false
        _isOnPause = false;
    }

    // Pauses game
    public bool PauseGame()
    {
        if (_player.activeSelf == true)
        {
            // Stopping time
            Time.timeScale = 0;
            // Setting pause state to false
            _isOnPause = true;
            
            return true;
        }
        return false;
    }

    // Unpause game
    public void PauseGameOff()
    {
        _isOnPause = false;
        _player.SetActive(true);
        Time.timeScale = 1.0f;
    }

    // Disables everything on scene to return it to the state of main menu
    public void BackToMainMenu()
    {
        _player.SetActive(false);
        SpawnManager.GetInstance().DisableAllObjects();
        Time.timeScale = 1.0f; 
        //Reset time
        _timeResult.ResetTime();
    } 

    // Game over
    public void GameOver()  
        
    {   // Disable all objects on screen
        SpawnManager.GetInstance().DisableAllObjects();
        // Deactivating player
        _player.SetActive(false);
        // Deactivating HUD 
        _hud.SetActive(false);
        // Activating Game over screen  
        _gameOverScreen.SetActive(true);
        //Add score and time to the game over screen   
        _scoreResGameOver.text = "Score : " + ScoreManager.GetInstance().PlayerScore.ToString();
        _timeResGameOver.text = "Time : " + _gameTime.text;
        
    } 

    // Timer work
    public void Timer()
    {
        _timeResult.seconds += 1 * Time.deltaTime;
        if (_timeResult.seconds >= 60)
        {
           _timeResult.minutes++;
           _timeResult.seconds = 0;
        } 
        //Print time
        _gameTime.text = _timeResult.ConvertToText();
    } 

    #endregion
}
