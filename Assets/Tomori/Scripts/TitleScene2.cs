using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene2 : MonoBehaviour
{
    [SerializeField] GameObject[] players1;
    [SerializeField] GameObject[] players2;
    [SerializeField] GameObject[] players3;
    [SerializeField] float moveSpeed = 3f;
    private Vector3 startPosition1 = new Vector3(131.14f, 22.9f, -41f);
    private Vector3 startPosition2 = new Vector3(144.13f, 22.9f, 64.3f);
    private Vector3 startPosition3 = new Vector3(160.4f, 22.9f, -41f);
    private string yellStateName = "Yell"; // Animator �̃X�e�[�g���ɍ��킹��

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        startPosition1 = players1[0].transform.position;
        startPosition2 = players2[0].transform.position;
        startPosition3 = players3[0].transform.position;
    }

    void Update()
    {
        foreach (GameObject player in players1)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Z��-9�ȏ�ŃA�j���[�V�����J�n�iYell�j
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0�t���[������Đ�
            }

            // Z��52�ȏ�Ȃ犮�S���Z�b�g��Yell�ăX�^�[�g
            if (z >= 72f)
            {
                // �ʒu���Z�b�g
                player.transform.position = new Vector3(
                    startPosition1.x,
                    player.transform.position.y,
                    startPosition1.z);
            }

            // ���ړ��� Translate �Ő���
            player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }

        /*foreach (GameObject player in players2)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Z��-9�ȏ�ŃA�j���[�V�����J�n�iYell�j
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0�t���[������Đ�
            }

            // Z��-52�ȉ��Ȃ犮�S���Z�b�g��Yell�ăX�^�[�g
            if (z <= -33f)
            {
                // �ʒu���Z�b�g
                player.transform.position = new Vector3(
                    startPosition2.x,
                    player.transform.position.y,
                    startPosition2.z);
            }

            // ���ړ��� Translate �Ő���i�t�����j
            player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }

        foreach (GameObject player in players3)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Z��-9�ȏ�ŃA�j���[�V�����J�n�iYell�j
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0�t���[������Đ�
            }

            // Z��52�ȏ�Ȃ犮�S���Z�b�g��Yell�ăX�^�[�g
            if (z >= 73f)
            {
                // �ʒu���Z�b�g
                player.transform.position = new Vector3(
                    startPosition3.x,
                    player.transform.position.y,
                    startPosition3.z);
            }

            // ���ړ��� Translate �Ő���
            player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }*/
    }
}
