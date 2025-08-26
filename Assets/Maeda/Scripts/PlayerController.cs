using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    //�v���C���[�̔ԍ�
    public int playerNum = 0;
    
    [SerializeField,Header("�v���C���[�̈ړ����x")] 
    float MoveSpeed = 1.0f;

    //�ړ��A�N�V����
    public InputAction moveAction;
    //������A�N�V����
    public InputAction throwAction;

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
    }
}