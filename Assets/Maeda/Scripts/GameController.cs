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

    /// <summary>
    /// ���̃v���C���[��Prefab��ύX
    /// </summary>
    /// <param name="playerNum">���{�b�gPrefab�̗v�f�����w��</param>
    public void SelectPrefab(int playerNum)
    {
        manager.playerPrefab = robots[playerNum];
    }
}
