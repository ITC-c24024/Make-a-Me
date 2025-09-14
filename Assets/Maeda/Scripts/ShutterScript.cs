using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterScript : MonoBehaviour
{
    [SerializeField, Header("�J�ɂ����鎞��")]
    float maxTime = 1.0f;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// �V���b�^�[��߂�
    /// </summary>
    /// <returns></returns>
    public IEnumerator CloseShutter()
    {
        float time = 0;
        
        while (time < maxTime)
        {
            time += Time.deltaTime;
            float currentY = Mathf.Lerp(1080, 0, time / maxTime);

            rectTransform.localPosition = new Vector3(
                rectTransform.localPosition.x,
                currentY,
                rectTransform.localPosition.z
                );

            yield return null;
        }
    }
    /// <summary>
    /// �V���b�^�[���J����
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenShutter()
    {
        float time = 0;

        while (time < maxTime)
        {
            time += Time.deltaTime;
            float currentY = Mathf.Lerp(0, 1080, time / maxTime);

            rectTransform.localPosition = new Vector3(
                rectTransform.localPosition.x,
                currentY,
                rectTransform.localPosition.z
                );

            yield return null;
        }
    }
}
