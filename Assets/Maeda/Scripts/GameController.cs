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

    [SerializeField]
    GameObject finishImage;

    float scaleChangeTime = 1f;
    float startScaleChageTime = 0.3f;

    Vector3 originalScale;
    Vector3 targetScale;

    public bool isOpen = false;
    public bool isStart = false;
    public bool isFinish = false;

    void Start()
    {
        Application.targetFrameRate = 60;

        countDownScript = GetComponent<CountDownScript>();
        timerScript = this.GetComponent<TimerScript>();

        targetScale = new Vector3(1, 1, 1);

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
        StartCoroutine(FinishScaleUp());
        yield return new WaitForSeconds(0.3f);

        isFinish = true;
        audioManager.MainStop();
        StartCoroutine(shutterScript.CloseShutter());

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("ResultScene");
    }

    IEnumerator FinishScaleUp()
    {
        finishImage.SetActive(true);
        float timer = 0f;
        while (timer < startScaleChageTime)
        {
            timer += Time.deltaTime;
            float scaleChangeTime = timer / startScaleChageTime;
            finishImage.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleChangeTime);

            yield return null;
        }
        finishImage.transform.localScale = targetScale;//�X�P�[����ۑ�

        yield return new WaitForSeconds(0.5f);//0.5�b�҂��ĉ��̏��������s
    }
}
