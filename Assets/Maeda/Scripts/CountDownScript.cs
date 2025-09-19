using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownScript : MonoBehaviour
{
    public float timer = 3;
    [SerializeField] GameObject countdownImageObject;
    [SerializeField] Image countdownImage;
    [SerializeField] Sprite[] newSprite;
    [SerializeField] Image startImage;

    AnimationCurve animationCurve;
    float scaleChangeTime = 1f;
    float startScaleChageTime = 0.3f;
    Vector3 originalScale;
    Vector3 targetScale;
    bool isTimer = false;
    bool isScaling = false; //�d�����ăR���[�`�����Ă΂Ȃ��悤�ɂ���
    bool isStart = true;//�X�^�[�g�ƃt�B�j�b�V���̐؂�ւ�

    void Start()
    {  
        animationCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f)
            );

        countdownImageObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);//�X�P�[���A�b�v�̃A�j���[�V���������邽�߂ɃX�P�[��������������
        startImage.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);//�X�P�[���A�b�v�̃A�j���[�V���������邽�߂ɃX�P�[��������������
        originalScale = countdownImageObject.transform.localScale;//�ŏ��̃X�P�[����ۑ�
        targetScale = new Vector3(1, 1, 1);//���̃X�P�[���Ɍ������đ傫������

        CountSatrt();//�J�E���g�_�E�����X�^�[�g
    }

    void Update()
    {
        if (isTimer)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            //�J�E���g�_�E���̏���
            if (timer < 2)
            {
                countdownImage.sprite = newSprite[1];
                if (!isScaling) StartCoroutine(NumberScaleChange());
            }
            if (timer < 1)
            {
                countdownImage.sprite = newSprite[2];
                if (!isScaling) StartCoroutine(NumberScaleChange());
            }
            if (timer < 0)
            {
                countdownImageObject.SetActive(false);
                startImage.enabled = true;
                
                StartCoroutine(StartScaleUp());
                isTimer = false;
                timer = 3;
            }
            else
            {
                startImage.enabled = false;
            }
        }    
    }

    public void CountSatrt()
    {
        countdownImage.sprite = newSprite[0];
        countdownImageObject.SetActive(true);
        isTimer = true;
        StartCoroutine(NumberScaleChange());
    }
    //�J�E���g�_�E���̃X�P�[���`�F���W
    IEnumerator NumberScaleChange()
    {      
        isScaling = true; // �R���[�`���̏d�����s��h��
        float time = 0f;

        while (time < scaleChangeTime)
        {
            time += Time.deltaTime;
            float t = time / scaleChangeTime;
            float scaleFactor = animationCurve.Evaluate(t);//�A�j���[�V����curve�ɒl����
            countdownImageObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleFactor);
            yield return null;
        }

        countdownImageObject.transform.localScale = originalScale;
        isScaling = false;
    }

    IEnumerator StartScaleUp()
    {
        float timer = 0f;
        while (timer < startScaleChageTime)
        {
            timer += Time.deltaTime;
            float scaleChangeTime = timer / startScaleChageTime;
            startImage.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleChangeTime);

            yield return null;
        }
        startImage.transform.localScale = targetScale;//�X�P�[����ۑ�

        yield return new WaitForSeconds(0.5f);//0.5�b�҂��ĉ��̏��������s

        startImage.enabled = false;
        startImage.sprite = newSprite[3];
    }
}
