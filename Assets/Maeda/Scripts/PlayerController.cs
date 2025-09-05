using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActionScript
{
    [SerializeField]
    TakeRange takeRangeSC;
    [SerializeField]
    CatchRange catchRangeSC;
    EnergyBatteryScript batteryScript;
    //�G�l���M�[�Ǘ��X�N���v�g
    EnergyScript energyScript;

    //�v���C���[�̔ԍ�
    public int playerNum = 0;
    
    [SerializeField,Header("�v���C���[�̈ړ����x")]
    float MoveSpeed = 1.0f;   

    //�o�b�e���[�̏�������
    public bool haveBattery = false;

    [SerializeField, Header("�X�^�����ʎ���")]
    float stanTime = 2.0f;
    //�X�^������
    bool isStan = false;

    void Start()
    {
        energyScript = GetComponent<EnergyScript>();
    }
    
    void Update()
    {
        if (!isStan)
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

        //�{�^��������������
        var throwAct = throwAction.triggered;
        if (haveBattery && throwAct && !isTimer && !isStan)
        {
            ChangeHaveBattery();
            StartCoroutine(takeRangeSC.PickupDelay());
            StartCoroutine(catchRangeSC.PickupDelay());
            
            batteryScript.Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Discharge"))
        {
            StartCoroutine(Stan());
        }
    }

    /// <summary>
    /// �o�b�e���[�X�N���v�g���w��
    /// </summary>
    /// <param name="batterySC">TakeRange�Ŏ�����o�b�e���[�̃X�N���v�g</param>
    public void ChangeBatterySC(EnergyBatteryScript batterySC)
    {
        ChangeHaveBattery();
        batteryScript = batterySC;

        StartCoroutine(PickupDelay());
    }

    /// <summary>
    /// �o�b�e���[���������؂�ւ�
    /// �`���[�W�����؂�ւ�
    /// </summary>
    public void ChangeHaveBattery()
    {
        haveBattery = !haveBattery;

        energyScript.ChargeSwitch(haveBattery);
    }

    /// <summary>
    /// �X�^������
    /// </summary>
    /// <returns></returns>
    IEnumerator Stan()
    {
        isStan = true;
        ChangeHaveBattery();
        yield return new WaitForSeconds(stanTime);
        isStan = false;
    }
}