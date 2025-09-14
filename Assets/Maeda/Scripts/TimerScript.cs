using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    GameController gameController;
    
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

        gameController.GameFinish();
    }
}
