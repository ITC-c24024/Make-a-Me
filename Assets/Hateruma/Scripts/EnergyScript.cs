using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScript : MonoBehaviour
{
    [SerializeField, Header("プレイヤー番号")]
    int playerNum;

    //レベル
    public int level = 1;

    //最大レベル
    int maxLevel = 5;

    [SerializeField, Header("総取得エネルギー")]
    int allEnergyAmount;

    [SerializeField, Header("レベルごとの取得エネルギ―量")]
    int energyAmount;

    [SerializeField, Header("レベルアップに必要なエネルギー量")]
    int requireEnergy = 100;

    [SerializeField, Header("チャージ中")]
    bool isCharge;

    //チャージ用タイマー
    float timer = 0;

    //ドロップマネージャースクリプト
    DropEnergyManagerScript dropManagerSC;

    [SerializeField, Header("エネルギー量表示用UI")]
    GameObject energyUIObj;

    [SerializeField, Header("エネルギー量表示用スライダー")]
    Slider energySlider;

    [SerializeField, Header("スライダー表示時間(秒)")]
    float sliderDisplayTime = 3f;

    //スライダー表示コルーチン用
    Coroutine showRoutine;

    //スライダーの座標
    Transform sliderPos;

    //カメラ
    Camera mainCam;

    [SerializeField, Header("ハンマーオブジェクト")]
    GameObject[] hammerObj;
    private void Start()
    {
        dropManagerSC = gameObject.GetComponent<DropEnergyManagerScript>();

        energySlider.maxValue = requireEnergy;

        sliderPos = energySlider.transform;

        mainCam = Camera.main;
    }
    void Update()
    {
        if (isCharge)
        {
            //１秒ごとに５ずつチャージされる
            timer += Time.deltaTime * 5;

            if (timer >= 1)
            {
                ChargeEnergy(1);
                timer = 0;
            }
        }

        if (sliderPos != null)
        {
            // プレイヤーの頭上に追従
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position + new Vector3(0, 2.5f, 0));
            sliderPos.position = screenPos;

            // カメラの方向を向く（ビルボード）
            sliderPos.forward = mainCam.transform.forward;
        }
    }

    /// <summary>
    /// エネルギーのチャージ
    /// </summary>
    /// <param name="amount">取得量</param>
    public void ChargeEnergy(int amount)
    {
        if (level < maxLevel)
        {
            allEnergyAmount += amount;

            while (amount + energyAmount >= requireEnergy)
            {
                amount -= requireEnergy - energyAmount;
                LevelUp();
            }

            if (amount > 0)
            {
                energyAmount += amount;
                energySlider.value = energyAmount;
            }
        }

        ShowSlider();
    }

    /// <summary>
    /// エネルギーのドロップ(総量の1/3)
    /// </summary>
    public void LostEnergy()
    {
        if (allEnergyAmount > 0)
        {
            var amount = allEnergyAmount / 3;

            dropManagerSC.Drop(amount);

            allEnergyAmount -= amount;

            while (amount > energyAmount)
            {
                amount -= energyAmount;
                LevelDown();
            }

            if (amount > 0)
            {
                energyAmount -= amount;
                energySlider.value = energyAmount;
            }
        }

        ShowSlider();
    }

    /// <summary>
    /// レベルアップ用
    /// </summary>
    void LevelUp()
    {
        hammerObj[level - 1].SetActive(false);
        hammerObj[level].SetActive(true);

        level++;
        requireEnergy += 50;
        energyAmount = 0;

        energySlider.maxValue += 50;
        energySlider.value = 0;
    }

    /// <summary>
    /// レベルダウン用
    /// </summary>
    void LevelDown()
    {
        hammerObj[level - 1].SetActive(false);
        hammerObj[level - 2].SetActive(true);

        level--;
        requireEnergy -= 50;
        energyAmount = requireEnergy;

        energySlider.maxValue -= 50;
        energySlider.value = requireEnergy;
    }

    /// <summary>
    /// エネルギーチャージ中かどうかの切り替え用
    /// </summary>
    /// <param name="charge">ON/OFF指定</param>
    public void ChargeSwitch(bool charge)
    {
        isCharge = charge;
    }

    void ShowSlider()
    {
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
        }

        showRoutine = StartCoroutine(ShowSliderRoutine());
    }

    IEnumerator ShowSliderRoutine()
    {
        energySlider.gameObject.SetActive(true);

        yield return new WaitForSeconds(sliderDisplayTime);

        energySlider.gameObject.SetActive(false);
        showRoutine = null;
    }
}
