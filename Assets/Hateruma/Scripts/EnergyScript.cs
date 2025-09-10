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
    Slider[] energySlider;

    [SerializeField, Header("���x����Image")]
    Image[] levelImage;

    [SerializeField, Header("���x����Sprite(��펞)")]
    Sprite[] levelSprite1;

    [SerializeField, Header("���x����Sprite(�펞)")]
    Sprite[] levelSprite2;

    [SerializeField, Header("�X���C�_�[�\������(�b)")]
    float sliderDisplayTime = 3f;

    //�X���C�_�[�\���R���[�`���p
    Coroutine showRoutine;

    //UI�̍��W
    Transform uiPos;

    //�J����
    Camera mainCam;

    [SerializeField, Header("�n���}�[�I�u�W�F�N�g")]
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
            //�P�b���ƂɂT���`���[�W�����
            timer += Time.deltaTime * 50;

            if (timer >= 1)
            {
                ChargeEnergy(1);
                timer = 0;
            }
        }

        if (uiPos != null)
        {
            // �v���C���[�̓���ɒǏ]
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position + new Vector3(0, 2f, 1));
            uiPos.position = screenPos;

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
                energySlider[0].value = energyAmount;
                energySlider[1].value = energyAmount;
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
                energySlider[0].value = energyAmount;
                energySlider[1].value = energyAmount;
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

        if (level < 5)
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
    /// ���x���_�E���p
    /// </summary>
    void LevelDown()
    {
        hammerObj[level - 1].SetActive(false);
        hammerObj[level - 2].SetActive(true);

        level--;

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
        energyUIObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(sliderDisplayTime);

        energyUIObj.gameObject.SetActive(false);
        showRoutine = null;
    }
}
