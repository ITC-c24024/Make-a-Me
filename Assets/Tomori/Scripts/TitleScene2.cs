using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene2 : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Vector3 startPosition = new Vector3(131.14f, 22.9f, -16f);
    [SerializeField] Vector3 startRotation = new Vector3(0f, 0f, 0f);
    private string yellStateName = "Yell"; // Animator �̃X�e�[�g���ɍ��킹��
    private string idleStateName = "Idle";      // Idle �X�e�[�g��

    void Update()
    {
        foreach (GameObject player in players)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Z��-9�ȏ�ŃA�j���[�V�����J�n�iYell�j
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0�t���[������Đ�
            }

            // Z��46�ȏ�Ȃ�A�j���[�V������~
            if (z >= 46f)
            {
                //anim.SetBool("Yell", false);
                //anim.Play(idleStateName, 0, 0f); // Idle �ɑ��ؑ�
            }

            // Z��52�ȏ�Ȃ犮�S���Z�b�g��Yell�ăX�^�[�g
            if (z >= 52f)
            {
                // �ʒu���Z�b�g
                player.transform.position = new Vector3(
                    startPosition.x,
                    player.transform.position.y,
                    startPosition.z);

                // ��]���Z�b�g
                //player.transform.rotation = Quaternion.Euler();

                // �A�j���[�V�������ŏ�����Đ�
                //anim.SetBool("Yell", false);
                //anim.Play(idleStateName, 0, 0f);    // ��U Idle
                //anim.SetBool("Yell", true);
                //anim.Play(yellStateName, 0, 0f);    // 0�t���[������ Yell �Đ�
            }

            // ���ړ��� Translate �Ő���
            player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnAnimatorMove()
    {
        foreach (GameObject player in players)
        {
            Animator anim = player.GetComponent<Animator>();
            if (anim && anim.applyRootMotion)
            {
                // Y���̍����������f
                player.transform.position += new Vector3(0, anim.deltaPosition.y, 0);
            }
        }
    }
}
