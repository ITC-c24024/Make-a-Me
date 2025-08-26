using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    //プレイヤーの番号
    public int playerNum = 0;
    
    [SerializeField,Header("プレイヤーの移動速度")] 
    float MoveSpeed = 1.0f;

    //移動アクション
    public InputAction moveAction;
    //投げるアクション
    public InputAction throwAction;

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
    }
}