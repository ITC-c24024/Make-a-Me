using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class TakeRange : MonoBehaviour
{
    PlayerController playerController;
    EnergyBatteryScript batteryScript;

    [SerializeField, Header("���{�b�g�I�u�W�F�N�g")]
    GameObject robot;

    //���锻��
    bool canTake = false;

    //���A�N�V����
    InputAction takeAction;

    void Start()
    {
        playerController = robot.GetComponent<PlayerController>();
        
        //ActionMap���擾
        var input = playerController.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //�Ή�����A�N�V�������擾
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
