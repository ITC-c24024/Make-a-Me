using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    TimerScript timerScript;

    void Start()
    {
        Application.targetFrameRate = 60;

        timerScript = this.GetComponent<TimerScript>();

        StartCoroutine(timerScript.Timer());
    }
}
