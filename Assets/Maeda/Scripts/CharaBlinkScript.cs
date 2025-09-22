using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBlinkScript : MonoBehaviour
{
    [SerializeField]
    [Header("�_�ŊԊu")]
    float timeOut = 0.1f;

    float timeElapsed;//�_�ŊԊu���v�Z����^�C�}�[

    bool isVisible = true;//�L�����N�^�[��setActive��������bool

    [SerializeField] GameObject[] playerParts = new GameObject[3];//�L�����N�^�[�̃p�[�c��3�����

    private bool isBlink = false;

    void Update()
    {
        if (isBlink)
        {
            CharacterBlinking();
        }
    }

    public void BlinkStart(bool set)
    {
        isBlink = set;
        if (!set)
        {
            foreach (var part in playerParts)
            {
                part.SetActive(true);
            }
        }
    }

    //�L�����N�^�[��_�ł�����
    void CharacterBlinking()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeOut)
        {
            isVisible = !isVisible; // ON/OFF��؂�ւ�
            foreach (var part in playerParts)
            {
                part.SetActive(isVisible);
            }
            timeElapsed = 0f;
        }
    }
}
