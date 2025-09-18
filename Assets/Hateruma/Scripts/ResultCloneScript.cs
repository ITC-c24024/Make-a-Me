using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ResultCloneScript : MonoBehaviour
{
    Vector3 pos;

    // x軸方向の移動範囲の最小値
    [SerializeField] float minX;

    // x軸方向の移動範囲の最大値
    [SerializeField] float maxX;

    // z軸方向の移動範囲の最小値
    [SerializeField] float minZ;

    // z軸方向の移動範囲の最大値
    [SerializeField] float maxZ;

    // y軸方向の移動範囲の最小値
    [SerializeField] float minY;

    // y軸方向の移動範囲の最大値
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
