using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    // Player score
    public int PlayerScore
    {
        get
        {
            return _playerScore;
        }
        set
        {
            if (value > 0)
            {
                _playerScore = value;
            }
        }
    }
    private int _playerScore;

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        PlayerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods

    // Start the game
    public void StartGame()
    {
        // Activating player
        _player.SetActive(true);
        // Set player rotation to upwards
        _player.transform.rotation = new Quaternion(0, 0, 0, 0);
        // Setting his position to vector zero
        _player.transform.position = Vector3.zero;
        // Setting player score to zero
        PlayerScore = 0;
        // Setting time scale to one in case of its being frozen
        Time.timeScale = 1.0f;
        // Setting pause state to false
        _isOnPause = false;
    }

    // For audio slider in options
    public void MusicVolume(float volume)
    {
        // Changing volume of audiosource
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

    #endregion
}
