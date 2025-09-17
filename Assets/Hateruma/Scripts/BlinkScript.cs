using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    [SerializeField, Header("�q�I�u�W�F�N�g���܂ނ��ǂ���")]
    bool all;

    Coroutine blinkCoroutine;

    GameObject[] childTargets;

    void Start()
    {
        if (all)
        {
            childTargets = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                childTargets[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    public void BlinkStart(int time, float speed, float lastSpeed)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkCount(time, speed, lastSpeed));
    }

    IEnumerator BlinkCount(int time, float speed, float lastSpeed)
    {
        float currentTime = 0f;
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        while (currentTime < time)
        {
            // �e�̓��b�V�������؂�ւ�
            if (mesh != null) mesh.enabled = !mesh.enabled;

            // �q�I�u�W�F�N�g��SetActive�Ő؂�ւ�
            if (all)
            {
                foreach (var child in childTargets)
                {
                    child.SetActive(!child.activeSelf);
                }
            }

            if (time - currentTime <= 2f)
            {
                speed = lastSpeed;
            }

            yield return new WaitForSeconds(speed);
            currentTime += speed;
        }

        // �ŏI�I�ɑS���\��
        if (mesh != null) mesh.enabled = true;
        if (all)
        {
            foreach (var child in childTargets) child.SetActive(true);
        }
    }
    public void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        // �e��\��
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = true;

        // �q�I�u�W�F�N�g���S�\��
        if (all && childTargets != null)
        {
            foreach (var child in childTargets)
            {
                child.SetActive(true);
            }
        }
    }

}

