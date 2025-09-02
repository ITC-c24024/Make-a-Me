using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeRange : ActionScript
{
    EnergyBatteryScript batteryScript;
    PlayerController playerController;
    PlayerController targetPC;

    //Žæ‚ê‚é”»’è
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

    void TakeBattery()
    {
        if (targetPC != null)
        {
            Debug.Log("’D‚Á‚½");
            targetPC.ChangeHaveBattery();
        }

        Debug.Log("take");
        if (batteryScript != null)
        {
            batteryScript.ChangeOwner(playerController.playerNum, robot);
        }

        playerController.ChangeBatterySC(batteryScript);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("pc");
            var playerSC = other.GetComponent<PlayerController>();
            if (playerSC.haveBattery)
            {
                targetPC = playerSC;
                
                canTake = true;
                batteryScript = other.gameObject.GetComponent<EnergyBatteryScript>();
            }
        }

        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {
            canTake = true;
            batteryScript = other.gameObject.GetComponent<EnergyBatteryScript>();           
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerController.haveBattery && other.gameObject.CompareTag("Player"))
        {
            var playerSC = other.GetComponent<PlayerController>();
            if (playerSC.haveBattery)
            {
                targetPC = null;

                canTake = false;
                batteryScript = null;
            }
        }

        if (!playerController.haveBattery && other.gameObject.CompareTag("Battery"))
        {
            canTake = false;
        } 
    }
}
