using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyScript : MonoBehaviour
{
    public int level = 1;
    int maxLevel = 5;
    [SerializeField, Header("���擾�G�l���M�[")] int allEnergyAmount;
    [SerializeField, Header("���x�����Ƃ̎擾�G�l���M�\��")] int energyAmount;
    [SerializeField, Header("���x���A�b�v�ɕK�v�ȃG�l���M�[��")] int requireEnergy = 100;

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

    /// <summary>
    /// �G�l���M�[�`���[�W�����ǂ����̐؂�ւ��p
    /// </summary>
    /// <param name="charge">������ON/OFF�w��</param>
    public void ChargeSwitch(bool charge)
    {
        isCharge = !isCharge;
    }
}
