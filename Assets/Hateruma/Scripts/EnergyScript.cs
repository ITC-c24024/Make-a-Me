using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScript : MonoBehaviour
{
    public int level = 1;//レベル
    int maxLevel = 5;//最大レベル

    [SerializeField, Header("総取得エネルギー")]
    int allEnergyAmount;

    [SerializeField, Header("レベルごとの取得エネルギ―量")]
    int energyAmount;

    [SerializeField, Header("レベルアップに必要なエネルギー量")]
    int requireEnergy = 100;

    [SerializeField, Header("チャージ中")]
    bool isCharge;

    float timer = 0;//チャージ用タイマー

    [SerializeField] Text levelText;
    [SerializeField] Text energyText;
    [SerializeField] Text requireEnergyText;
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

        energyText.text = $"{energyAmount}";
        requireEnergyText.text = $"{requireEnergy}";
    }

    /// <summary>
    /// エネルギーのチャージ
    /// </summary>
    /// <param name="amount">取得量</param>
    void ChargeEnergy(int amount)
    {
        if(level < maxLevel)
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
            }
        }
    }

    /// <summary>
    /// エネルギーのドロップ(総量の1/3)
    /// </summary>
    void LostEnergy()
    {
        var amount = allEnergyAmount / 3;
        allEnergyAmount -= amount;

        while (amount > energyAmount)
        {
            amount -= energyAmount;
            LevelDown();
        }

        if(amount > 0)
        {
            energyAmount -= amount;
        }
    }

    /// <summary>
    /// レベルアップ用
    /// </summary>
    void LevelUp()
    {
        level++;
        requireEnergy += 50;
        energyAmount = 0;

        levelText.text = $"{level}";
    }

    /// <summary>
    /// レベルダウン用
    /// </summary>
    void LevelDown()
    {
        level--;
        requireEnergy -= 50;
        energyAmount = requireEnergy;

        levelText.text = $"{level}";
    }

    /// <summary>
    /// エネルギーチャージ中かどうかの切り替え用
    /// </summary>
    /// <param name="charge">ON/OFF指定</param>
    public void ChargeSwitch(bool charge)
    {
        isCharge = charge;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Discharge"))
        {
            LostEnergy();
        }
    }
}
