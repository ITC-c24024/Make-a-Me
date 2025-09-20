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

    public bool isOpen = false;
    public bool isStart = false;
    public bool isFinish = false;

    void Start()
    {
        Application.targetFrameRate = 60;

        countDownScript = GetComponent<CountDownScript>();
        timerScript = this.GetComponent<TimerScript>();

        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(shutterScript.OpenShutter());

        yield return new WaitForSeconds(2.0f);
        isOpen = true;
    }
    /// <summary>
    /// �^�C�}�[���J�n���A������悤�ɂ���
    /// </summary>
    public IEnumerator GameStart()
    {
        countDownScript.enabled = true;
        yield return new WaitForSeconds(3.5f);

        StartCoroutine(timerScript.Timer());
        isStart = true;
        audioManager.Main();
    }
    /// <summary>
    /// �x������炵�ABGM������
    /// </summary>
    public IEnumerator Notice()
    {
        audioManager.MainStop();

        audioManager.Warning();
        yield return new WaitForSeconds(3.0f);

        audioManager.MainSpeedUp();
        audioManager.Main();
    }
    /// <summary>
    /// BGM���~�߁A�V���b�^�[�����
    /// </summary>
    public IEnumerator GameFinish()
    {
        isFinish = true;
        audioManager.MainStop();
        StartCoroutine(shutterScript.CloseShutter());

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("ResultScene");
    }
}
