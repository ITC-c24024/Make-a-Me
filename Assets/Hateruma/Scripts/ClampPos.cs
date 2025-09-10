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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.localPosition;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.localPosition = pos;
    }
}
