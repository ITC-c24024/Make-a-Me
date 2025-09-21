using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManagerSC : MonoBehaviour
{
    [SerializeField, Header("�X�R�A�}�l�[�W���[�X�N���v�g")]
    ScoreManager scoreManaSC;

    [SerializeField, Header("�V���b�^�[�X�N���v�g")]
    ShutterScript shutterSC;

    [SerializeField, Header("�f�B�X�y���T�[�X�N���v�g")]
    DispencerScript[] dispencersSC;

    [SerializeField, Header("���ʃC���[�W")]
    Image[] rankImage;

    [SerializeField, Header("���ʃX�v���C�g")]
    Sprite[] rankSprite;

    [SerializeField, Header("�{�^���p�l��")]
    GameObject buttonPanelObj;

    int showResult = 0;
    [SerializeField, Header("����")]
    List<int> rank = new();

    [SerializeField]
    AudioManager audioManager;
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
                int playerRank = scoreManaSC.players[i].rank;

                if (playerRank > 0 && playerRank <= rankSprite.Length)
                {
                    rankImage[i].sprite = rankSprite[playerRank - 1];
                    rankImage[i].enabled = true;
                    audioManager.Rank();
                    StartCoroutine(BounceUI(rankImage[i].transform, 0.3f));
                }
                else
                {
                    rankImage[i].enabled = false;
                    Debug.LogWarning($"Invalid rank for player {i + 1}: {playerRank}");
                }
            }



            yield return new WaitForSeconds(0.5f);

            audioManager.Result();

            yield return new WaitForSeconds(1.5f);

            buttonPanelObj.SetActive(true);
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
