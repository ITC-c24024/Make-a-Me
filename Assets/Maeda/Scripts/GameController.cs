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

        Invoke("Open", 1.0f);
    }

    void Open()
    {
        StartCoroutine(shutterScript.OpenShutter());
    }
    /// <summary>
    /// タイマーを開始し、動けるようにする
    /// </summary>
    public void GameStart()
    {        
        StartCoroutine(timerScript.Timer());
        isStart = true;
        audioManager.Main();
    }
    /// <summary>
    /// BGMを加速
    /// </summary>
    public void Notice()
    {
        audioManager.MainStop();
        audioManager.MainSpeedUp();
        audioManager.Main();
    }
    /// <summary>
    /// BGMを止め、シャッターを閉じる
    /// </summary>
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
