using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerBindScript : MonoBehaviour
{
    GameController gameController;
    [SerializeField] private GameObject[] playersObj; // シーン上のP1〜P4

    int slotNum = 0;

    void Start()
    {
        gameController = GetComponent<GameController>();
    }

    public void OnPlayerJoined(PlayerInput joined)
    {
        // 仮オブジェクトかどうか判定
        if (joined.gameObject.tag.StartsWith("P"))
        {
            Debug.Log("本物のプレイヤーに割り当て済みなのでスキップ");
            return;
        }

        Debug.Log($"Player {slotNum} joined");

        // 対応する本物プレイヤーのPlayerInput
        var player = playersObj[slotNum].GetComponent<PlayerInput>();

        playersObj[slotNum].SetActive(true);

        // 仮オブジェクトのデバイスを本物に移す
        player.SwitchCurrentControlScheme(joined.devices.ToArray());

        // 仮オブジェクトを削除
        Destroy(joined.gameObject);

        if (slotNum == 1)
        {
            gameController.Count();
        }
        slotNum++;
    }
}


