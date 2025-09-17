using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class UIManagerScript : MonoBehaviour
{
    //�I��p
    public InputAction selectAction;

    //����p
    public InputAction decisionAction;

    public bool isCoolTime;
    private void Awake()
    {
        //ActionMap���擾
        var input = gameObject.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        //�Ή�����A�N�V�������擾
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
