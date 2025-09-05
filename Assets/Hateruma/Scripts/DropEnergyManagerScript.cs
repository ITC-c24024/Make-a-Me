using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnergyManagerScript : MonoBehaviour
{
    [SerializeField,Header("�h���b�v�I�u�W�F�N�g")] 
    GameObject[] dropObj = new GameObject[3];

    //�h���b�v�I�u�W�F�N�g�̃X�N���v�g
    DropEnergyScript[] dropEnergySC;

    //�v���C���[�I�u�W�F�N�g
    public GameObject playerObj;

    [SerializeField,Header("�g�p����Ă��Ȃ��h���b�v�I�u�W�F�N�g���i�[���邽�߂̃��X�g")] 
    List<DropEnergyScript> dropList = new List<DropEnergyScript>();

    void Start()
    {
        dropEnergySC = new DropEnergyScript[dropObj.Length];//�I�u�W�F�N�g�̐��ɉ����Ĕz��͈̔͂��w��

        playerObj = this.gameObject;

        for (int i = 0; i < dropObj.Length; i++)
        {
            dropEnergySC[i] = dropObj[i].GetComponent<DropEnergyScript>();//�h���b�v�I�u�W�F�N�g�̃X�N���v�g���擾

            dropEnergySC[i].playerObj = playerObj;//�Ή������v���C���[�����蓖�Ă�

            dropEnergySC[i].dropManagerSC = this;//�h���b�v�I�u�W�F�N�g��DropManagerScript�����蓖��

            dropEnergySC[i].SetNum(i);//�e�I�u�W�F�N�g�ɔԍ���t����(���X�g�ɖ߂��ۂɎ��ʂ��邽��)

            dropList.Add(dropEnergySC[i]);//�h���b�v�I�u�W�F�N�g�����X�g�ɒǉ�
        }
    }

    /// <summary>
    /// �G�l���M�[�I�u�W�F�N�g���h���b�v������
    /// </summary>
    /// <param name="amount">�h���b�v����G�l���M�[�̗�</param>
    public void Drop(int amount)
    {
        //���X�g����g�p����Ă��Ȃ��I�u�W�F�N�g��I��Ńh���b�v
        if(dropList.Count > 0)
        {
            StartCoroutine(dropList[0].SetHoneyAmount(amount));
            dropList.RemoveAt(0);
        }
    }
    /// <summary>
    /// �E��ꂽ�I�u�W�F�N�g�����X�g�ɉ�����
    /// </summary>
    /// <param name="num">�I�u�W�F�N�g�i���o�[</param>
    public void AddDrop(int num)
    {
        dropList.Add(dropEnergySC[num]);
    }
}
