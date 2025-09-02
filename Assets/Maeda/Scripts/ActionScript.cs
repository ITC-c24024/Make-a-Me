using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class ActionScript : MonoBehaviour
{
    public PlayerController playerController;

    //ロボットオブジェクト
    public GameObject robot;

    //入力制限
    public bool isTimer = false;

    //移動アクション
    public InputAction moveAction;
    //取る、投げるアクション
    public InputAction throwAction;

    private void Awake()
    {
        //ActionMapを取得
        var input = robot.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //対応するアクションを取得
        moveAction = actionMap["Move"];
        throwAction = actionMap["Throw"];
    }

    public IEnumerator PickupDelay()
    {
        isTimer = true;
        yield return null; 
        isTimer = false;
    }
}
