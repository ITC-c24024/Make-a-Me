using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : ActionScript
{
    [SerializeField]
    ScoreScript scoreScript;
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
    public bool isStun = false;
    [SerializeField, Header("無敵時間")]
    float invincibleTime = 2.0f;
    //無敵判定
    bool invincible = false;

    Rigidbody playerRB;

    Animator animator;

    void Start()
    {
        energyScript = GetComponent<EnergyScript>();
        playerRB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!isStun)
        {
            //入力値をVector2型で取得
            Vector2 move = moveAction.ReadValue<Vector2>();

            if(move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1)
            {
                animator.SetBool("Iswalk", true);

                //プレイヤーを移動
                transform.position += new Vector3(move.x, 0, move.y) * MoveSpeed * Time.deltaTime;
            }
            else
            {
                animator.SetBool("Iswalk", false);
            }

            if ((move.x > 0.1 || move.x < -0.1 || move.y > 0.1 || move.y < -0.1) && !scoreScript.isWork)
            {
                //スティックの角度を計算
                float angle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
                //プレイヤーを回転
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
        else animator.SetBool("Iswalk", false);

        //ボタンを押した判定
        var throwAct = throwAction.triggered;
        if (haveBattery && throwAct && !isTimer && !isStun)
        {
            ChangeHaveBattery(false);
            StartCoroutine(takeRangeSC.PickupDelay());
            StartCoroutine(catchRangeSC.PickupDelay());
            
            batteryScript.Throw();
            animator.SetBool("IsThrow", true);
            animator.SetBool("IsThrow", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Discharge") && !isStun)
        {
            StartCoroutine(Stan());
        }
        if (other.CompareTag($"WorkArea{playerNum}") && !isStun)
        {          
            if (batteryScript != null)
            {
                ChangeHaveBattery(false);
                batteryScript.Drop();
            }
        }
    }

    /// <summary>
    /// バッテリースクリプトを指定
    /// </summary>
    /// <param name="batterySC">TakeRangeで取ったバッテリーのスクリプト</param>
    public void ChangeBatterySC(EnergyBatteryScript batterySC)
    {
        ChangeHaveBattery(true);
        batteryScript = batterySC;

        StartCoroutine(PickupDelay());
    }

    /// <summary>
    /// バッテリー所持判定を切り替え
    /// チャージ判定を切り替え
    /// </summary>
    /// <param name="have">所持判定に代入するbool</param>
    public void ChangeHaveBattery(bool have)
    {
        haveBattery = have;
        animator.SetBool("IsHave", haveBattery);

        energyScript.ChargeSwitch(haveBattery);
    }

    /// <summary>
    /// スタン処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Stan()
    {
        //エネルギードロップ
        energyScript.LostEnergy();

        invincible = true;
        if (scoreScript.isWork) JobAnim(false);
        isStun = true;
        animator.SetBool("Isstun", true);
        
        ChangeHaveBattery(false);

        yield return new WaitForSeconds(stanTime);

        isStun = false;
        if (scoreScript.isWork) JobAnim(true);   
        animator.SetBool("Isstun", false);
        
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);

        Invoke("ResetInvincible", invincibleTime);
    }

    /// <summary>
    /// 無敵状態リセット
    /// </summary>
    void ResetInvincible()
    {
        invincible = false;
    }

    public void JobAnim(bool isWalk)
    {
        animator.SetBool("IsJob", isWalk);
    }
}