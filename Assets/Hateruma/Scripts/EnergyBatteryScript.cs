using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyBatteryScript : MonoBehaviour
{
    [SerializeField, Header("�������Ă���v���C���[�̔ԍ�")]
    int ownerNum;

    [SerializeField, Header("�����鋭��")]
    float throwPower;

    [SerializeField, Header("�������Ă���v���C���[�̃I�u�W�F�N�g")]
    GameObject ownerObj;

    [SerializeField, Header("���d�I�u�W�F�N�g")]
    GameObject dischargeObj;

    [SerializeField, Header("���X�|�[���ꏊ�̃I�u�W�F�N�g")]
    GameObject[] respawnObj;

    Rigidbody batteryRB;//�o�b�e���[�I�u�W�F�N�g��Rigidbody
    Collider batteryCol;//�o�b�e���[�I�u�W�F�N�g��Collider

    public bool bombSwitch;//���d�\���ǂ����̃X�C�b�`

    bool isDischarge;//���d�d�����Ȃ��悤�ɂ���t���O

    void Start()
    {
        batteryRB = gameObject.GetComponent<Rigidbody>();
        batteryCol = gameObject.GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        if (ownerObj != null)
        {
            batteryRB.MovePosition(ownerObj.transform.position + new Vector3(0,1.2f,0));
            batteryRB.MoveRotation(ownerObj.transform.rotation * Quaternion.Euler(0,90,0));
        }
    }


    /// <summary>
    /// �����҂�o�^�E�ύX
    /// </summary>
    /// <param name="num">�v���C���[�i���o�[</param>
    /// <param name="player">�v���C���[�̃I�u�W�F�N�g</param>
    public void ChangeOwner(int num, GameObject player)
    {
        ownerNum = num;
        ownerObj = player;
        batteryRB.useGravity = false;
        batteryCol.isTrigger = true;

        bombSwitch = false;

    }

    /// <summary>
    /// �R�A�̕��d�ɂ��A���̏ꍇ�ɘA�����̃R�A�̏����҂𒲂ׂ�
    /// </summary>
    /// <returns>�������Ă���v���C���[�̃i���o�[</returns>
    public int OwnerCheck()
    {
        return ownerNum;
    }

    /// <summary>
    /// ������ꂽ�ۂ̋�������
    /// </summary>
    public void Throw()
    {
        var ownerForward = Quaternion.AngleAxis(-60, ownerObj.transform.right) * ownerObj.transform.forward;

        ownerObj = null;
        batteryRB.useGravity = true;
        batteryCol.isTrigger = false;
        batteryRB.AddForce(ownerForward * throwPower,ForceMode.Impulse);
        bombSwitch = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�A���A�ǃI�u�W�F�N�g�ɂ�����ƕ��d
        if (collision.gameObject.tag.StartsWith("P") ||
            collision.gameObject.CompareTag("Floor") || 
            collision.gameObject.CompareTag("Wall")|| 
            collision.gameObject.CompareTag("Battery"))
        {
            if (bombSwitch)
            {
                StartCoroutine(Discharge());
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //���d�ɓ�����ƘA������
        if (other.gameObject.CompareTag("Discharge"))
        {
            //�A�����̃o�b�e���[�̏��L�҂����
            var playerNum = other.gameObject.transform.parent.GetComponent<EnergyBatteryScript>().OwnerCheck();
            ownerNum = playerNum;

            StartCoroutine(Discharge());//���d
        }
    }

    /// <summary>
    /// ���d����
    /// </summary>
    /// <returns></returns>
    IEnumerator Discharge()
    {
        //�d���h�~
        if (isDischarge)
        {
            yield break;
        }
        else
        {
            isDischarge = true;
        }

        batteryRB.useGravity = false;
        batteryCol.enabled = false;

        batteryRB.velocity = Vector3.zero;//�ړ��̊��������Z�b�g
        batteryRB.angularVelocity = Vector3.zero;//��]�̊��������Z�b�g

        ownerObj = null;

        //���d�͈͂�\��
        dischargeObj.SetActive(true);
        bombSwitch = false;
        yield return new WaitForSeconds(1);
        dischargeObj.SetActive(false);
        isDischarge = false;

        StartCoroutine(Respawn());//���X�|�[��
    }

    /// <summary>
    /// ���X�|�[������
    /// ���X�|�[���ꏊ�Ɏw�肳�ꂽ�I�u�W�F�N�g���烉���_���ɑI�����ă��X�|�[��
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawn()
    {
        ownerNum = 0;
        batteryRB.isKinematic = true;
        batteryCol.enabled = true;
        batteryRB.useGravity = true;

        var selectObj = respawnObj[Random.Range(0, respawnObj.Length)];
        transform.position = selectObj.transform.position;
        transform.rotation = selectObj.transform.rotation;

        yield return new WaitForSeconds(2);

        batteryRB.isKinematic = false;
        batteryRB.AddForce(selectObj.transform.forward * throwPower / 2,ForceMode.Impulse);
    }
}