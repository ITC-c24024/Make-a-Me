using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffectScript : MonoBehaviour
{

    [SerializeField, Header("Impact�G�t�F�N�g�I�u�W�F�N�g")]
    GameObject impactEffectObj;

    void HitHammer()
    {
        impactEffectObj.SetActive(true);
        Invoke(nameof(DisableEffect), 0.5f);
    }

    void DisableEffect()
    {
        impactEffectObj.SetActive(false);
    }
}
