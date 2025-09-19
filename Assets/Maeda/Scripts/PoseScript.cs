using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class PoseScript : MonoBehaviour
{
    [SerializeField]
    GameController gameController;
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    ShutterScript shutterScript;
    [SerializeField, Header("�|�[�Y���")]
    GameObject poseImage;

    [SerializeField, Header("�|�[�YUI")]
    GameObject[] poseUI;


    //UI�؂�ւ��ϐ�
    private int uiNum = 0;


    float maxScale = 1.1f;

    float maxTime = 1f;


    //�|�[�Y��ʃA�N�V����
    private InputAction stickAction, decideAction, poseAction;

    private AnimationCurve animationCurve;

    private Coroutine currentCoroutine;

    void Start()
    {
        //�v���C���[��ActionMap���擾
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;

        stickAction = actionMap["Move"];
        decideAction = actionMap["Throw"];
        poseAction = input.actions["Pose"];

        animationCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f));

        poseUI[uiNum].SetActive(true);
        //StartAnimationForScene();
    }

    void Update()
    {
        //�|�[�Y��ʂɈړ�
        var poseAct = poseAction.triggered;

        if (poseAct && gameController.isOpen)
        {
            audioManager.Dicide();

            if (Time.timeScale == 1)
            {
                //�����Ȃ�����
                Time.timeScale = 0;
                poseImage.SetActive(true);
            }
            else
            {
                poseImage.SetActive(false);
                Time.timeScale = 1;
            }
            
        }


        //����
        var selectAct = decideAction.triggered;

        if (selectAct && Time.timeScale == 0)
        {
            audioManager.Dicide();
            switch (uiNum)
            {
                case 0:
                    Invoke("DeletePanel", 0.3f);

                    //������悤�ɂ���
                    Time.timeScale = 1;
                    break;
                case 1:
                    //������悤�ɂ���
                    Time.timeScale = 1;
                    StartCoroutine(shutterScript.CloseShutter());
                    Invoke("SelectTitle", 2.5f);
                    break;
            }
        }


        //UI�؂�ւ�
        var stickAct = stickAction.ReadValue<Vector2>().y;

        if (stickAct > 0 && Time.timeScale == 0 && uiNum != 0)
        {
            poseUI[uiNum].SetActive(false);

            uiNum = 0;
            audioManager.Select();

            StartCoroutine(BounceUI(poseUI[uiNum].transform, 0.3f));

            poseUI[uiNum].SetActive(true);
        }

        if (stickAct < 0 && Time.timeScale == 0 && uiNum != 1)
        {
            poseUI[uiNum].SetActive(false);

            uiNum = 1;
            audioManager.Select();

            StartCoroutine(BounceUI(poseUI[uiNum].transform, 0.3f));

            poseUI[uiNum].SetActive(true);
        }
    }

    private void DeletePanel()
    {
        poseImage.SetActive(false);
    }

    private void SelectTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator BounceUI(Transform target, float time)
    {
        float t = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / time;
            float s = 2f;
            float curved = 1f + s * Mathf.Pow(t - 1f, 3) + s * Mathf.Pow(t - 1f, 2);
            target.localScale = Vector3.LerpUnclamped(startScale, endScale, curved);
            yield return null;
        }
        target.localScale = Vector3.one;
    }
}
