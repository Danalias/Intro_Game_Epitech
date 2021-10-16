using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] string currentScene;

    public void retryButton() {
        Timer.levelTime = 0;
        Time.timeScale = 1f;
        PauseMenu.isGamePaused = false;
        SceneManager.LoadScene(currentScene);
    }
}
