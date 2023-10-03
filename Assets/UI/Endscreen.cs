using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Endscreen : MonoBehaviour
{
    public  GameObject EndscreenUI;
    public WeaponController weaponController;

    private void Start()
    {
        weaponController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponController>();
    }


    public void Endgame()
    {
        EndscreenUI.SetActive(true);
        Time.timeScale = 0f;
        weaponController.enabled = false;
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