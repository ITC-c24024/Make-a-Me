using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField, Header("ロボットオブジェクト")]
    GameObject[] robots;

    void Start()
    {
        for (int i = 0; i < robots.Length; i++)
        {
            Instantiate(robots[i], new Vector3(0, 0, 0), Quaternion.identity);
        }        
    }

    void Update()
    {
        
    }
}
