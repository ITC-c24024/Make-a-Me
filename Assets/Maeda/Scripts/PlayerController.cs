using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    EnergyBatteryScript batteryScript;

    //プレイヤーの番号
    public int playerNum = 0;
    
    [SerializeField,Header("プレイヤーの移動速度")]
    float MoveSpeed = 1.0f;

    //入力タイマー
    float timer = 0;
    //入力制限
    public bool input = false;

    //バッテリーの所持判定
    public bool haveBattery = false;

    //移動アクション
    InputAction moveAction;
    //取る、投げるアクション
    InputAction throwAction;

    private void Awake()
    {
        //ActionMapを取得
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //対応するアクションを取得
        moveAction = actionMap["Move"];
        throwAction = actionMap["Throw"];
    }

    void Start()
    {
        var gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.SelectPrefab(playerNum);
    }
    
    void Update()
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

        //ボタンを押した判定
        var throwAct = throwAction.triggered;
        if (haveBattery && throwAct)
        {
            input = true;

            haveBattery = false;
            Debug.Log("throw");
            batteryScript.Throw();
        }

        if (input)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                timer = 0;
                input = false;
            }
        }
    }

    /// <summary>
    /// バッテリースクリプトを指定
    /// </summary>
    /// <param name="batterySC">TakeRangeで取ったバッテリーのスクリプト</param>
    public void ChangeBatterySC(EnergyBatteryScript batterySC)
    {
        haveBattery = true;
        batteryScript = batterySC;
    }
}