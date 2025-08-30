using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class ActionScript : MonoBehaviour
{
    public PlayerController playerController;

    //���{�b�g�I�u�W�F�N�g
    public GameObject robot;

    //���͐���
    public bool isTimer = false;

    //�ړ��A�N�V����
    public InputAction moveAction;
    //���A������A�N�V����
    public InputAction throwAction;

    private void Awake()
    {
        //ActionMap���擾
        var input = robot.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //�Ή�����A�N�V�������擾
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
