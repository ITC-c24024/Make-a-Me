using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManagerSC : MonoBehaviour
{

    [SerializeField, Header("シャッタースクリプト")]
    ShutterScript shutterSC;

    void Start()
    {
        StartCoroutine(shutterSC.OpenShutter());
    }

    void Update()
    {
        
    }
}
