using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CanvasButtons : MonoBehaviour
{
    public GameObject mainMenu;
    public AudioMixer audioMixer;
    
    //Button for Quit
    public void Quit()
    {   //quit the game
        Application.Quit();
    } 
    
    public void StartTheGame()
    {
        mainMenu.SetActive(false);
    } 
    //For audio slider in options
    public void MusicVolume(float volume) 
    {
        audioMixer.SetFloat("volume",volume);
    }
}
