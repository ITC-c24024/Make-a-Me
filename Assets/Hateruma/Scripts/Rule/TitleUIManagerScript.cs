using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManagerScript : UIManagerScript
{
    [SerializeField, Header("タイトルロゴ")]
    Image titleImage;

    [SerializeField, Header("タイトルのボタンImage")]
    Image[] buttonImage;

    [SerializeField, Header("ルール画面")]
    GameObject rulePanelObj;

    [SerializeField, Header("操作方法画面")]
    GameObject controllPanelObj;

    [SerializeField, Header("操作方法画面の戻るUI")]
    Image returnControllPanel;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    ShutterScript shutterScript;

    int selectNum;

    bool canSelect;

    bool isTitle = true;

    void Start()
    {
        canSelect = true;
    }

    void Update()
    {
        Vector2 stickMove = selectAction.ReadValue<Vector2>();

        if (isTitle && decisionAction.triggered)
        {
            StartCoroutine(HideTitle());
        }

        if (!isCoolTime && canSelect && !isTitle)
        {
            if (stickMove.y > 0.2f) ChangeSelect(-1); // 上
            if (stickMove.y < -0.2f) ChangeSelect(1); // 下
            if (decisionAction.triggered) Decision();

        }
        else if (!isCoolTime && !canSelect && !isTitle)
        {
            if (stickMove.y > 0.2f || stickMove.y < -0.2f)
            {
                returnControllPanel.enabled = true;
                StartCoroutine(BounceUI(returnControllPanel.transform, 0.3f));
                StartCoroutine(SelectCoolTime());
            }
            if (decisionAction.triggered)
            {
                controllPanelObj.SetActive(false);
                canSelect = true;
            }

        }
    }

    void ChangeSelect(int direction)
    {
        // 現在の選択をOFF
        if (selectNum < buttonImage.Length)
        {
            buttonImage[selectNum].enabled = false;
        }

        // 移動
        selectNum += direction;

        if (selectNum < 0) selectNum = buttonImage.Length - 1;
        if (selectNum > buttonImage.Length - 1) selectNum = 0;

        // 新しい選択をON
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
        switch (selectNum)
        {
            case 0:
                audioManager.TitleStop();
                StartCoroutine(shutterScript.CloseShutter());
                Invoke(nameof(MainScene), 2.5f);
                canSelect = false;
                break;

            case 1:
                rulePanelObj.SetActive(true);
                canSelect = false;
                break;

            case 2:
                controllPanelObj.SetActive(true);
                canSelect = false;
                break;

            case 3:
                Application.Quit();
                break;
        }
    }

    public void SelectOK()
    {
        canSelect = true;
    }

    void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator HideTitle()
    {
        float count = 0f;
        Color startColor = titleImage.color;

        while (count < 1)
        {
            count += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, count / 1);
            titleImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // 完全に透明にする
        titleImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        
    }
}
