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
    [SerializeField]
    ScoreManager scoreManager;

    [SerializeField, Header("クローンオブジェクト")]
    GameObject[] clones;
    [SerializeField]
    GameObject player;
    [SerializeField, Header("ハンマーオブジェクト")]
    GameObject hammer;
    [SerializeField, Header("ハンマー追従オブジェクト")]
    GameObject follow;

    [SerializeField, Header("スコアUI")]
    Image[] scoreImage;
    [SerializeField, Header("数字UI")]
    Sprite[] numSprite;

    public float maxTime = 10f;
    [SerializeField] float workTime = 0f;
    [SerializeField, Header("作業クールタイム")]
    float coolTime = 1.0f;
    //作業効率
    private float[] efficiency = new float[] { 1.0f, 1.5f, 2.5f, 4.0f, 6.0f };

    [SerializeField] int score = 0;
    [SerializeField] bool isWork = false;
    bool isArea = false;

    [SerializeField] int workAreaNum = 0;

    [SerializeField, Header("クローンのスタートZ座標")]
    float startPos;
    [SerializeField, Header("クローンの停止Z座標")]
    float stopPos;

    void Start()
    {
        StartCoroutine(MoveMaterial());
    }

    void Update()
    {
        if (isWork)
        {
            workTime += Time.deltaTime * efficiency[energyScript.level-1];          
        }
        if (isArea)
        {
            hammer.transform.position = follow.transform.position;
            hammer.transform.rotation = follow.transform.rotation;
        }

        if(workTime >= maxTime / 2) //二段階
        {
            clones[0].SetActive(false);
            clones[1].SetActive(true);
        }
        if (workTime >= maxTime) //完成
        {
            clones[1].SetActive(false);
            clones[2].SetActive(true);

            isWork = false;
            playerController.Job(isWork);

            workTime = 0;
            score += 1;
            scoreManager.ChangeScore(playerController.playerNum);
            SetUI();

            StartCoroutine(MoveClone());
        }
    }

    /// <summary>
    /// クローン材料の移動
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveMaterial()
    {
        clones[0].SetActive(true);
        
        float time = 0;
        while (time < coolTime)
        {
            time += Time.deltaTime;

            float currentPos = Mathf.Lerp(startPos, stopPos, time / coolTime);
            for(int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = new Vector3(
                clones[i].transform.position.x,
                clones[i].transform.position.y,
                currentPos
                );
            }           

            yield return null;
        }
        if (isArea)
        {
            isWork = true;
            playerController.Job(isWork);
        }
    }
    /// <summary>
    /// 完成クローンの移動
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveClone()
    {       
        yield return new WaitForSeconds(1.0f);

        float time = 0;
        while (time < coolTime)
        {
            time += Time.deltaTime;

            float currentPos = Mathf.Lerp(stopPos, startPos, time / coolTime);
            for (int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = new Vector3(
                clones[i].transform.position.x,
                clones[i].transform.position.y,
                currentPos
                );
            }

            yield return null;
        }
        clones[2].SetActive(false);

        StartCoroutine(MoveMaterial());
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isArea = true;
            isWork = true;
            playerController.Job(isWork);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isArea = false;
            isWork = false;
            playerController.Job(isWork);
            hammer.transform.position = new Vector3(
                player.transform.position.x, 
                player.transform.position.y, 
                -0.035f
                ) ;
            hammer.transform.rotation = Quaternion.Euler(0, 0, 90);
            
        }
    }
}
