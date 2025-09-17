using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShutterScript : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;
    [SerializeField, Header("�J�ɂ����鎞��")]
    float maxTime = 2.0f;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// �V���b�^�[��߂�
    /// </summary>
    /// <returns></returns>
    public IEnumerator CloseShutter()
    {
        audioManager.Shutter();
        yield return new WaitForSeconds(0.2f);

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
        audioManager.Shutter();
        yield return new WaitForSeconds(0.2f);

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
