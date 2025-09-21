using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampPos : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.localPosition;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.localPosition = pos;
    }
}
