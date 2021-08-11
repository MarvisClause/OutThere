using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region Variables

    //Singleton
    private static ScoreManager _instance;
    public static ScoreManager GetInstance()
    {
        if (_instance == null)
        {
            Debug.LogWarning("Instance of score manager is null referenced!");
            throw new System.Exception("Instance of score manager is null referenced!");
        }
        return _instance;
    }

    // Player score
    private int _playerScore;
    public int PlayerScore { get { return _playerScore; } }
    // Interface
    [SerializeField] protected Text _scoreDisplay;
    [SerializeField] protected GameObject _mainMenu;

    #endregion

    #region Unity

    private void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        _scoreDisplay.text = "Score : " + _playerScore.ToString();
        if (_mainMenu.activeSelf == true)
        {
            _playerScore = 0;
        }   
    }

    #endregion

    #region Methods

    // Add number to score
    public void AddToScore(int toAdd)
    {
        _playerScore += toAdd;
    }

    #endregion
}
