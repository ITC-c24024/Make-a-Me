using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitScript : MonoBehaviour
{
    [SerializeField, Header("待機UI")]
    Image waitUI;
    [SerializeField, Header("待機文字")]
    Image waitText;
    [SerializeField, Header("OK人数UI")]
    Image okNum;
    [SerializeField, Header("人数UI")]
    Sprite[] okSprite;
    [SerializeField, Header("四角UI")]
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
