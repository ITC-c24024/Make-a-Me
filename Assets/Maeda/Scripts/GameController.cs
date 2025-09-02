using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    [SerializeField, Header("ロボットPrefab")]
    GameObject[] robots;
    //インスタンス化したロボットオブジェクト
    public GameObject[] players = new GameObject[4];
    public PlayerController[] playerControllers = new PlayerController[4];
    public TakeRange[] takeRanges = new TakeRange[4];

    PlayerInputManager manager;

    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 次のプレイヤーのPrefabを変更
    /// </summary>
    /// <param name="playerNum">ロボットPrefabの要素数を指定</param>
    public void SelectPrefab(int playerNum)
    {
        manager.playerPrefab = robots[playerNum];

        playerControllers[playerNum - 1] = players[playerNum - 1].GetComponent<PlayerController>();
        takeRanges[playerNum - 1] = players[playerNum - 1].GetComponentInChildren<TakeRange>();  

        for (int i = 0; i < players.Length; i++)
        {
            for (int n = 0; n < players.Length; n++)
            {
                if (players[i] != null)
                {
                    takeRanges[i].SetPlayerSC(n, playerControllers[n]);
                }
            }
        }
    }

    public void SetPlayer(int num, GameObject player)
    {
        players[num - 1] = player;
    }
}
