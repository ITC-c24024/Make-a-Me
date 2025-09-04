using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeRange : ActionScript
{
    EnergyBatteryScript batteryScript;
    PlayerController playerController;
    public PlayerController[] playerControllers = new PlayerController[4];    

    //取れる判定
    bool canTake = false;

    void Start()
    {
        playerController = robot.GetComponent<PlayerController>();       
    }

    void Update()
    {
        var takeAct = throwAction.triggered;
        if (!playerController.haveBattery && takeAct && canTake && !isTimer)
        {
            TakeBattery();
        }
    }

    public void SetPlayerSC(int i,PlayerController pc)
    {
        playerControllers[i] = pc;
    }

    void TakeBattery()
    {
        Debug.Log("take");
        if (batteryScript != null)
        {
            var ownerNum = batteryScript.OwnerCheck();
            Debug.Log(ownerNum);
            //バッテリーの所持者がいるとき
            if (ownerNum != 0)
            {
                //奪った相手の所持判定をfalse
                playerControllers[ownerNum - 1].ChangeHaveBattery();
            }
            batteryScript.ChangeOwner(playerController.playerNum, robot);
            playerController.ChangeBatterySC(batteryScript);
        }   
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {
            var energyBatterySC = other.gameObject.GetComponent<EnergyBatteryScript>();
            if (!energyBatterySC.bombSwitch)
            {
                canTake = true;
                batteryScript = other.gameObject.GetComponent<EnergyBatteryScript>();
            }                      
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {
            var energyBatterySC = other.gameObject.GetComponent<EnergyBatteryScript>();
            if (!energyBatterySC.bombSwitch)
            {
                canTake = false;
            }                
        } 
    }
}
