using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public struct Player
    {
        public int score;
        public int rank;
    }
    Player[] players = new Player[4];

    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player();
        }
    }

    /// <summary>
    /// �X�R�A���X�V
    /// </summary>
    /// <param name="num">��Ə�̔ԍ�</param>
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
        for (int i = 0; i < players.Length; i++)
        {
            for (int n = 0; n < players.Length; n++)
            {
                if (i == n) break;

                if (players[i].score > players[n].score)
                {
                    players[i].rank++;
                }             
            }
        }
    }
}
