using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public float maxTime = 10f;
    [SerializeField] float workTime = 0f;

    [SerializeField] int score = 0;
    [SerializeField] bool isWork = false;

    [SerializeField] int workAreaNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWork)
        {
            workTime += Time.deltaTime;
        }

        if(workTime >= maxTime)
        {
            workTime = 0;
            score += 1;
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isWork = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isWork = false;
        }
    }
}
