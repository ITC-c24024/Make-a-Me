using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    TimerScript timerScript;
    [SerializeField]
    ShutterScript shutterScript;

    void Start()
    {
        Application.targetFrameRate = 60;

        timerScript = this.GetComponent<TimerScript>();

        Invoke("GameStart", 1.0f);
    }

    void GameStart()
    {
        StartCoroutine(shutterScript.OpenShutter());
        StartCoroutine(timerScript.Timer());
    }

    public void GameFinish()
    {
        SceneManager.LoadScene("ResultScene");
    }
}
