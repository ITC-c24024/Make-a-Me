using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollObjScript : MonoBehaviour
{
    public RightConveyorScript rightConveyorSC;
    public IEnumerator Move(Vector3 startPos, Vector3 targetPos, float speed)
    {
        transform.position = startPos;

        var posDistance = Vector3.Distance(startPos, targetPos);
        float t = 0f;

        while (t < 1f)
        {
            t += (Time.deltaTime * speed) / posDistance;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        rightConveyorSC.AddClone(this);
    }

}
