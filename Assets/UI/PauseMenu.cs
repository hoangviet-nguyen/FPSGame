using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused;
    public GameObject pauseMenuUI;
    public WeaponController weaponController;
    public Slider mouseSensitivitySlider;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        weaponController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponentInChildren<WeaponController>();
        if (GameValues.mousesensitivity == 0) GameValues.mousesensitivity = 15;
        mouseSensitivitySlider.value = GameValues.mousesensitivity;
        Resume();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void Resume()
    {
        GameValues.mousesensitivity = (int) mouseSensitivitySlider.value;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        weaponController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Pause()
    {   
        mouseSensitivitySlider.value = GameValues.mousesensitivity;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        weaponController.enabled = false;
        
    }
    
    private void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
    
    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }



}
