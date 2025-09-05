using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScript : MonoBehaviour
{
    [SerializeField,Header("�v���C���[�ԍ�")] 
    int playerNum;

    //���x��
    public int level = 1;

    //�ő僌�x��
    int maxLevel = 5;

    [SerializeField, Header("���擾�G�l���M�[")]
    int allEnergyAmount;

    [SerializeField, Header("���x�����Ƃ̎擾�G�l���M�\��")]
    int energyAmount;

    [SerializeField, Header("���x���A�b�v�ɕK�v�ȃG�l���M�[��")]
    int requireEnergy = 100;

    [SerializeField, Header("�`���[�W��")]
    bool isCharge;

    //�`���[�W�p�^�C�}�[
    float timer = 0;

    //�h���b�v�}�l�[�W���[�X�N���v�g
    DropEnergyManagerScript dropManagerSC;

    //�v���C���[�R���g���[���[�X�N���v�g
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
            //�P�b���ƂɂT���`���[�W�����
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
    /// �G�l���M�[�̃`���[�W
    /// </summary>
    /// <param name="amount">�擾��</param>
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
    /// �G�l���M�[�̃h���b�v(���ʂ�1/3)
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
    /// ���x���A�b�v�p
    /// </summary>
    void LevelUp()
    {
        level++;
        requireEnergy += 50;
        energyAmount = 0;

        levelText.text = $"level:{level}";
    }

    /// <summary>
    /// ���x���_�E���p
    /// </summary>
    void LevelDown()
    {
        level--;
        requireEnergy -= 50;
        energyAmount = requireEnergy;

        levelText.text = $"level:{level}";
    }

    /// <summary>
    /// �G�l���M�[�`���[�W�����ǂ����̐؂�ւ��p
    /// </summary>
    /// <param name="charge">ON/OFF�w��</param>
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
