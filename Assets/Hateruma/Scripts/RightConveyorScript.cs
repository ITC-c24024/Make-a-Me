using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightConveyorScript : MonoBehaviour
{
    [SerializeField, Header("開始位置")]
    Vector3 startPos;

    [SerializeField, Header("終了位置")]
    Vector3 finishPos;

    [SerializeField, Header("クローンオブジェクト")]
    List<ScrollObjScript> cloneSC;

    List<ScrollObjScript> unUsedCloneSC = new List<ScrollObjScript>(12);

    List<ScrollObjScript> usedCloneSC = new List<ScrollObjScript>(12);

    [SerializeField, Header("スクロール速度")]
    float scrollSpeed;

    bool isScroll;

    void Start()
    {
        foreach (var sc in cloneSC)
        {
            unUsedCloneSC.Add(sc);
        }

        foreach (var sc in unUsedCloneSC)
        {
            sc.rightConveyorSC = this;
        }

        isScroll = true;
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        while (isScroll)
        {
            var clone = unUsedCloneSC[Random.Range(0, unUsedCloneSC.Count)];

            unUsedCloneSC.Remove(clone);
            usedCloneSC.Add(clone);

            StartCoroutine(clone.Move(startPos, finishPos, scrollSpeed));

            yield return new WaitForSeconds(1f);
        }
    }

    public void AddClone(ScrollObjScript scrollObj)
    {
        unUsedCloneSC.Add(scrollObj);
        usedCloneSC.Remove(scrollObj);
    }
}
