using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CanvasButtons : MonoBehaviour
{
    #region Variables   

    // Pause menu
    [SerializeField] protected GameObject _pauseMenu;
    // Main menu
    [SerializeField] protected GameObject _mainMenu;

    #endregion
    
    #region Unity  

    public void Start()
    {
        _pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    
    #endregion 

    #region Methods

    //Button for Quit
    public void Quit()
    {  
        // Quit the game
        Application.Quit();
    } 
    
    // Starts the game
    public void StartTheGame()
    {
        _mainMenu.SetActive(false);
        GameManager.GetInstance().StartGame();
    }
    
    // For audio slider in options
    public void MusicVolume(float volume) 
    {
        GameManager.GetInstance().MusicVolume(volume);
    } 

    // Pauses game
    public void PauseGame()
    {
        if (GameManager.GetInstance().PauseGame() == true)
        {
            _pauseMenu.SetActive(true);
        }
    }

    // For pause game 
    public void PauseGameOff()
    {
        _pauseMenu.SetActive(false);
        GameManager.GetInstance().PauseGameOff();
    }

    #endregion
}
