using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Endscreen : MonoBehaviour
{
    public  GameObject EndscreenUI;
    



    public void Endgame()
    {
        EndscreenUI.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


}