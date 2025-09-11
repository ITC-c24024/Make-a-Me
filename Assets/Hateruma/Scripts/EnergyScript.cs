using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScript : MonoBehaviour
{
    [SerializeField, Header("プレイヤー番号")]
    int playerNum;

    // レベル
    public int level = 1;

    // 最大レベル
    int maxLevel = 5;

    [SerializeField, Header("総取得エネルギー")]
    int allEnergyAmount;

    [SerializeField, Header("レベルごとの取得エネルギ―量")]
    int energyAmount;

    [SerializeField, Header("レベルアップに必要なエネルギー量")]
    int requireEnergy = 100;

    [SerializeField, Header("チャージ中")]
    bool isCharge;

    // チャージ用タイマー
    float timer = 0;

    // ドロップマネージャースクリプト
    DropEnergyManagerScript dropManagerSC;

    [SerializeField, Header("エネルギー量表示用UI")]
    GameObject energyUIObj;

    [SerializeField, Header("エネルギー量表示用スライダー")]
    Slider[] energySlider;

    [SerializeField, Header("ハンマーイメージ")]
    Image hammerImage;

    [SerializeField, Header("ハンマーのスプライト")]
    Sprite[] hammerSprite;

    [SerializeField, Header("レベルのImage")]
    Image[] levelImage;

    [SerializeField, Header("レベルのSprite(非常時)")]
    Sprite[] levelSprite1;

    [SerializeField, Header("レベルのSprite(常時)")]
    Sprite[] levelSprite2;

    [SerializeField, Header("スライダー表示時間(秒)")]
    float sliderDisplayTime = 3f;

    // スライダー表示コルーチン用
    Coroutine showRoutine;

    // UIの座標
    Transform uiPos;

    // カメラ
    Camera mainCam;

    [SerializeField, Header("ハンマーオブジェクト")]
    GameObject[] hammerObj;

    private void Start()
    {
        dropManagerSC = gameObject.GetComponent<DropEnergyManagerScript>();

        energySlider[0].maxValue = requireEnergy;
        energySlider[1].maxValue = requireEnergy;

        uiPos = energyUIObj.transform;

        mainCam = Camera.main;
    }

    void Update()
    {
        if (isCharge)
        {
            // 1秒ごとに5ずつチャージされる
            timer += Time.deltaTime * 10;
            if (timer >= 1f)
            {
                ChargeEnergy(1);
                timer = 0f;
            }
        }

        if (uiPos != null)
        {
            // プレイヤーの頭上に追従
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 2));
            uiPos.position = screenPos;
        }
    }

    /// <summary>
    /// エネルギーのチャージ
    /// </summary>
    public void ChargeEnergy(int amount)
    {

        if(allEnergyAmount < 700)
        {
            allEnergyAmount += amount;
        }

        while (amount + energyAmount >= requireEnergy)
        {
            amount -= requireEnergy - energyAmount;
            LevelUp();
        }

        if (amount > 0)
        {
            energyAmount += amount;
            energySlider[0].value = energyAmount;
            energySlider[1].value = energyAmount;
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
                energySlider[0].value = energyAmount;
                energySlider[1].value = energyAmount;
            }
        }

        ShowSlider();
    }

    /// <summary>
    /// レベルアップ用
    /// </summary>
    void LevelUp()
    {
        level++;
        level = Mathf.Clamp(level, 1, maxLevel);

        hammerObj[level - 2].SetActive(false);
        hammerObj[level - 1].SetActive(true);
        hammerImage.sprite = hammerSprite[level - 1];

        if (level < maxLevel)
        {
            requireEnergy += 50;
            energyAmount = 0;

            energySlider[0].maxValue += 50;
            energySlider[1].maxValue += 50;
            energySlider[0].value = 0;
            energySlider[1].value = 0;
        }

        levelImage[0].sprite = levelSprite1[level - 1];
        levelImage[1].sprite = levelSprite2[level - 1];
    }

    /// <summary>
    /// レベルダウン用
    /// </summary>
    void LevelDown()
    {
        level--;
        level = Mathf.Clamp(level, 1, maxLevel);

        hammerObj[level].SetActive(false);
        hammerObj[level - 1].SetActive(true);
        hammerImage.sprite = hammerSprite[level - 1];

        if (level > 1)
        {
            requireEnergy -= 50;
            energyAmount = requireEnergy;

            energySlider[0].maxValue -= 50;
            energySlider[1].maxValue -= 50;
            energySlider[0].value = requireEnergy;
            energySlider[1].value = requireEnergy;
        }

        levelImage[0].sprite = levelSprite1[level - 1];
        levelImage[1].sprite = levelSprite2[level - 1];
    }

    /// <summary>
    /// エネルギーチャージ中かどうかの切り替え用
    /// </summary>
    public void ChargeSwitch(bool charge)
    {
        isCharge = charge;
    }

    void ShowSlider()
    {
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
            showRoutine = null;
        }

        showRoutine = StartCoroutine(ShowSliderRoutine());
    }

    IEnumerator ShowSliderRoutine()
    {
        energyUIObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(sliderDisplayTime);

        energyUIObj.gameObject.SetActive(false);
        showRoutine = null;
    }
}

