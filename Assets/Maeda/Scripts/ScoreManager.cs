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
    Player[] players = new Player[4];
    
    

    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player();
            players[i].rank = 4;
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
        var sorted = players.OrderBy(score => score).ToArray();
        
        for (int i = 0; i < sorted.Length; i++)
        {
            Debug.Log(sorted[i].score);
        }
    }
}
