using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField,Header("タイトルBGM")]
    AudioSource titleSource;
    [SerializeField, Header("メインBGM")]
    AudioSource mainSource;
    [SerializeField, Header("シャッターSE")]
    AudioSource shutterSource;
    [SerializeField, Header("選択SE")]
    AudioSource selectSource;
    [SerializeField, Header("決定SE")]
    AudioSource dicideSource;
    [SerializeField, Header("作業SE")]
    AudioSource workSource;
    [SerializeField, Header("放電SE")]
    AudioSource dischargeSource;
    [SerializeField, Header("レベルアップSE")]
    AudioSource levelUpSource;

    [SerializeField, Header("タイトルBGM")]
    AudioClip titleClip;
    [SerializeField, Header("メインBGM")]
    AudioClip mainClip;
    [SerializeField, Header("シャッターSE")]
    AudioClip shutterClip;
    [SerializeField, Header("選択SE")]
    AudioClip selectClip;
    [SerializeField, Header("決定SE")]
    AudioClip dicideClip;
    [SerializeField, Header("作業SE")]
    AudioClip workClip;
    [SerializeField, Header("放電SE")]
    AudioClip dischargeClip;
    [SerializeField, Header("レベルアップSE")]
    AudioClip levelUpClip;

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
}