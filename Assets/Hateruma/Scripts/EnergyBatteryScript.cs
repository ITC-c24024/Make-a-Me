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

    Rigidbody batteryRB;//Core�I�u�W�F�N�g��Rigidbody

    [SerializeField, Header("���d�\���ǂ����̃X�C�b�`")]
    bool bombSwitch;

    bool isDischarge;//���d�d�����Ȃ��悤�ɂ���t���O

    void Start()
    {
        batteryRB = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (ownerObj != null)
        {
            batteryRB.MovePosition(ownerObj.transform.position + Vector3.up);
            batteryRB.MoveRotation(ownerObj.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
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
        batteryRB.isKinematic = true;
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
        var ownerForward = ownerObj.transform.forward;

        ownerObj = null;
        batteryRB.isKinematic = false;
        batteryRB.AddForce(ownerForward * throwPower, ForceMode.Impulse);
        bombSwitch = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�A���A�ǃI�u�W�F�N�g�ɂ�����ƕ��d
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall"))
        {
            if (bombSwitch)
            {
                StartCoroutine(Discharge());
            }
        }
        else if (collision.gameObject.CompareTag("Discharge"))
        {
            var playerNum = collision.gameObject.transform.parent.GetComponent<EnergyBatteryScript>().OwnerCheck();
            ownerNum = playerNum;
            StartCoroutine(Discharge());
        }
    }

    /// <summary>
    /// ���d����
    /// </summary>
    /// <returns></returns>
    IEnumerator Discharge()
    {
        if (isDischarge)
        {
            yield break;
        }
        else
        {
            isDischarge = true;
        }

        batteryRB.velocity = Vector3.zero;//�ړ��̊��������Z�b�g
        batteryRB.angularVelocity = Vector3.zero;//��]�̊��������Z�b�g

        dischargeObj.SetActive(true);
        bombSwitch = false;
        yield return new WaitForSeconds(1);
        dischargeObj.SetActive(false);
        isDischarge = false;

        StartCoroutine(Respawn());
    }


    IEnumerator Respawn()
    {
        batteryRB.isKinematic = true;

        var selectObj = respawnObj[Random.Range(0, respawnObj.Length)];
        transform.position = selectObj.transform.position;
        transform.rotation = selectObj.transform.rotation;

        yield return new WaitForSeconds(2);

        batteryRB.isKinematic = false;
        batteryRB.AddForce(selectObj.transform.forward * throwPower / 2, ForceMode.Impulse);
    }
}