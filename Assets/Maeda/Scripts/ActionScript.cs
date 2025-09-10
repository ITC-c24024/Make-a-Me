using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class ActionScript : MonoBehaviour
{
    //���{�b�g�I�u�W�F�N�g
    public GameObject robot;
    //�o�b�e���[�Ǐ]�I�u�W�F�N�g
    public GameObject handObj;

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

    /// <summary>
    /// ���̓N�[���^�C��
    /// </summary>
    /// <returns></returns>
    public IEnumerator PickupDelay()
    {
        isTimer = true;
        yield return new WaitForSeconds(0.2f); 
        isTimer = false;
    }
}
