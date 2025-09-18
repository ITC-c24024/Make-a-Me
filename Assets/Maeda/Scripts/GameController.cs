using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    CountDownScript countDownScript;
    [SerializeField]
    AudioManager audioManager;
    TimerScript timerScript;
    [SerializeField]
    ShutterScript shutterScript;

    public bool isStart = false;
    public bool isFinish = false;

    void Start()
    {
        Application.targetFrameRate = 60;

        countDownScript = GetComponent<CountDownScript>();
        timerScript = this.GetComponent<TimerScript>();
        //audioManager.Main();
        //isStart = true;
        //StartCoroutine(timerScript.Timer());
        Invoke("Open", 1.0f);
    }

    void Open()
    {
        StartCoroutine(shutterScript.OpenShutter());
    }

    public void Count()
    {
        countDownScript.enabled = true;
        Invoke("GameStart", 3.5f);
    }
    /// <summary>
    /// �^�C�}�[���J�n���A������悤�ɂ���
    /// </summary>
    void GameStart()
    {      
        StartCoroutine(timerScript.Timer());
        isStart = true;
        audioManager.Main();
    }
    /// <summary>
    /// BGM������
    /// </summary>
    public void Notice()
    {
        audioManager.MainStop();
        audioManager.MainSpeedUp();
        audioManager.Main();
    }
    /// <summary>
    /// BGM���~�߁A�V���b�^�[�����
    /// </summary>
    public void GameFinish()
    {
        isFinish = true;
        audioManager.MainStop();
        StartCoroutine(shutterScript.CloseShutter());

        Invoke("ResultScene", 2.5f);
    }

    void ResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }
}
