using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScript : MonoBehaviour
{
    [SerializeField,Header("プレイヤー番号")] 
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

    //プレイヤーコントローラースクリプト
    PlayerController playerControllerSC;

    [SerializeField] Text levelText;
    [SerializeField] Text energyText;
    [SerializeField] Text requireEnergyText;

    private void Start()
    {
        dropManagerSC = gameObject.GetComponent<DropEnergyManagerScript>();
        playerControllerSC = gameObject.GetComponent<PlayerController>();
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

        energyText.text = $"energy:{energyAmount}";
        requireEnergyText.text = $"require:{requireEnergy}";
    }

    /// <summary>
    /// エネルギーのチャージ
    /// </summary>
    /// <param name="amount">取得量</param>
    public void ChargeEnergy(int amount)
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
        if(allEnergyAmount > 0)
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
            }
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

        levelText.text = $"level:{level}";
    }

    /// <summary>
    /// レベルダウン用
    /// </summary>
    void LevelDown()
    {
        level--;
        requireEnergy -= 50;
        energyAmount = requireEnergy;

        levelText.text = $"level:{level}";
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
            if (!playerControllerSC.isStun)
            {
                LostEnergy();
            }
        }
    }
}
