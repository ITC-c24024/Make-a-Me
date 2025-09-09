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

    [SerializeField, Header("スコアUI")]
    Image[] scoreImage;
    [SerializeField, Header("数字UI")]
    Sprite[] numSprite;

    public float maxTime = 10f;
    [SerializeField] float workTime = 0f;
    //作業効率
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
        int ten = score / 10; //十の位
        int one = score - 10 * ten; //一の位

        //十の位がある時,十の位が表示されていないとき
        if (ten > 0 && !scoreImage[1].enabled)
        {
            Debug.Log("hoge");
            //一の位を右にずらす
            scoreImage[0].rectTransform.anchoredPosition = new Vector2(
                scoreImage[0].rectTransform.anchoredPosition.x + 20,
                scoreImage[0].rectTransform.anchoredPosition.y
                );
            //十の位を表示
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
