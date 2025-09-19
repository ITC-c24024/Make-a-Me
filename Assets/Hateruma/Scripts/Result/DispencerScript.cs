using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class DispencerScript : MonoBehaviour
{
    [SerializeField, Header("開始時間")]
    float startTime;

    [SerializeField, Header("発射オブジェクト")]
    GameObject shotObj;

    [SerializeField, Header("プレイヤー番号")]
    int playerNum;

    [SerializeField] List<GameObject> cloneObj; // SerializeField を残す
    List<Rigidbody> cloneRB;

    [SerializeField, Header("クローンの数")]
    int cloneCount;

    //発射数
    int shotCount;

    [SerializeField, Header("順位イメージ")]
    Image[] rankImage;

    [SerializeField, Header("カウントのイメージ")]
    Image[] countImage;

    [SerializeField, Header("数字のスプライト")]
    Sprite[] numSprite;

    ScoreManager scoreManaSC;
    private void Awake()
    {
        //scoreManaSC = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        if (cloneObj == null) cloneObj = new List<GameObject>();
        if (cloneRB == null) cloneRB = new List<Rigidbody>();

        //cloneCount = scoreManaSC.players[playerNum - 1].score;

        GameObject[] clones = GameObject.FindGameObjectsWithTag($"ResultCloneP{playerNum}");
        cloneObj.AddRange(clones);
    }


    void Start()
    {
        foreach (var obj in cloneObj)
        {
            cloneRB.Add(obj.GetComponent<Rigidbody>());
        }
        StartCoroutine(ShotClone());
    }

    IEnumerator ShotClone()
    {
        var count = 0.25f;

        yield return new WaitForSeconds(startTime);

        while (cloneCount > 0)
        {
            shotObj.transform.localEulerAngles = new Vector3(
                Random.Range(60f, 120f),
                Random.Range(-30f, 30f)
            );

            var clone = cloneObj[0];
            var rb = cloneRB[0];

            clone.transform.position = shotObj.transform.position;

            rb.isKinematic = false;
            rb.AddForce(shotObj.transform.forward * 10, ForceMode.Impulse);

            cloneObj.RemoveAt(0);
            cloneRB.RemoveAt(0);

            cloneCount--;
            shotCount++;

            if (count > 0.1f)
            {
                count -= 0.01f;
            }

            SetUI();

            yield return new WaitForSeconds(count);
        }
    }

    void SetUI()
    {
        int ten = shotCount / 10; //十の位
        int one = shotCount - 10 * ten; //一の位

        //十の位がある時,十の位が表示されていないとき
        if (ten > 0 && !countImage[1].enabled)
        {
            Debug.Log("hoge");
            //一の位を右にずらす
            countImage[0].rectTransform.anchoredPosition = new Vector2(
                countImage[0].rectTransform.anchoredPosition.x + 20,
                countImage[0].rectTransform.anchoredPosition.y
                );
            //十の位を表示
            countImage[1].enabled = true;
        }

        countImage[0].sprite = numSprite[one];
        countImage[1].sprite = numSprite[ten];
    }
}
