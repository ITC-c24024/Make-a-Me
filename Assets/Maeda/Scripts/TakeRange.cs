using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class TakeRange : MonoBehaviour
{
    PlayerController playerController;
    EnergyBatteryScript batteryScript;

    [SerializeField, Header("ロボットオブジェクト")]
    GameObject robot;

    //取れる判定
    bool canTake = false;

    //取るアクション
    InputAction takeAction;

    void Start()
    {
        playerController = robot.GetComponent<PlayerController>();
        
        //ActionMapを取得
        var input = playerController.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //対応するアクションを取得
        takeAction = actionMap["Throw"];
    }

    void Update()
    {
        var takeAct = takeAction.triggered;
        if (takeAct && canTake && !playerController.input)
        {
            batteryScript.ChangeOwner(playerController.playerNum, robot);
            playerController.ChangeBatterySC(batteryScript);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {
            canTake = true;
            batteryScript = other.gameObject.GetComponent<EnergyBatteryScript>();           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {
            canTake = false;
        }
    }
}
