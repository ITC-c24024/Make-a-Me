using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitScript : MonoBehaviour
{
    [SerializeField, Header("�ҋ@UI")]
    Image waitUI;
    [SerializeField, Header("�ҋ@����")]
    Image waitText;
    [SerializeField, Header("A�{�^��UI")]
    Image[] aButton;
    [SerializeField, Header("OK�l��UI")]
    Image okNum;
    [SerializeField, Header("�l��UI")]
    Sprite[] okSprite;
    [SerializeField, Header("�l�pUI")]
    Image[] square;
    [SerializeField, Header("OKUI")]
    Image[] okUI;

    public IEnumerator SetUI()
    {
        waitUI.gameObject.SetActive(true);

        float time = 0;
        float maxTime = 0.15f;
        while (time < maxTime)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(0, 1, time / maxTime);
            waitUI.color = new Color(
                waitUI.color.r,
                waitUI.color.g,
                waitUI.color.b,
                alpha
                );
            waitText.color = new Color(
                waitText.color.r,
                waitText.color.g,
                waitText.color.b,
                alpha
                );
            okNum.color = new Color(
                okNum.color.r,
                okNum.color.g,
                okNum.color.b,
                alpha
                );
            for (int i = 0; i < 4; i++)
            {
                square[i].color = new Color(
                square[i].color.r,
                square[i].color.g,
                square[i].color.b,
                alpha
                );
                aButton[i].color = new Color(
                aButton[i].color.r,
                aButton[i].color.g,
                aButton[i].color.b,
                alpha
                );
            }

            yield return null;
        }
    }
    public void ChangeUI(int num)
    {
        square[num].gameObject.SetActive(false);
        okNum.sprite = okSprite[num];
        okUI[num].enabled = true;
        if (num == 0)
        {
            waitText.enabled = false;
            StartCoroutine(DeleteUI());
        }       
    }
    IEnumerator DeleteUI()
    {
        yield return new WaitForSeconds(1.0f);

        float time = 0;
        float maxTime = 0.15f;
        while (time < maxTime)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(1, 0, time / maxTime);
            waitUI.color = new Color(
                waitUI.color.r,
                waitUI.color.g,
                waitUI.color.b,
                alpha
                );
            okNum.color = new Color(
                okNum.color.r,
                okNum.color.g,
                okNum.color.b,
                alpha
                );
            for(int i = 0; i < 4; i++)
            {
                okUI[i].color = new Color(
                okUI[i].color.r,
                okUI[i].color.g,
                okUI[i].color.b,
                alpha
                );
            }

            yield return null;
        }
        waitUI.gameObject.SetActive(false);
    }
}
