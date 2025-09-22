using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public struct Player
    {
        public int score;
        public int rank;
    }

    public Player[] players = new Player[4];
    public int maxScore = 0;

    public static ScoreManager Instance { get; private set; }

    void Awake()
    {
        // �V���O���g�����i�����I�u�W�F�N�g������ꍇ�͍폜�j
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ResetScores(); // ������
    }

    /// <summary>
    /// �X�R�A���X�V
    /// </summary>
    public void ChangeScore(int num)
    {
        players[num - 1].score++;
        Ranking();
    }

    /// <summary>
    /// ���ʂ��v�Z
    /// </summary>
    void Ranking()
    {
        var sorted = players
            .Select((p, index) => new { Player = p, index = index })
            .OrderByDescending(x => x.Player.score)
            .ToArray();

        int currentRank = 1;
        for (int i = 0; i < sorted.Length; i++)
        {
            if (i > 0 && sorted[i].Player.score < sorted[i - 1].Player.score)
            {
                currentRank = i + 1;
            }
            int originIndex = sorted[i].index;
            players[originIndex].rank = currentRank;
        }
        maxScore = sorted[0].Player.score;
    }

    /// <summary>
    /// �S�X�R�A�Ə��ʂ����Z�b�g
    /// </summary>
    public void ResetScores()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].score = 0;
            players[i].rank = 0;
        }
        maxScore = 0;
    }
}
