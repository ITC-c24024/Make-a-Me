using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider seSlider;

    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    AudioMixerGroup bgmGrp;
    [SerializeField]
    AudioMixerGroup seGrp;

    [SerializeField,Header("�^�C�g��BGM")]
    AudioSource titleSource;
    [SerializeField, Header("���C��BGM")]
    AudioSource mainSource;
    [SerializeField, Header("�V���b�^�[SE")]
    AudioSource shutterSource;
    [SerializeField, Header("�I��SE")]
    AudioSource selectSource;
    [SerializeField, Header("����SE")]
    AudioSource dicideSource;
    [SerializeField, Header("���SE")]
    AudioSource workSource;
    [SerializeField, Header("���dSE")]
    AudioSource dischargeSource;
    [SerializeField, Header("���x���A�b�vSE")]
    AudioSource levelUpSource;
    [SerializeField, Header("���x���_�E��SE")]
    AudioSource levelDownSource;
    [SerializeField, Header("�x��SE")]
    AudioSource warningSource;
    [SerializeField, Header("����SE")]
    AudioSource shootSource;
    [SerializeField, Header("���U���gBGM")]
    AudioSource resultSource;
    [SerializeField, Header("���ʕ\��SE")]
    AudioSource rankSource;

    [SerializeField, Header("�^�C�g��BGM")]
    AudioClip titleClip;
    [SerializeField, Header("���C��BGM")]
    AudioClip mainClip;
    [SerializeField, Header("�V���b�^�[SE")]
    AudioClip shutterClip;
    [SerializeField, Header("�I��SE")]
    AudioClip selectClip;
    [SerializeField, Header("����SE")]
    AudioClip dicideClip;
    [SerializeField, Header("���SE")]
    AudioClip workClip;
    [SerializeField, Header("���dSE")]
    AudioClip dischargeClip;
    [SerializeField, Header("���x���A�b�vSE")]
    AudioClip levelUpClip;
    [SerializeField, Header("���x���_�E��SE")]
    AudioClip levelDownClip;
    [SerializeField, Header("�x��SE")]
    AudioClip warningClip;
    [SerializeField, Header("����SE")]
    AudioClip shootClip;
    [SerializeField, Header("���U���gBGM")]
    AudioClip resultClip;
    [SerializeField, Header("���ʕ\��SE")]
    AudioClip rankClip;

    void Start()
    {
        titleSource.outputAudioMixerGroup = bgmGrp;
        mainSource.outputAudioMixerGroup = bgmGrp;
        shutterSource.outputAudioMixerGroup = seGrp;
        selectSource.outputAudioMixerGroup = seGrp;
        dicideSource.outputAudioMixerGroup = seGrp;
        workSource.outputAudioMixerGroup = seGrp;
        dischargeSource.outputAudioMixerGroup = seGrp;
        levelUpSource.outputAudioMixerGroup = seGrp;
        levelDownSource.outputAudioMixerGroup = seGrp;
        warningSource.outputAudioMixerGroup = seGrp;
        shootSource.outputAudioMixerGroup = seGrp;
        resultSource.outputAudioMixerGroup = bgmGrp;
        rankSource.outputAudioMixerGroup = seGrp;
    }

    void Update()
    {       
        if (bgmSlider != null && seSlider != null)
        {
            var bgmvalue = bgmSlider.value;
            var seValue = seSlider.value;
            audioMixer.SetFloat("Param_BGM", bgmvalue);
            audioMixer.SetFloat("Param_SE", seValue);
        }
    }

    public void Title()
    {
        titleSource.PlayOneShot(titleClip);
    }
    public void TitleStop()
    {
        titleSource.Stop();
    } 
    public void Main()
    {
        mainSource.PlayOneShot(mainClip);
    }
    public void MainStop()
    {
        mainSource.Stop();
    }
    public void MainSpeedUp()
    {
        mainSource.pitch = 1.25f;
    }
    public void Shutter()
    {
        shutterSource.PlayOneShot(shutterClip);
    }
    public void Select()
    {
        selectSource.PlayOneShot(selectClip);
    }
    public void Dicide()
    {
        dicideSource.PlayOneShot(dicideClip);
    }
    public void Work()
    {
        workSource.PlayOneShot(workClip);
    }
    public void Discharge()
    {
        dischargeSource.PlayOneShot(dischargeClip);
    }
    public void LevelUp()
    {
        levelUpSource.PlayOneShot(levelUpClip);
    }
    public void LevelDown()
    {
        levelDownSource.PlayOneShot(levelDownClip);
    }
    public void Warning()
    {
        warningSource.PlayOneShot(warningClip);
    }
    public void Shoot()
    {
        shootSource.PlayOneShot(shootClip);
    }
    public void Result()
    {
        resultSource.PlayOneShot(resultClip);
    }
    public void Rank()
    {
        rankSource.PlayOneShot(rankClip);
    }
}