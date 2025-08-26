using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    [SerializeField, Header("���{�b�g�I�u�W�F�N�g")]
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
        //���̃v���C���[��Prefab��ύX
        manager.playerPrefab = robots[playerNum];
    }
}
