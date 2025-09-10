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

    //�o�b�e���[�I�u�W�F�N�g��Rigidbody
    Rigidbody batteryRB;

    //���d�\���ǂ����̃X�C�b�`
    public bool bombSwitch;

    //���d�d�����Ȃ��悤�ɂ���t���O
    bool isDischarge;

    void Start()
    {
        batteryRB = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (ownerObj != null)
        {
            transform.position = ownerObj.transform.position + new Vector3(0,0);
            transform.rotation = ownerObj.transform.rotation * Quaternion.Euler(0, 90, 90);
        }
    }


    /// <summary>
    /// �����҂�o�^�E�ύX
    /// </summary>
    /// <param name="num">�v���C���[�i���o�[</param>
    /// <param name="player">�v���C���[�̃I�u�W�F�N�g</param>
    public bool ChangeOwner(int num, GameObject player)
    {
        if (isDischarge) return false;
        
        ownerNum = num;
        ownerObj = player;
        batteryRB.isKinematic = true;

        bombSwitch = false;

        return true;
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
        var ownerForward = Quaternion.AngleAxis(-5, ownerObj.transform.right) * ownerObj.transform.forward;

        ownerObj = null;
        batteryRB.isKinematic = false;

        batteryRB.velocity = Vector3.zero;
        batteryRB.angularVelocity = Vector3.zero;

        batteryRB.AddForce(ownerForward * throwPower,ForceMode.Impulse);
        bombSwitch = true;
    }

    /// <summary>
    /// ��Ə�ɓ��������̃o�b�e���[�h���b�v����
    /// </summary>
    public void Drop()
    {
        ownerObj = null;
        batteryRB.isKinematic = false;

        batteryRB.velocity = Vector3.zero;
        batteryRB.angularVelocity = Vector3.zero;
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
            ownerNum = playerNum;//�z�[�~���O�����̖��c��

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

        batteryRB.isKinematic = true;

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


        var selectObj = respawnObj[Random.Range(0, respawnObj.Length)];
        transform.position = selectObj.transform.position;
        transform.rotation = selectObj.transform.rotation * Quaternion.Euler(0,90,0);

        yield return new WaitForSeconds(2);

        batteryRB.isKinematic = false;
        batteryRB.AddForce(selectObj.transform.forward * throwPower / 2,ForceMode.Impulse);
    }
}