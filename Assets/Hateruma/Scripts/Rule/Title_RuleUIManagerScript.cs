using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_RuleUIManagerScript : UIManagerScript
{

    [SerializeField, Header("Topic��Image")]
    Image[] topicImage;

    [SerializeField, Header("Topic�̐�����")]
    Image[] sentenceImage;

    [SerializeField, Header("Topic�̐�������")]
    GameObject[] movieImage;

    [SerializeField, Header("Exit��Image")]
    Image exitImage;

    [SerializeField, Header("�^�C�g��UI�X�N���v�g")]
    TitleUIManagerScript titleUISC;

    //�I�𒆂�UI�̔ԍ�
    int selectNum;
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

        if (decisionAction.triggered && selectNum == topicImage.Length)
        {
            titleUISC.SelectOK();
            gameObject.SetActive(false);
        }
    }

    void ChangeSelect(int direction)
    {
        // ���݂̑I����OFF
        if (selectNum < topicImage.Length)
        {
            topicImage[selectNum].enabled = false;
            sentenceImage[selectNum].enabled = false;
            movieImage[selectNum].SetActive(false);
        }
        else
        {
            exitImage.enabled = false;
        }

        // �ړ�
        selectNum += direction;

        if (selectNum < 0) selectNum = topicImage.Length;            
        if (selectNum > topicImage.Length) selectNum = 0;           

        // �V�����I����ON
        if (selectNum < topicImage.Length)
        {
            topicImage[selectNum].enabled = true;
            sentenceImage[selectNum].enabled = true;
            movieImage[selectNum].SetActive(true);
        }
        else
        {
            exitImage.enabled = true;
        }

        StartCoroutine(SelectCoolTime());
    }

}
