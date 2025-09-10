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

    //バッテリーオブジェクトのRigidbody
    Rigidbody batteryRB;

    //放電可能かどうかのスイッチ
    public bool bombSwitch;

    //放電重複しないようにするフラグ
    bool isDischarge;

    void Start()
    {
        batteryRB = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (ownerObj != null)
        {
            transform.position = ownerObj.transform.position + new Vector3(0,0);
            transform.rotation = ownerObj.transform.rotation * Quaternion.Euler(0, 90, 90);
        }
    }


    /// <summary>
    /// 所持者を登録・変更
    /// </summary>
    /// <param name="num">プレイヤーナンバー</param>
    /// <param name="player">プレイヤーのオブジェクト</param>
    public bool ChangeOwner(int num, GameObject player)
    {
        if (isDischarge) return false;
        
        ownerNum = num;
        ownerObj = player;
        batteryRB.isKinematic = true;

        bombSwitch = false;

        return true;
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
        var ownerForward = Quaternion.AngleAxis(-5, ownerObj.transform.right) * ownerObj.transform.forward;

        ownerObj = null;
        batteryRB.isKinematic = false;

        batteryRB.velocity = Vector3.zero;
        batteryRB.angularVelocity = Vector3.zero;

        batteryRB.AddForce(ownerForward * throwPower,ForceMode.Impulse);
        bombSwitch = true;
    }

    /// <summary>
    /// 作業場に入った時のバッテリードロップ処理
    /// </summary>
    public void Drop()
    {
        ownerObj = null;
        batteryRB.isKinematic = false;

        batteryRB.velocity = Vector3.zero;
        batteryRB.angularVelocity = Vector3.zero;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤー、床、壁オブジェクトにあたると放電
        if (collision.gameObject.tag.StartsWith("P") ||
            collision.gameObject.CompareTag("Floor") || 
            collision.gameObject.CompareTag("Wall")|| 
            collision.gameObject.CompareTag("Battery"))
        {
            if (bombSwitch)
            {
                StartCoroutine(Discharge());
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //放電に当たると連鎖する
        if (other.gameObject.CompareTag("Discharge"))
        {
            //連鎖元のバッテリーの所有者を特定
            var playerNum = other.gameObject.transform.parent.GetComponent<EnergyBatteryScript>().OwnerCheck();
            ownerNum = playerNum;//ホーミング処理の名残り

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

        batteryRB.isKinematic = true;

        ownerObj = null;

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


        var selectObj = respawnObj[Random.Range(0, respawnObj.Length)];
        transform.position = selectObj.transform.position;
        transform.rotation = selectObj.transform.rotation * Quaternion.Euler(0,90,0);

        yield return new WaitForSeconds(2);

        batteryRB.isKinematic = false;
        batteryRB.AddForce(selectObj.transform.forward * throwPower / 2,ForceMode.Impulse);
    }
}