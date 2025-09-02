using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyScript : MonoBehaviour
{
    public int level = 1;
    int maxLevel = 10;
    [SerializeField, Header("総取得エネルギー")] int allEnergyAmount;
    [SerializeField,Header("レベルごとの取得エネルギ―量")] int energyAmount;
    [SerializeField,Header("レベルアップに必要なエネルギー量")] int requireEnergy = 100;

    bool isCharge;

    float timer = 0;
    void Update()
    {
        if (isCharge)
        {
            timer += Time.deltaTime * 10;

            if (timer >= 1)
            {
                ChargeEnergy(1);
                timer = 0;
            }
            
        }
    }

    void ChargeEnergy(int amount)
    {
        energyAmount += amount;
        allEnergyAmount += amount;

        if (energyAmount >= requireEnergy && level < maxLevel)
        {
            energyAmount = 0;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        requireEnergy += 50;
    }

    public void ChargeSwitch()
    {
        isCharge = !isCharge;
    }
}
