using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;
    TimerScript timerScript;
    [SerializeField]
    ShutterScript shutterScript;

    public bool isStart = false;

    void Start()
    {
        Application.targetFrameRate = 60;

        timerScript = this.GetComponent<TimerScript>();

        Invoke("GameStart", 1.0f);
    }

    void GameStart()
    {
        StartCoroutine(shutterScript.OpenShutter());
        StartCoroutine(timerScript.Timer());
        Invoke("MainBGM", 2.5f);
    }

    void MainBGM()
    {
        isStart = true;
        audioManager.Main();
    }

    public void Notice()
    {
        audioManager.MainStop();
        audioManager.MainSpeedUp();
        MainBGM();
    }

    public void GameFinish()
    {
        audioManager.MainStop();
        StartCoroutine(shutterScript.CloseShutter());
        Invoke("ResultScene", 2.5f);
    }

    void ResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }
}
