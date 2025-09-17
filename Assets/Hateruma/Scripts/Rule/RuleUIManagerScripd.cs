using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleUIManagerScripd : UIManagerScript
{

    [SerializeField, Header("Topic��Image")]
    Image[] topicImage;

    [SerializeField, Header("Topic�̐�����")]
    Image[] sentenceImage;

    [SerializeField, Header("Topic�̐�������")]
    Image[] movieImage;

    [SerializeField, Header("Exit��Image")]
    Image exitImage;

    //�I�𒆂�UI�̔ԍ�
    int selectNum;
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 stickMove = selectAction.ReadValue<Vector2>();

        if (stickMove.y > 0.2f && !isCoolTime)
        {
            if (selectNum == topicImage.Length)
            {
                exitImage.enabled = false;
            }
            else
            {
                topicImage[selectNum].enabled = false;
                sentenceImage[selectNum].enabled = false;
            }

            if (selectNum > 0)
            {
                selectNum--;
            }
            else if (selectNum == 0)
            {
                selectNum = topicImage.Length;
            }

            if(selectNum == topicImage.Length)
            {
                exitImage.enabled = true;
            }
            else
            {
                topicImage[selectNum].enabled = true;
                sentenceImage[selectNum].enabled = true;
            }

            StartCoroutine(SelectCoolTime());
        }
        if (stickMove.y < -0.2f && !isCoolTime)
        {
            if (selectNum == topicImage.Length)
            {
                exitImage.enabled = false;
            }
            else
            {
                topicImage[selectNum].enabled = false;
                sentenceImage[selectNum].enabled = false;
            }

            if (selectNum < topicImage.Length)
            {
                selectNum++;
            }
            else if (selectNum == topicImage.Length)
            {
                selectNum = 0;
            }

            if (selectNum == topicImage.Length)
            {
                exitImage.enabled = true;
            }
            else
            {
                topicImage[selectNum].enabled = true;
                sentenceImage[selectNum].enabled = true;
            }

            StartCoroutine(SelectCoolTime());
        }
    }
}
