using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    GameController gameController;
    
    [SerializeField, Header("中心オブジェクト")]
    GameObject center;
    [SerializeField, Header("歯車外側")]
    Image gearOut;
    [SerializeField, Header("歯車内側")]
    Image gearIn;
    [SerializeField, Header("経過時間スライダー")]
    Slider slider;

    [SerializeField, Header("ゲーム時間(秒)")]
    float limitTime = 180;

    void Start()
    {
        gameController = GetComponent<GameController>();
    }

    /// <summary>
    /// スライダー制御
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        float currentTime = 0;
        while (currentTime < limitTime)
        {
            currentTime += Time.deltaTime;

            gearOut.rectTransform.localEulerAngles += new Vector3(0, 0, -1);
            gearIn.rectTransform.localEulerAngles += new Vector3(0, 0, 4);

            //針を回転
            float rotationZ = Mathf.Lerp(0, -360, currentTime / limitTime);
            center.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            //経過時間スライダーを進める
            float value= Mathf.Lerp(0, 1, currentTime / limitTime);
            slider.value = value;

            yield return null;
        }

        gameController.GameFinish();
    }
}
