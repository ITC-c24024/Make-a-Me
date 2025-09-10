using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideConveyorScript : MonoBehaviour
{

    [SerializeField, Header("�J�n�ʒu")]
    public Vector3 startPos;

    [SerializeField, Header("�I���ʒu")]
    public Vector3 finishPos;

    [SerializeField, Header("�N���[���I�u�W�F�N�g")]
    public List<ScrollObjScript> cloneSC;

    [SerializeField, Header("�X�N���[�����x")]
    public float scrollSpeed;

    public virtual void AddClone(ScrollObjScript scrollObj, int playerNum)
    {

    }
}
