using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    EnergyBatteryScript batteryScript;

    //�v���C���[�̔ԍ�
    public int playerNum = 0;
    
    [SerializeField,Header("�v���C���[�̈ړ����x")]
    float MoveSpeed = 1.0f;

    //���̓^�C�}�[
    float timer = 0;
    //���͐���
    public bool input = false;

    //�o�b�e���[�̏�������
    public bool haveBattery = false;

    //�ړ��A�N�V����
    InputAction moveAction;
    //���A������A�N�V����
    InputAction throwAction;

    private void Awake()
    {
        //ActionMap���擾
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //�Ή�����A�N�V�������擾
        moveAction = actionMap["Move"];
        throwAction = actionMap["Throw"];
    }

    void Start()
    {
        var gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.SelectPrefab(playerNum);
    }
    
    void Update()
    {
        //���͒l��Vector2�^�Ŏ擾
        Vector2 move = moveAction.ReadValue<Vector2>();

        //�v���C���[���ړ�
        transform.position += new Vector3(move.x, 0, move.y) * MoveSpeed * Time.deltaTime;

        if (move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1)
        {
            //�X�e�B�b�N�̊p�x���v�Z
            float angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
            //�v���C���[����]
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        //�{�^��������������
        var throwAct = throwAction.triggered;
        if (haveBattery && throwAct)
        {
            input = true;

            haveBattery = false;
            Debug.Log("throw");
            batteryScript.Throw();
        }

        if (input)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                timer = 0;
                input = false;
            }
        }
    }

    /// <summary>
    /// �o�b�e���[�X�N���v�g���w��
    /// </summary>
    /// <param name="batterySC">TakeRange�Ŏ�����o�b�e���[�̃X�N���v�g</param>
    public void ChangeBatterySC(EnergyBatteryScript batterySC)
    {
        haveBattery = true;
        batteryScript = batterySC;
    }
}