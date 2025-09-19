using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ResultCloneScript : MonoBehaviour
{
    Vector3 pos;

    // x�������̈ړ��͈͂̍ŏ��l
    [SerializeField] float minX;

    // x�������̈ړ��͈͂̍ő�l
    [SerializeField] float maxX;

    // z�������̈ړ��͈͂̍ŏ��l
    [SerializeField] float minZ;

    // z�������̈ړ��͈͂̍ő�l
    [SerializeField] float maxZ;

    // y�������̈ړ��͈͂̍ŏ��l
    [SerializeField] float minY;

    // y�������̈ړ��͈͂̍ő�l
    [SerializeField] float maxY;


    void Start()
    {
    }

    void Update()
    {
        pos = transform.localPosition;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.localPosition = pos;
    }
}
