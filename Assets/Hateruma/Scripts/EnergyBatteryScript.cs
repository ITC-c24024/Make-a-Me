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

    Rigidbody batteryRB;//CoreオブジェクトのRigidbody

    [SerializeField, Header("放電可能かどうかのスイッチ")]
    bool bombSwitch;

    bool isDischarge;//放電重複しないようにするフラグ

    void Start()
    {
        batteryRB = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (ownerObj != null)
        {
            batteryRB.MovePosition(ownerObj.transform.position + Vector3.up);
            batteryRB.MoveRotation(ownerObj.transform.rotation);
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
        else if (collision.gameObject.CompareTag("Discharge"))
        {
            var playerNum = collision.gameObject.transform.parent.GetComponent<EnergyBatteryScript>().OwnerCheck();
            ownerNum = playerNum;
            StartCoroutine(Discharge());
        }
    }

    /// <summary>
    /// 放電処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Discharge()
    {
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

        dischargeObj.SetActive(true);
        bombSwitch = false;
        yield return new WaitForSeconds(1);
        dischargeObj.SetActive(false);
        isDischarge = false;

        StartCoroutine(Respawn());
    }


    IEnumerator Respawn()
    {
        batteryRB.isKinematic = true;

        var selectObj = respawnObj[Random.Range(0, respawnObj.Length)];
        transform.position = selectObj.transform.position;
        transform.rotation = selectObj.transform.rotation;

        yield return new WaitForSeconds(2);

        batteryRB.isKinematic = false;
        batteryRB.AddForce(selectObj.transform.forward * throwPower / 2, ForceMode.Impulse);
    }
}