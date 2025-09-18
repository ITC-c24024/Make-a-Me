using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    GameController gameController;

    [SerializeField,Header("�^�C�}�[UI�̐e")]
    GameObject timerObj;
    [SerializeField, Header("���S�I�u�W�F�N�g")]
    GameObject center;
    [SerializeField, Header("���ԊO��")]
    Image gearOut;
    [SerializeField, Header("���ԓ���")]
    Image gearIn;
    [SerializeField, Header("�o�ߎ��ԃX���C�_�[")]
    Slider slider;

    [SerializeField, Header("�Q�[������(�b)")]
    float limitTime = 180;

    bool isNotice = false;

    void Start()
    {
        gameController = GetComponent<GameController>();
    }

    /// <summary>
    /// �X���C�_�[����
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        float currentTime = 0;
        while (currentTime < limitTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > limitTime * 1 / 6 && !isNotice)
            {
                isNotice = true;
                StartCoroutine(Notice());
                gameController.Notice();
            }

            gearOut.rectTransform.localEulerAngles += new Vector3(0, 0, -1);
            gearIn.rectTransform.localEulerAngles += new Vector3(0, 0, 4);

            //�j����]
            float rotationZ = Mathf.Lerp(0, -360, currentTime / limitTime);
            center.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            //�o�ߎ��ԃX���C�_�[��i�߂�
            float value= Mathf.Lerp(0, 1, currentTime / limitTime);
            slider.value = value;

            yield return null;
        }
        isNotice = false;
        StartCoroutine(gameController.GameFinish());
    }
    /// <summary>
    /// �^�C�}�[UI����������񂳂���
    /// </summary>
    /// <returns></returns>
    IEnumerator Notice()
    {
        Vector3 startScale = timerObj.transform.localScale;
        float changeT = 0.5f;
        while (isNotice)
        {
            float time = 0;
            while (time < changeT)
            {
                time += Time.deltaTime;

                float rate = Mathf.Lerp(0, 3.14f, time / changeT);
                Vector3 currentScale = startScale * (1 + Mathf.Abs(Mathf.Sin(rate)) * 0.2f);
                timerObj.transform.localScale = currentScale;

                yield return null;
            }
        }  
    }
}
