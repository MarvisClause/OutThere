using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] protected int score;
    [SerializeField] protected Text scoreDisplay;
    // Player score 
    [SerializeField] protected GameObject _mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        scoreDisplay.text = "Score : " + score.ToString();
        
    if (_mainMenu.activeSelf == true)
            {
                score = 0;
            
            }
           
    } 
    public void AddToScore()
    {
        score++;
    }  
    
}
