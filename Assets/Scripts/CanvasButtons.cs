using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CanvasButtons : MonoBehaviour
{
    #region Variables   
    [SerializeField] protected GameObject _pauseMenu;
    [SerializeField] protected GameObject _player;
    [SerializeField] protected GameObject _mainMenu;
    [SerializeField] protected AudioMixer _audioMixer;
    //public static bool GameIsPaused = false;
    #endregion
    
    #region Unity  
    public void Start()
    {
        _pauseMenu.SetActive(false);
    }
    private void Update()
    {
        if (_player.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _pauseMenu.SetActive(true);
                _player.SetActive(false);
            }

        }
    }
    
    #endregion 

    #region Methods
    //Button for Quit
    public void Quit()
    {   //quit the game
        Application.Quit();
    } 
    
    public void StartTheGame()
    {
        _mainMenu.SetActive(false);
        _player.SetActive(true);

    } 
    //For audio slider in options
    public void MusicVolume(float volume) 
    {
        _audioMixer.SetFloat("volume",volume);
    } 
    //For pause game 
    public void PauseGameOff()
    {
        _pauseMenu.SetActive(false);
        _player.SetActive(true);
    }
    #endregion

}
