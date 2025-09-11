using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : ActionScript
{
    [SerializeField]
    ScoreScript scoreScript;
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
    public bool isStun = false;
    [SerializeField, Header("���G����")]
    float invincibleTime = 2.0f;
    //���G����
    bool invincible = false;

    Rigidbody playerRB;

    Animator animator;

    void Start()
    {
        energyScript = GetComponent<EnergyScript>();
        playerRB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!isStun)
        {
            //���͒l��Vector2�^�Ŏ擾
            Vector2 move = moveAction.ReadValue<Vector2>();

            if(move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1)
            {
                animator.SetBool("Iswalk", true);

                //�v���C���[���ړ�
                transform.position += new Vector3(move.x, 0, move.y) * MoveSpeed * Time.deltaTime;
            }
            else
            {
                animator.SetBool("Iswalk", false);
            }

            if ((move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1) /*&& !scoreScript.isArea*/)
            {
                //�X�e�B�b�N�̊p�x���v�Z
                //float angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
                //�v���C���[�����X�ɉ�]
                Quaternion to = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 720 * Time.deltaTime);
            }
        }
        else animator.SetBool("Iswalk", false);

        //�{�^��������������
        var throwAct = throwAction.triggered;
        if (haveBattery && throwAct && !isTimer && !isStun)
        {
            ChangeHaveBattery(false);
            StartCoroutine(takeRangeSC.PickupDelay());
            StartCoroutine(catchRangeSC.PickupDelay());
            
            batteryScript.Throw();
            animator.SetBool("IsThrow", true);
            animator.SetBool("IsThrow", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Discharge") && !isStun && !invincible)
        {
            StartCoroutine(Stan());
        }
        if (other.CompareTag($"WorkArea{playerNum}") && !isStun && !invincible)
        {          
            if (batteryScript != null)
            {
                ChangeHaveBattery(false);
                batteryScript.Drop();
            }
        }
    }

    /// <summary>
    /// �o�b�e���[�X�N���v�g���w��
    /// </summary>
    /// <param name="batterySC">TakeRange�Ŏ�����o�b�e���[�̃X�N���v�g</param>
    public void ChangeBatterySC(EnergyBatteryScript batterySC)
    {
        ChangeHaveBattery(true);
        batteryScript = batterySC;

        StartCoroutine(PickupDelay());
    }

    /// <summary>
    /// �o�b�e���[���������؂�ւ�
    /// �`���[�W�����؂�ւ�
    /// </summary>
    /// <param name="have">��������ɑ������bool</param>
    public void ChangeHaveBattery(bool have)
    {
        haveBattery = have;
        animator.SetBool("IsHave", haveBattery);

        energyScript.ChargeSwitch(haveBattery);
    }

    /// <summary>
    /// �X�^������
    /// </summary>
    /// <returns></returns>
    IEnumerator Stan()
    {
        //�G�l���M�[�h���b�v
        energyScript.LostEnergy();

        invincible = true;
        if (scoreScript.isWork)
        {
            scoreScript.ChangeIsWork(false);
            JobAnim(false);
        }
        isStun = true;
        animator.SetBool("Isstun", true);
        
        ChangeHaveBattery(false);

        yield return new WaitForSeconds(stanTime);

        isStun = false;
        if (scoreScript.isWork)
        {
            scoreScript.ChangeIsWork(true);
            JobAnim(true);
        }  
        animator.SetBool("Isstun", false);
        
        transform.localRotation = Quaternion.Euler(
            0, 
            transform.localEulerAngles.y, 
            transform.localEulerAngles.z
            );

        Invoke("ResetInvincible", invincibleTime);
    }

    /// <summary>
    /// ���G��ԃ��Z�b�g
    /// </summary>
    void ResetInvincible()
    {
        invincible = false;
    }

    public void JobAnim(bool isWalk)
    {
        if (animator != null)
        {
            animator.SetBool("IsJob", isWalk);
        }
    }
}