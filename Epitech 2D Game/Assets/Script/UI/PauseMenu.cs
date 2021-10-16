using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField] GameObject deathMenu = null;

    float currentTime = 1f;
    
    public GameObject PauseMenuUI = null;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PauseMenuUI != null) {
            Debug.Log("pause ?");
            if (isGamePaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        PauseMenuUI.SetActive(false);
        Time.timeScale = currentTime;
        isGamePaused = false;
    }

    void Pause() {
        currentTime = Time.timeScale;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ActivateDeathMenu() {
        isGamePaused = true;
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
