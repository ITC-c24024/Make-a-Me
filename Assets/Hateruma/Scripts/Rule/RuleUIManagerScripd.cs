using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleUIManagerScripd : UIManagerScript
{

    [SerializeField, Header("TopicÇÃImage")]
    Image[] topicImage;

    [SerializeField, Header("TopicÇÃê‡ñæï∂")]
    Image[] sentenceImage;

    [SerializeField, Header("TopicÇÃê‡ñæìÆâÊ")]
    Image[] movieImage;

    int selectNum;
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 stickMove = selectAction.ReadValue<Vector2>();

        if (stickMove.y > 0.2f && !isCoolTime)
        {
            if (selectNum > 0)
            {
                selectNum--;
            }
            else if (selectNum == 0)
            {
                selectNum = topicImage.Length - 1;
            }
            StartCoroutine(SelectCoolTime());
        }
        if (stickMove.y < -0.2f && !isCoolTime)
        {
            if (selectNum < topicImage.Length - 1)
            {
                selectNum++;
            }
            else if (selectNum == topicImage.Length - 1)
            {
                selectNum = 0;
            }
            StartCoroutine(SelectCoolTime());
        }
    }
}
