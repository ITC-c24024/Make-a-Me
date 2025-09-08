using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScript : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�ԍ�")]
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

    [SerializeField, Header("�G�l���M�[�ʕ\���pUI")]
    GameObject energyUIObj;

    [SerializeField, Header("�G�l���M�[�ʕ\���p�X���C�_�[")]
    Slider energySlider;

    [SerializeField, Header("�X���C�_�[�\������(�b)")]
    float sliderDisplayTime = 3f;

    //�X���C�_�[�\���R���[�`���p
    Coroutine showRoutine;

    //�X���C�_�[�̍��W
    Transform sliderPos;

    //�J����
    Camera mainCam;

    [SerializeField, Header("�n���}�[�I�u�W�F�N�g")]
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
            //�P�b���ƂɂT���`���[�W�����
            timer += Time.deltaTime * 5;

            if (timer >= 1)
            {
                ChargeEnergy(1);
                timer = 0;
            }
        }

        if (sliderPos != null)
        {
            // �v���C���[�̓���ɒǏ]
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position + new Vector3(0, 2.5f, 0));
            sliderPos.position = screenPos;

            // �J�����̕����������i�r���{�[�h�j
            sliderPos.forward = mainCam.transform.forward;
        }
    }

    /// <summary>
    /// �G�l���M�[�̃`���[�W
    /// </summary>
    /// <param name="amount">�擾��</param>
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
    /// �G�l���M�[�̃h���b�v(���ʂ�1/3)
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
    /// ���x���A�b�v�p
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
    /// ���x���_�E���p
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
    /// �G�l���M�[�`���[�W�����ǂ����̐؂�ւ��p
    /// </summary>
    /// <param name="charge">ON/OFF�w��</param>
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
