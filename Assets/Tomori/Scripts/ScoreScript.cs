using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    [SerializeField]
    EnergyScript energyScript;
    [SerializeField]
    PlayerController playerController;

    [SerializeField, Header("�X�R�AUI")]
    Image[] scoreImage;
    [SerializeField, Header("����UI")]
    Sprite[] numSprite;

    public float maxTime = 10f;
    [SerializeField] float workTime = 0f;
    //��ƌ���
    private float[] efficiency = new float[] { 1.0f, 1.5f, 2.5f, 4.0f, 6.0f };

    [SerializeField] int score = 0;
    [SerializeField] bool isWork = false;

    [SerializeField] int workAreaNum = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (isWork)
        {
            workTime += Time.deltaTime * efficiency[energyScript.level-1];
        }

        if(workTime >= maxTime)
        {
            workTime = 0;
            score += 1;
            SetUI();
        }
    }

    void SetUI()
    {
        int ten = score / 10; //�\�̈�
        int one = score - 10 * ten; //��̈�

        //�\�̈ʂ����鎞,�\�̈ʂ��\������Ă��Ȃ��Ƃ�
        if (ten > 0 && !scoreImage[1].enabled)
        {
            Debug.Log("hoge");
            //��̈ʂ��E�ɂ��炷
            scoreImage[0].rectTransform.anchoredPosition = new Vector2(
                scoreImage[0].rectTransform.anchoredPosition.x + 20,
                scoreImage[0].rectTransform.anchoredPosition.y
                );
            //�\�̈ʂ�\��
            scoreImage[1].enabled = true;
        }
        
        scoreImage[0].sprite = numSprite[one];
        scoreImage[1].sprite = numSprite[ten];
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isWork = true;
            playerController.Job(isWork);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isWork = false;
            playerController.Job(isWork);
        }
    }
}
