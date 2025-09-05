using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    //�I�u�W�F�N�g�̃��b�V��
    MeshRenderer mesh;

    //�_�ŏ����̃R���[�`��
    Coroutine blinkCoroutine;

    [SerializeField,Header("�q�I�u�W�F�N�g���܂ނ��ǂ���")] 
    bool all;

    //�q�I�u�W�F�N�g
    Transform[] childObj;

    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();//�I�u�W�F�N�g�̃��b�V���擾
    }

    /// <summary>
    /// �u�����N(�_��)������
    /// </summary>
    /// <param name="time">�_�Ŏ���</param>
    /// <param name="speed">�_�Ŏ���</param>
    /// <param name="lastSpeed">�����鐡�O�̓_�Ŏ���</param>
    public void BlinkStart(int time, float speed, float lastSpeed)
    {
        //���łɎ��s����Ă����ꍇ�ɏd�����Ȃ��悤�ɏ������~�߂ĐV�����X�^�[�g������
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkCount(time, speed, lastSpeed));
    }

    /// <summary>
    /// �u�����N�����{�́ABlinkStart�֐�����̂݌Ăт���
    /// </summary>
    IEnumerator BlinkCount(int time, float speed, float lastSpeed)
    {
        var currentTime = 0f;//���݂̎���

        while (currentTime < time)
        {
            mesh.enabled = !mesh.enabled;//���b�V���̕\���ؑ�

            //�c��2�b�ȉ��ɂȂ�����_�ő��x��ύX
            if (time-currentTime <= 2f)
            {
                speed = lastSpeed;
            }

            yield return new WaitForSeconds(speed);//�_�ł̎������҂�
            currentTime += speed;//�҂������̎��Ԃ𑫂�
        }

        mesh.enabled = true;//�ŏI�I�ɕ\����ԂɂȂ�悤�ɂ���
    }
}
