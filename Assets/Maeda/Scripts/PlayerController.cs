using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActionScript
{
    [SerializeField]
    TakeRange takeRangeSC;
    [SerializeField]
    CatchRange catchRangeSC;
    EnergyBatteryScript batteryScript;
    //エネルギー管理スクリプト
    EnergyScript energyScript;

    //プレイヤーの番号
    public int playerNum = 0;
    
    [SerializeField,Header("プレイヤーの移動速度")]
    float MoveSpeed = 1.0f;   

    //バッテリーの所持判定
    public bool haveBattery = false;

    [SerializeField, Header("スタン効果時間")]
    float stanTime = 2.0f;
    //スタン判定
    bool isStan = false;

    void Start()
    {
        energyScript = GetComponent<EnergyScript>();
    }
    
    void Update()
    {
        if (!isStan)
        {
            //入力値をVector2型で取得
            Vector2 move = moveAction.ReadValue<Vector2>();

            //プレイヤーを移動
            transform.position += new Vector3(move.x, 0, move.y) * MoveSpeed * Time.deltaTime;

            if (move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1)
            {
                //スティックの角度を計算
                float angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
                //プレイヤーを回転
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }      

        //ボタンを押した判定
        var throwAct = throwAction.triggered;
        if (haveBattery && throwAct && !isTimer && !isStan)
        {
            ChangeHaveBattery();
            StartCoroutine(takeRangeSC.PickupDelay());
            StartCoroutine(catchRangeSC.PickupDelay());
            
            batteryScript.Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Discharge"))
        {
            StartCoroutine(Stan());
        }
    }

    /// <summary>
    /// バッテリースクリプトを指定
    /// </summary>
    /// <param name="batterySC">TakeRangeで取ったバッテリーのスクリプト</param>
    public void ChangeBatterySC(EnergyBatteryScript batterySC)
    {
        ChangeHaveBattery();
        batteryScript = batterySC;

        StartCoroutine(PickupDelay());
    }

    /// <summary>
    /// バッテリー所持判定を切り替え
    /// チャージ判定を切り替え
    /// </summary>
    public void ChangeHaveBattery()
    {
        haveBattery = !haveBattery;

        energyScript.ChargeSwitch(haveBattery);
    }

    /// <summary>
    /// スタン処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Stan()
    {
        isStan = true;
        ChangeHaveBattery();
        yield return new WaitForSeconds(stanTime);
        isStan = false;
    }
}