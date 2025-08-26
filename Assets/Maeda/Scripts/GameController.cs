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

    public void SelectPrefab(int playerNum)
    {
        //次のプレイヤーのPrefabを変更
        manager.playerPrefab = robots[playerNum];
    }
}
