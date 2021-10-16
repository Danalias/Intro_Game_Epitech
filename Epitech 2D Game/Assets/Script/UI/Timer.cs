using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{

    public static float globalTime = 0;
    public static float levelTime = 0;
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update() {
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            Destroy(gameObject);
        }
        globalTime += Time.deltaTime;
        levelTime += Time.deltaTime;
    }
}
