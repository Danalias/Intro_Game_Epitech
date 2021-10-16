using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public static bool isWin = false;

    public Text secondsTxt;

    [SerializeField] List<GameObject> enemies;
    [SerializeField] GameObject victoryMenu;
    [SerializeField] string nextStage;

    void Update() {
        //enemies.RemoveAll(enemy => enemy == null);
        if (isWin && PauseMenu.isGamePaused == false) {
            PauseMenu.isGamePaused = true;
            writeTime();
            victoryMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void loadNextStage() {
        Time.timeScale = 1f;
        isWin = false;
        PauseMenu.isGamePaused = false;
        SceneManager.LoadScene(this.nextStage);
    }

    void writeTime() {
        secondsTxt.text = ((int)Timer.levelTime).ToString() + " seconds";
        Timer.levelTime = 0;
    }

    void writeFinalTime() {
        secondsTxt.text = ((int)Timer.globalTime).ToString() + " seconds";
    }
}
