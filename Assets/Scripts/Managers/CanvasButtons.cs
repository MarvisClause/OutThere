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
    // Input menu 
    [SerializeField] protected GameObject _inputWindow;
    // HUD 
    [SerializeField] protected GameObject _hud; 
    // Gameover
    [SerializeField] protected GameObject _gameover;

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
        _hud.SetActive(true);
    }
    // Pauses game
    public void PauseGame()
    {
        if (GameManager.GetInstance().PauseGame() == true)
        {
            _pauseMenu.SetActive(true);
            _hud.SetActive(false);
        }
    }
    // For pause game 
    public void PauseGameOff()
    {
        _pauseMenu.SetActive(false);
        GameManager.GetInstance().PauseGameOff();
        _hud.SetActive(true);
    }

    // Returns scene state to main menu
    public void BackToMainMenu()
    {
        _pauseMenu.SetActive(false);
        _mainMenu.SetActive(true);
        _hud.SetActive(false);
        _gameover.SetActive(false);
        GameManager.GetInstance().BackToMainMenu(); 

    }
    // Read input text
    public void ReadName(string inputName)
    {
        GameManager.GetInstance().Username = string.Copy(inputName); 
    }
    public void Submit()
    {
        //Save name and score in High Score table
        HighscoreTable._instance.AddHighscoreEntry(ScoreManager.GetInstance().PlayerScore,GameManager.GetInstance().TimeResult, GameManager.GetInstance().Username);
        HighscoreTable._instance.UpdateHighscoreTable();
    }
    #endregion
}
