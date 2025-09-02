using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyBatteryScript : MonoBehaviour
{
    [SerializeField, Header("所持しているプレイヤーの番号")]
    int ownerNum;

    [SerializeField, Header("投げる強さ")]
    float throwPower;

    [SerializeField, Header("所持しているプレイヤーのオブジェクト")]
    GameObject ownerObj;

    [SerializeField, Header("放電オブジェクト")]
    GameObject dischargeObj;

    [SerializeField, Header("リスポーン場所のオブジェクト")]
    GameObject[] respawnObj;

    Rigidbody batteryRB;//バッテリーオブジェクトのRigidbody
    Collider batteryCol;//バッテリーオブジェクトのCollider

    public bool bombSwitch;//放電可能かどうかのスイッチ

    bool isDischarge;//放電重複しないようにするフラグ

    void Start()
    {
        batteryRB = gameObject.GetComponent<Rigidbody>();
        batteryCol = gameObject.GetComponent<Collider>();
    }

    void FixedUpdate()
    {

        if (ownerObj != null)
        {
            batteryRB.MovePosition(ownerObj.transform.position + Vector3.up);
            batteryRB.MoveRotation(ownerObj.transform.rotation * Quaternion.Euler(0,90,0));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
        }
    }


    /// <summary>
    /// 所持者を登録・変更
    /// </summary>
    /// <param name="num">プレイヤーナンバー</param>
    /// <param name="player">プレイヤーのオブジェクト</param>
    public void ChangeOwner(int num, GameObject player)
    {
        ownerNum = num;
        ownerObj = player;
        batteryRB.isKinematic = true;
        batteryCol.isTrigger = true;

        bombSwitch = false;
    }

    /// <summary>
    /// コアの放電による連鎖の場合に連鎖元のコアの所持者を調べる
    /// </summary>
    /// <returns>所持しているプレイヤーのナンバー</returns>
    public int OwnerCheck()
    {
        return ownerNum;
    }

    /// <summary>
    /// 投げられた際の挙動処理
    /// </summary>
    public void Throw()
    {
        var ownerForward = ownerObj.transform.forward;

        ownerObj = null;
        batteryRB.isKinematic = false;
        batteryCol.isTrigger = false;
        batteryRB.AddForce(ownerForward * throwPower, ForceMode.Impulse);
        bombSwitch = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤー、床、壁オブジェクトにあたると放電
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall"))
        {
            if (bombSwitch)
            {
                StartCoroutine(Discharge());
            }
        }
        //放電に当たると連鎖する
        else if (collision.gameObject.CompareTag("Discharge"))
        {
            //連鎖元のバッテリーの所有者を特定
            var playerNum = collision.gameObject.transform.parent.GetComponent<EnergyBatteryScript>().OwnerCheck();
            ownerNum = playerNum;

            StartCoroutine(Discharge());//放電
        }
    }

    /// <summary>
    /// 放電処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Discharge()
    {
        //重複防止
        if (isDischarge)
        {
            yield break;
        }
        else
        {
            isDischarge = true;
        }

        batteryRB.velocity = Vector3.zero;//移動の慣性をリセット
        batteryRB.angularVelocity = Vector3.zero;//回転の慣性をリセット

        //放電範囲を表示
        dischargeObj.SetActive(true);
        bombSwitch = false;
        yield return new WaitForSeconds(1);
        dischargeObj.SetActive(false);
        isDischarge = false;

        StartCoroutine(Respawn());//リスポーン
    }

    /// <summary>
    /// リスポーン処理
    /// リスポーン場所に指定されたオブジェクトからランダムに選択してリスポーン
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawn()
    {
        ownerNum = 0;
        batteryRB.isKinematic = true;

        var selectObj = respawnObj[Random.Range(0, respawnObj.Length)];
        transform.position = selectObj.transform.position;
        transform.rotation = selectObj.transform.rotation;

        yield return new WaitForSeconds(2);

        batteryRB.isKinematic = false;
        batteryRB.AddForce(selectObj.transform.forward * throwPower / 2, ForceMode.Impulse);
    }
}