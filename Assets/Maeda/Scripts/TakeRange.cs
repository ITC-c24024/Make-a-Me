using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeRange : ActionScript
{
    EnergyBatteryScript batteryScript;
    PlayerController playerController;
    public PlayerController[] playerControllers = new PlayerController[4];    

    //���锻��
    public bool canTake = false;

    void Start()
    {
        playerController = robot.GetComponent<PlayerController>();       
    }

    void Update()
    {
        var takeAct = throwAction.triggered;
        if (!playerController.haveBattery && takeAct && canTake && !isTimer && !playerController.isStun)
        {
            TakeBattery();
        }
    }

    /// <summary>
    /// �C���X�^���X������Player��PlayerController���擾
    /// </summary>
    /// <param name="i">�z��̗v�f����GameController����w��</param>
    /// <param name="pc">�i�[����PlayerController��GameController����w��</param>
    public void SetPlayerSC(int i,PlayerController pc)
    {
        playerControllers[i] = pc;
    }

    /// <summary>
    /// �o�b�e���[�擾����
    /// </summary>
    void TakeBattery()
    {
        if (batteryScript != null)
        {
            Debug.Log("take");
            var ownerNum = batteryScript.OwnerCheck();
            
            //�o�b�e���[�̏����҂�����Ƃ�
            if (ownerNum != 0)
            {
                //�D��������̏��������false
                playerControllers[ownerNum - 1].ChangeHaveBattery(false);
            }
            //�o�b�e���[�����҂������ɂ���
            canTake = false;
            batteryScript.ChangeOwner(playerController.playerNum, handObj);
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
            canTake = false;               
        } 
    }
}
