using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    [SerializeField, Header("ロボットオブジェクト")]
    GameObject[] robots;

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
    }
}
