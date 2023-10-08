using UnityEngine;
using UnityEngine.SceneManagement;

public class Endscreen : MonoBehaviour
{
    public  GameObject EndscreenUI;
    public GameObject WinScreenUI;
    public WeaponController weaponController;

    private void Start()
    {
        weaponController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponController>();
    }
    
    public void Endgame()
    {

        Cursor.lockState = CursorLockMode.None;
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
    
    public void EndgameWin()
    {
        Cursor.lockState = CursorLockMode.None;
        WinScreenUI.SetActive(true);
        Time.timeScale = 0f;
        weaponController.enabled = false;
    }
}