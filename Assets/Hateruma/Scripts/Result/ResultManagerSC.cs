using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManagerSC : MonoBehaviour
{

    [SerializeField, Header("�V���b�^�[�X�N���v�g")]
    ShutterScript shutterSC;

    void Start()
    {
        StartCoroutine(shutterSC.OpenShutter());
    }

    void Update()
    {
        
    }
}
