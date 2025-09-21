using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUIScript : UIManagerScript
{

    [SerializeField, Header("�^�C�g���̃{�^��Image")]
    Image[] buttonImage;

    [SerializeField]
    ShutterScript shutterScript;

    int selectNum;

    [SerializeField]
    AudioManager audioManager;
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 stickMove = selectAction.ReadValue<Vector2>();

        if (!isCoolTime)
        {
            if (stickMove.y > 0.2f) ChangeSelect(-1); // ��
            if (stickMove.y < -0.2f) ChangeSelect(1); // ��
        }

        if (decisionAction.triggered && selectNum == buttonImage.Length)
        {
            audioManager.Dicide();
            gameObject.SetActive(false);
        }
    }

    void ChangeSelect(int direction)
    {
        audioManager.Select();

        // ���݂̑I����OFF
        if (selectNum < buttonImage.Length)
        {
            buttonImage[selectNum].enabled = false;
        }

        // �ړ�
        selectNum += direction;

        if (selectNum < 0) selectNum = buttonImage.Length - 1;
        if (selectNum > buttonImage.Length - 1) selectNum = 0;

        // �V�����I����ON
        if (selectNum < buttonImage.Length)
        {
            buttonImage[selectNum].enabled = true;
            StartCoroutine(BounceUI(buttonImage[selectNum].transform, 0.3f));
        }

        StartCoroutine(SelectCoolTime());
    }

    IEnumerator BounceUI(Transform target, float time)
    {
        float t = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (t < 1f)
        {
            t += Time.deltaTime / time;

            float s = 2f;
            float curved = 1f + s * Mathf.Pow(t - 1f, 3) + s * Mathf.Pow(t - 1f, 2);

            target.localScale = Vector3.LerpUnclamped(startScale, endScale, curved);
            yield return null;
        }

        target.localScale = Vector3.one;
    }

    void Decision()
    {
        audioManager.Dicide();

        switch (selectNum)
        {
            case 0:
                audioManager.TitleStop();
                StartCoroutine(shutterScript.CloseShutter());
                Invoke(nameof(MainScene), 2.5f);
                break;

            case 1:
                StartCoroutine(shutterScript.CloseShutter());
                Invoke(nameof(TitleScene), 2.5f);
                break;
        }
    }

    void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    void TitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
