using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnergyScript : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;

    [SerializeField, Header("�v���C���[�ԍ�")]
    int playerNum;

    // ���x��
    public int level = 1;

    // �ő僌�x��
    int maxLevel = 5;

    [SerializeField, Header("���擾�G�l���M�[")]
    int allEnergyAmount;

    [SerializeField, Header("���x�����Ƃ̎擾�G�l���M�\��")]
    int energyAmount;

    //���x���A�b�v�ɕK�v�ȃG�l���M�[��
    int[] requireEnergy = { 50, 75, 100, 125, 150 };

    //���x���ɉ����Ẵh���b�v����
    float[] dropRate = { 0.2f, 0.3f, 0.5f, 0.7f, 0.8f };

    [SerializeField, Header("�`���[�W��")]
    bool isCharge;

    // �`���[�W�p�^�C�}�[
    float timer = 0;

    // �h���b�v�}�l�[�W���[�X�N���v�g
    DropEnergyManagerScript dropManagerSC;

    [SerializeField, Header("�G�l���M�[�ʕ\���pUI")]
    GameObject energyUIObj;

    [SerializeField, Header("�G�l���M�[�ʕ\���p�X���C�_�[")]
    Slider[] energySlider;

    [SerializeField, Header("�n���}�[�C���[�W")]
    Image hammerImage;

    [SerializeField, Header("�n���}�[�̃X�v���C�g")]
    Sprite[] hammerSprite;

    [SerializeField, Header("���x����Image")]
    Image[] levelImage;

    [SerializeField, Header("���x����Sprite(��펞)")]
    Sprite[] levelSprite1;

    [SerializeField, Header("���x����Sprite(�펞)")]
    Sprite[] levelSprite2;

    [SerializeField, Header("�X���C�_�[�\������(�b)")]
    float sliderDisplayTime = 3f;

    // �X���C�_�[�\���R���[�`���p
    Coroutine showRoutine;

    // UI�̍��W
    Transform uiPos;

    // �J����
    Camera mainCam;

    [SerializeField, Header("�n���}�[�I�u�W�F�N�g")]
    GameObject[] hammerObj;

    [SerializeField, Header("�n���}�[�̐e�I�u�W�F�N�g")]
    GameObject hammerMasterObj;

    private void Start()
    {
        dropManagerSC = gameObject.GetComponent<DropEnergyManagerScript>();

        energySlider[0].maxValue = requireEnergy[0];
        energySlider[1].maxValue = requireEnergy[0];

        uiPos = energyUIObj.transform;

        mainCam = Camera.main;
    }

    void Update()
    {
        if (isCharge)
        {
            // 1�b���Ƃ�20���`���[�W�����
            timer += Time.deltaTime * 10;
            if (timer >= 1f)
            {
                ChargeEnergy(1);
                timer = 0f;
            }
        }

        if (uiPos != null)
        {
            // �v���C���[�̓���ɒǏ]
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 2));
            uiPos.position = screenPos;
        }
    }

    /// <summary>
    /// �G�l���M�[�̃`���[�W
    /// </summary>
    public void ChargeEnergy(int amount)
    {

        if (allEnergyAmount < 500)
        {
            allEnergyAmount += amount;
        }

        while (amount + energyAmount >= requireEnergy[level - 1])
        {
            amount -= requireEnergy[level - 1] - energyAmount;

            if (level != maxLevel)
            {
                LevelUp();
            }
            else
            {
                // ���x���ő�Ȃ�Q�[�W�𖄂ߐ؂��ďI��
                energyAmount = requireEnergy[level - 1];
                energySlider[0].maxValue = requireEnergy[level - 1];
                energySlider[1].maxValue = requireEnergy[level - 1];
                energySlider[0].value = energyAmount;
                energySlider[1].value = energyAmount;

                amount = 0;
                break;
            }

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
    /// �G�l���M�[�̃h���b�v(���ʂ�1/3)
    /// </summary>
    public void LostEnergy()
    {
        if (allEnergyAmount <= 0) return;

        // ���ʂ̂����A���x���ɉ����ăG�l���M�[���h���b�v
        int amount = Mathf.CeilToInt(allEnergyAmount * dropRate[level-1]);
        dropManagerSC.Drop(amount);
        allEnergyAmount -= amount;

        while (amount > 0)
        {
            if (energyAmount >= amount)
            {
                // ���̃��x���Ō��炵�����
                energyAmount -= amount;
                amount = 0;
            }
            else
            {
                // ���̃��x���̃Q�[�W��S���g���؂�
                amount -= energyAmount;
                energyAmount = 0;

                // �܂����炷�����c���Ă��āA���x�� > 1 �Ȃ�_�E��
                if (level > 1)
                {
                    LevelDown();
                    // ���x���_�E�������玟�̃Q�[�W�͖��^������X�^�[�g
                    energyAmount = requireEnergy[level - 1];
                }
                else
                {
                    // ���x��1�ō��؂�����[��
                    amount = 0;
                }
            }
        }

        // �X���C�_�[�X�V
        energySlider[0].maxValue = requireEnergy[Mathf.Clamp(level - 1, 0, requireEnergy.Length - 1)];
        energySlider[1].maxValue = energySlider[0].maxValue;
        energySlider[0].value = energyAmount;
        energySlider[1].value = energyAmount;

        ShowSlider();
    }



    /// <summary>
    /// ���x���A�b�v�p
    /// </summary>
    void LevelUp()
    {
        audioManager.LevelUp();

        level++;
        level = Mathf.Clamp(level, 1, maxLevel);

        hammerObj[level - 2].SetActive(false);
        hammerObj[level - 1].SetActive(true);
        hammerImage.sprite = hammerSprite[level - 1];

        StartCoroutine(BounceObject(hammerMasterObj.transform, 0.3f));
        StartCoroutine(BounceObject(levelImage[0].transform, 0.3f));

        energyAmount = 0;

        energySlider[0].maxValue = requireEnergy[level - 1];
        energySlider[1].maxValue = requireEnergy[level - 1];
        energySlider[0].value = 0;
        energySlider[1].value = 0;

        levelImage[0].sprite = levelSprite1[level - 1];
        levelImage[1].sprite = levelSprite2[level - 1];
    }

    /// <summary>
    /// ���x���_�E���p
    /// </summary>
    void LevelDown()
    {
        level--;
        level = Mathf.Clamp(level, 1, maxLevel);

        hammerObj[level].SetActive(false);
        hammerObj[level - 1].SetActive(true);
        hammerImage.sprite = hammerSprite[level - 1];

        StartCoroutine(BounceObject(hammerMasterObj.transform, 0.3f));
        StartCoroutine(BounceObject(levelImage[0].transform, 0.3f));

        energySlider[0].maxValue = requireEnergy[level - 1];
        energySlider[1].maxValue = requireEnergy[level - 1];
        energySlider[0].value = energyAmount;
        energySlider[1].value = energyAmount;

        levelImage[0].sprite = levelSprite1[level - 1];
        levelImage[1].sprite = levelSprite2[level - 1];
    }

    IEnumerator BounceObject(Transform target, float time)
    {
        float t = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (t < 1f)
        {
            t += Time.deltaTime / time;

            float s = 2f;
            float curved = 1f + s * Mathf.Pow(t - 1f, 3) + s * Mathf.Pow(t - 1f, 2);

            target.localScale = Vector3.LerpUnclamped(startScale, endScale, curved);
            yield return null;
        }

        target.localScale = Vector3.one;
    }





    /// <summary>
    /// �G�l���M�[�`���[�W�����ǂ����̐؂�ւ��p
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

