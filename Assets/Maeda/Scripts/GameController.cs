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
    /// �^�C�}�[���J�n���A������悤�ɂ���
    /// </summary>
    public void GameStart()
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
        audioManager.MainStop();
        StartCoroutine(shutterScript.CloseShutter());
        Invoke("ResultScene", 2.5f);
    }

    void ResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }
}
