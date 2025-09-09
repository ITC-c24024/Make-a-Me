using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField, Header("中心オブジェクト")]
    GameObject center;
    [SerializeField, Header("経過時間スライダー")]
    Slider slider;

    [SerializeField, Header("ゲーム時間(秒)")]
    float limitTime = 180;

    void Start()
    {
        
    }

    public IEnumerator Timer()
    {
        float currentTime = 0;
        while (currentTime < limitTime)
        {
            currentTime += Time.deltaTime;
            
            //針を回転
            float rotationZ = Mathf.Lerp(0, -360, currentTime / limitTime);
            center.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            //経過時間スライダーを進める
            float value= Mathf.Lerp(0, 1, currentTime / limitTime);
            slider.value = value;

            yield return null;
        }
    }
}
