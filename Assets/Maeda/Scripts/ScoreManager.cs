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
        DontDestroyOnLoad(this.gameObject);

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
        // �X�R�A�̍������ɕ��בւ�
        var sorted = players
            .Select((p, index) => new { Player = p, index = index })
            .OrderByDescending(x => x.Player.score)
            .ToArray();

        int currentRank = 1;
        for (int i = 0; i < sorted.Length; i++)
        {            
            //��������
            if (i > 0 && sorted[i].Player.score < sorted[i - 1].Player.score)
            {
                currentRank = i + 1;
            }
            int originIndex = sorted[i].index;
            players[originIndex].rank = currentRank;
        }
        
        for(int i = 0; i < players.Length; i++)
        {
            Debug.Log($"Player{i + 1}: Score={players[i].score}, Rank={players[i].rank}");
        }
    }
}
