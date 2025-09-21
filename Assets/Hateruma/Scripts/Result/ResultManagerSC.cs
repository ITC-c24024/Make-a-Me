using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManagerSC : MonoBehaviour
{
    [SerializeField, Header("スコアマネージャースクリプト")]
    ScoreManager scoreManaSC;

    [SerializeField, Header("シャッタースクリプト")]
    ShutterScript shutterSC;

    [SerializeField, Header("ディスペンサースクリプト")]
    DispencerScript[] dispencersSC;

    [SerializeField, Header("順位イメージ")]
    Image[] rankImage;

    [SerializeField, Header("順位スプライト")]
    Sprite[] rankSprite;

    [SerializeField, Header("ボタンパネル")]
    GameObject buttonPanelObj;

    int showResult = 0;
    [SerializeField, Header("順位")]
    List<int> rank = new();

    private void Awake()
    {
        scoreManaSC = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        foreach (var sc in dispencersSC)
        {
            sc.scoreManaSC = scoreManaSC;
            sc.resultManaSC = this;
        }
    }

    void Start()
    {
        // Startで順位を取得
        rank.Clear();
        for (int i = 0; i < 4; i++)
        {
            rank.Add(scoreManaSC.players[i].rank);
        }

        StartCoroutine(shutterSC.OpenShutter());
    }

    public IEnumerator ShowResultAdd()
    {
        showResult++;
        if (showResult == 4)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < 4; i++)
            {
                rankImage[i].sprite = rankSprite[rank[i] - 1]; 
                rankImage[i].enabled = true;
                StartCoroutine(BounceUI(rankImage[i].transform, 0.3f));
            }

            yield return new WaitForSeconds(2);

            buttonPanelObj.SetActive(true);
            StartCoroutine(BounceUI(buttonPanelObj.transform, 0.3f));
        }
    }

    IEnumerator BounceUI(Transform target, float time)
    {
        float t = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float s = 2f;
            float curved = 1f + s * Mathf.Pow(t - 1f, 3) + s * Mathf.Pow(t - 1f, 2);
            target.localScale = Vector3.LerpUnclamped(startScale, endScale, curved);
            yield return null;
        }
        target.localScale = Vector3.one;
    }
}
