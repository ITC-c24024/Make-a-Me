using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispencerScript : MonoBehaviour
{
    [SerializeField, Header("開始時間")]
    float startTime;

    [SerializeField, Header("発射オブジェクト")]
    GameObject shotObj;

    [SerializeField, Header("クローンの色")]
    string cloneColor;

    [SerializeField] List<GameObject> cloneObj; // SerializeField を残す
    List<Rigidbody> cloneRB;

    public int cloneCount;

    private void Awake()
    {
        if (cloneObj == null) cloneObj = new List<GameObject>();
        if (cloneRB == null) cloneRB = new List<Rigidbody>();

        GameObject[] clones = GameObject.FindGameObjectsWithTag($"ResultClone{cloneColor}");
        cloneObj.AddRange(clones);
    }


    void Start()
    {
        foreach (var obj in cloneObj)
        {
            cloneRB.Add(obj.GetComponent<Rigidbody>());
        }
        StartCoroutine(ShotClone());
    }

    IEnumerator ShotClone()
    {
        var count = 0.25f;

        yield return new WaitForSeconds(startTime);

        while (cloneCount > 0)
        {
            shotObj.transform.localEulerAngles = new Vector3(
                Random.Range(60f, 120f),
                Random.Range(-30f, 30f)
            );

            var clone = cloneObj[0];
            var rb = cloneRB[0];

            clone.transform.position = shotObj.transform.position;

            rb.isKinematic = false;
            rb.AddForce(shotObj.transform.forward * 10, ForceMode.Impulse);

            cloneObj.RemoveAt(0);
            cloneRB.RemoveAt(0);

            cloneCount--;

            if (count > 0.1f)
            {
                count -= 0.005f;
            }

            yield return new WaitForSeconds(count);
        }
    }
}
