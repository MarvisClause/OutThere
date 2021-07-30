using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for holding global variables
public static class Globals
{
    #region Tags

    // Player tag
    public const string PLAYER_TAG = "Player";
    // Enemy tag
    public const string ENEMY_TAG = "Enemy";

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
            throw new System.Exception();
        }
        return _instance;
    }

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
