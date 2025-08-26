using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyBatteryScript : MonoBehaviour
{
    [SerializeField,Header("所持しているプレイヤーの番号")]
    int ownerNum;
    
    [SerializeField,Header("所持しているプレイヤーのオブジェクト")]
    GameObject ownerObj;

    Rigidbody coreRB;//CoreオブジェクトのRigidbody

    [SerializeField,Header("放電可能かどうかのスイッチ")]
    bool bombSwitch;
    
    bool isDischarge;//放電重複しないようにするフラグ

    [SerializeField,Header("投げる強さ")]
    float throwPower;

    [SerializeField,Header("放電オブジェクト")]
    GameObject dischargeObj;
    void Start()
    {
        coreRB = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (ownerObj != null)
        {
            coreRB.MovePosition(ownerObj.transform.position + Vector3.up);
            coreRB.MoveRotation(ownerObj.transform.rotation);
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
        coreRB.isKinematic = true;
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
        coreRB.isKinematic = false;
        coreRB.AddForce(ownerForward * throwPower,ForceMode.Impulse);
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
        dischargeObj.SetActive(true);
        bombSwitch = false;
        yield return new WaitForSeconds(1);
        dischargeObj.SetActive(false);
        isDischarge = false;
    }
}