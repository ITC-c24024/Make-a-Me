using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnergyScript : MonoBehaviour
{
    //�G�l���M�[�̗�
    public int dropEnergyAmount;

    //�I�u�W�F�N�g�i���o�[
    int objNum;

    [SerializeField, Header("�X�^�[�g�̃|�W�V����")]
    Vector3 startPos;

    //�h���b�v�I�u�W�F�N�g�ɑΉ�����v���C���[��R�Â�
    public GameObject playerObj;

    //�E�����v���C���[��R�Â�
    GameObject pickPlayer;

    [SerializeField,Header("�h���b�v�ɂ����鎞��")] 
    float dropTime = 1f;

    [SerializeField,Header("�h���b�v�I�u�W�F�N�g�����Z�b�g�����܂ł̎���")] 
    int deleteTime;

    [SerializeField,Header("�擾�\�ȏ�Ԃ��ǂ����̔���")] 
    bool drop;

    [SerializeField,Header("//�h���b�v�����ǂ����̔���")] 
    bool start;

    //�h���b�v�A�擾�ǂ���Ȃ̂��̔���
    bool isDrop;

    //���ݒn�_
    Vector3 currentPos;

    //�^�[�Q�b�g�n�_
    Vector3 targetPos;

    //�h���b�v���̎��ԊǗ�
    float currentTime = 0f;

    //Y���̓����𐧌䂷��
    AnimationCurve animCurveY;

    //�����x�𒲐�����
    public AnimationCurve animCurveDrop;
    public AnimationCurve animCurvePick;

    //�h���b�v�}�l�[�W���[�X�N���v�g
    public DropEnergyManagerScript dropManagerSC;

    //�_�ŏ����̃X�N���v�g
    BlinkScript blinkSC;

    private void Start()
    {
        blinkSC = this.gameObject.GetComponent<BlinkScript>();
    }
    private void Update()
    {
        //�h���b�v���̋�������
        if (start)
        {
            //�E���ۂ̃^�[�Q�b�g���v���C���[�ɂ���
            if (!isDrop)
            {
                targetPos = pickPlayer.transform.localPosition;
            }

            var pos = new Vector3();

            pos.x = currentPos.x + (targetPos.x - currentPos.x) * currentTime / dropTime;
            pos.z = currentPos.z + (targetPos.z - currentPos.z) * currentTime / dropTime;

            pos.y = animCurveY.Evaluate(currentTime);

            transform.localPosition = pos;

            //�����x���A�j���[�V�����J�[�u�Œ���
            if (isDrop)
            {
                currentTime += Time.deltaTime * animCurveDrop.Evaluate(currentTime);
            }
            else
            {
                currentTime += Time.deltaTime * animCurvePick.Evaluate(currentTime);
            }

            //���S�ɗ����Ă��Ȃ��Ă��擾�ł���悤�ɂ���
            if (currentTime >= dropTime / 2 && isDrop)
            {
                drop = true;
            }

            if (currentTime >= dropTime)
            {
                start = false;
                currentTime = 0f;
            }
        }
    }

    /// <summary>
    /// DropEnergyManager������I�u�W�F�N�g�̔ԍ������蓖�Ă�
    /// </summary>
    /// <param name="num">�ԍ�</param>
    public void SetNum(int num)
    {
        objNum = num;
    }

    /// <summary>
    /// EnergyScript����h���b�v���̃G�l���M�[�����p���A�h���b�v������
    /// </summary>
    /// <param name="amount">�G�l���M�[�̗�</param>
    public IEnumerator SetHoneyAmount(int amount)
    {
        dropTime = 1f;

        dropEnergyAmount = amount;

        //�v���C���[�̃|�W�V�����Ńh���b�v
        currentPos = playerObj.transform.localPosition;

        //�h���b�v�n�_���甼�a3�̉~����̃����_���Ȓn�_�Ƀ^�[�Q�b�g���w��
        var angle = Random.Range(0, 360);
        var rad = angle * Mathf.Deg2Rad;
        var px = Mathf.Cos(rad) * 3f + currentPos.x;
        var pz = Mathf.Sin(rad) * 3f + currentPos.z;

        //�^�[�Q�b�g�n�_���X�e�[�W�O�ɂȂ�Ȃ��悤�ɂ���
        if (px <= -9f)//X��
        {
            px = -9f;
        }
        else if (px >= 9f)
        {
            px = 9f;
        }

        if (pz <= -5.5f)//Z��
        {
            pz = -5.5f;
        }
        else if (pz >= 3.2f)
        {
            pz = 3.2f;
        }
        targetPos = new Vector3(px, currentPos.y, pz);


        //�R�Ȃ�Ƀh���b�v����悤�ɂ���
        animCurveY = new AnimationCurve(
            new Keyframe(0, currentPos.y, 0, 10),
            new Keyframe(dropTime, targetPos.y, -10, 0)
            );


        start = true;
        isDrop = true;

        //�h���b�v��ܕb�҂��ē_�ł�����
        yield return new WaitForSeconds(dropTime + 5f);
        blinkSC.BlinkStart(5, 0.3f, 0.1f);

        //�_�ŏ������I���Ɠ����Ƀ��Z�b�g������
        yield return new WaitForSeconds(5);
        PosReset();
        drop = false;
        dropManagerSC.AddDrop(objNum);
    }


    private void OnTriggerStay(Collider other)
    {
        //�v���C���[���G�ꂽ�Ƃ��̎擾����
        if (drop)
        {
            if (other.gameObject.tag.StartsWith("P") && isDrop)
            {
                pickPlayer = other.gameObject;
                StartCoroutine(PickUp(other.gameObject));
            }
        }
    }

    /// <summary>
    /// �E�������̋�������A���Z����
    /// </summary>
    /// <param name="player">�E�����v���C���[</param>
    IEnumerator PickUp(GameObject player)
    {
        isDrop = false;

        dropTime = 0.5f;//�E������

        //�E�����ۂ�Y���̓���
        animCurveY = new AnimationCurve(
            new Keyframe(0, currentPos.y, 0, 10),
            new Keyframe(dropTime, targetPos.y + 0.5f, -10, 0)
            );

        currentPos = this.transform.localPosition;//���݂̈ʒu

        start = true;

        //�E����̂�҂��Ă�����Z
        yield return new WaitForSeconds(dropTime);
        var energySC = player.GetComponent<EnergyScript>();
        energySC.ChargeEnergy(dropEnergyAmount);

        drop = false;

        PosReset();

        dropManagerSC.AddDrop(objNum);//�E��ꂽ�ۂ̃��X�g�ǉ�
    }

    /// <summary>
    /// �擾��A���Ō�̃|�W�V�������Z�b�g
    /// </summary>
    void PosReset()
    {
        this.transform.localPosition = startPos;
    }
}
