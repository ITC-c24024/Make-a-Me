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
        yield return null;
    }
    public void ChangeUI(int num)
    {
        square[num].gameObject.SetActive(false);
        if (num == 3)
        {
            waitText.enabled = false;
            StartCoroutine(DeleteUI());
        }
        okNum.sprite = okSprite[num];
        okUI[num].enabled = true; 
    }
    IEnumerator DeleteUI()
    {
        waitUI.gameObject.SetActive(false);
        yield return null;
    }
}
