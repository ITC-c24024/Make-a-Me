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
    /// スコアを更新
    /// </summary>
    /// <param name="num">作業場の番号</param>
    public void ChangeScore(int num)
    {
        players[num - 1].score++;

        Ranking();
    }

    /// <summary>
    /// 順位を計算
    /// </summary>
    void Ranking()
    {
        // スコアの高い順に並べ替え
        var sorted = players
            .Select((p, index) => new { Player = p, index = index })
            .OrderByDescending(x => x.Player.score)
            .ToArray();

        int currentRank = 1;
        for (int i = 0; i < sorted.Length; i++)
        {            
            //同率処理
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
