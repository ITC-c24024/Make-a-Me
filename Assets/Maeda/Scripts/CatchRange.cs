using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatchRange : ActionScript
{
    EnergyBatteryScript batteryScript;

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
            Debug.Log("take");
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
