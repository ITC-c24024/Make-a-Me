using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class UIManagerScript : MonoBehaviour
{
    //選択用
    public InputAction selectAction;

    //決定用
    public InputAction decisionAction;

    public bool isCoolTime;
    private void Awake()
    {
        //ActionMapを取得
        var input = gameObject.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //対応するアクションを取得
        selectAction = actionMap["Select"];
        decisionAction = actionMap["Decision"];
    }
    public IEnumerator SelectCoolTime()
    {
        if (isCoolTime)
        {
            yield break;
        }
        else
        {
            isCoolTime = true;
        }

        yield return new WaitForSeconds(0.2f);

        isCoolTime = false;
    }
}
