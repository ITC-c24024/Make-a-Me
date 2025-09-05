using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    //オブジェクトのメッシュ
    MeshRenderer mesh;

    //点滅処理のコルーチン
    Coroutine blinkCoroutine;

    [SerializeField,Header("子オブジェクトを含むかどうか")] 
    bool all;

    //子オブジェクト
    Transform[] childObj;

    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();//オブジェクトのメッシュ取得
    }

    /// <summary>
    /// ブリンク(点滅)させる
    /// </summary>
    /// <param name="time">点滅時間</param>
    /// <param name="speed">点滅周期</param>
    /// <param name="lastSpeed">消える寸前の点滅周期</param>
    public void BlinkStart(int time, float speed, float lastSpeed)
    {
        //すでに実行されていた場合に重複しないように処理を止めて新しくスタートさせる
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkCount(time, speed, lastSpeed));
    }

    /// <summary>
    /// ブリンク処理本体、BlinkStart関数からのみ呼びだす
    /// </summary>
    IEnumerator BlinkCount(int time, float speed, float lastSpeed)
    {
        var currentTime = 0f;//現在の時間

        while (currentTime < time)
        {
            mesh.enabled = !mesh.enabled;//メッシュの表示切替

            //残り2秒以下になったら点滅速度を変更
            if (time-currentTime <= 2f)
            {
                speed = lastSpeed;
            }

            yield return new WaitForSeconds(speed);//点滅の周期分待つ
            currentTime += speed;//待った分の時間を足す
        }

        mesh.enabled = true;//最終的に表示状態になるようにする
    }
}
