using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene2 : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Vector3 startPosition = new Vector3(131.14f, 22.9f, -16f);
    [SerializeField] Vector3 startRotation = new Vector3(0f, 0f, 0f);
    private string yellStateName = "Yell"; // Animator のステート名に合わせる
    private string idleStateName = "Idle";      // Idle ステート名

    void Update()
    {
        foreach (GameObject player in players)
        {
            Animator anim = player.GetComponent<Animator>();
            float z = player.transform.position.z;

            // Zが-9以上でアニメーション開始（Yell）
            if (z >= -9f && !anim.GetCurrentAnimatorStateInfo(0).IsName(yellStateName))
            {
                anim.SetBool("Yell", true);
                anim.Play(yellStateName, 0, 0f); // 0フレームから再生
            }

            // Zが46以上ならアニメーション停止
            if (z >= 46f)
            {
                //anim.SetBool("Yell", false);
                //anim.Play(idleStateName, 0, 0f); // Idle に即切替
            }

            // Zが52以上なら完全リセット＆Yell再スタート
            if (z >= 52f)
            {
                // 位置リセット
                player.transform.position = new Vector3(
                    startPosition.x,
                    player.transform.position.y,
                    startPosition.z);

                // 回転リセット
                //player.transform.rotation = Quaternion.Euler();

                // アニメーションを最初から再生
                //anim.SetBool("Yell", false);
                //anim.Play(idleStateName, 0, 0f);    // 一旦 Idle
                //anim.SetBool("Yell", true);
                //anim.Play(yellStateName, 0, 0f);    // 0フレームから Yell 再生
            }

            // 横移動は Translate で制御
            player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnAnimatorMove()
    {
        foreach (GameObject player in players)
        {
            Animator anim = player.GetComponent<Animator>();
            if (anim && anim.applyRootMotion)
            {
                // Y軸の高さだけ反映
                player.transform.position += new Vector3(0, anim.deltaPosition.y, 0);
            }
        }
    }
}
