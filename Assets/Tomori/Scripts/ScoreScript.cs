using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    [SerializeField]
    EnergyScript energyScript;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    ScoreManager scoreManager;

    [SerializeField, Header("�N���[���I�u�W�F�N�g")]
    GameObject[] clones;
    [SerializeField]
    GameObject player;
    [SerializeField, Header("�n���}�[�I�u�W�F�N�g")]
    GameObject hammer;
    [SerializeField, Header("�n���}�[�Ǐ]�I�u�W�F�N�g")]
    GameObject follow;

    [SerializeField, Header("�X�R�AUI")]
    Image[] scoreImage;
    [SerializeField, Header("����UI")]
    Sprite[] numSprite;

    public float maxTime = 10f;
    [SerializeField] float workTime = 0f;
    [SerializeField, Header("��ƃN�[���^�C��")]
    float coolTime = 1.0f;
    //��ƌ���
    private float[] efficiency = new float[] { 1.0f, 1.5f, 2.5f, 4.0f, 6.0f };

    [SerializeField] int score = 0;
    [SerializeField] bool isWork = false;
    bool isArea = false;

    [SerializeField] int workAreaNum = 0;

    [SerializeField, Header("�N���[���̃X�^�[�gZ���W")]
    float startPos;
    [SerializeField, Header("�N���[���̒�~Z���W")]
    float stopPos;

    void Start()
    {
        StartCoroutine(MoveMaterial());
    }

    void Update()
    {
        if (isWork)
        {
            workTime += Time.deltaTime * efficiency[energyScript.level-1];          
        }
        if (isArea)
        {
            hammer.transform.position = follow.transform.position;
            hammer.transform.rotation = follow.transform.rotation;
        }

        if(workTime >= maxTime / 2) //��i�K
        {
            clones[0].SetActive(false);
            clones[1].SetActive(true);
        }
        if (workTime >= maxTime) //����
        {
            clones[1].SetActive(false);
            clones[2].SetActive(true);

            isWork = false;
            playerController.Job(isWork);

            workTime = 0;
            score += 1;
            scoreManager.ChangeScore(playerController.playerNum);
            SetUI();

            StartCoroutine(MoveClone());
        }
    }

    /// <summary>
    /// �N���[���ޗ��̈ړ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveMaterial()
    {
        clones[0].SetActive(true);
        
        float time = 0;
        while (time < coolTime)
        {
            time += Time.deltaTime;

            float currentPos = Mathf.Lerp(startPos, stopPos, time / coolTime);
            for(int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = new Vector3(
                clones[i].transform.position.x,
                clones[i].transform.position.y,
                currentPos
                );
            }           

            yield return null;
        }
        if (isArea)
        {
            isWork = true;
            playerController.Job(isWork);
        }
    }
    /// <summary>
    /// �����N���[���̈ړ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveClone()
    {       
        yield return new WaitForSeconds(1.0f);

        float time = 0;
        while (time < coolTime)
        {
            time += Time.deltaTime;

            float currentPos = Mathf.Lerp(stopPos, startPos, time / coolTime);
            for (int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = new Vector3(
                clones[i].transform.position.x,
                clones[i].transform.position.y,
                currentPos
                );
            }

            yield return null;
        }
        clones[2].SetActive(false);

        StartCoroutine(MoveMaterial());
    }    

    void SetUI()
    {
        int ten = score / 10; //�\�̈�
        int one = score - 10 * ten; //��̈�

        //�\�̈ʂ����鎞,�\�̈ʂ��\������Ă��Ȃ��Ƃ�
        if (ten > 0 && !scoreImage[1].enabled)
        {
            Debug.Log("hoge");
            //��̈ʂ��E�ɂ��炷
            scoreImage[0].rectTransform.anchoredPosition = new Vector2(
                scoreImage[0].rectTransform.anchoredPosition.x + 20,
                scoreImage[0].rectTransform.anchoredPosition.y
                );
            //�\�̈ʂ�\��
            scoreImage[1].enabled = true;
        }
        
        scoreImage[0].sprite = numSprite[one];
        scoreImage[1].sprite = numSprite[ten];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isArea = true;
            isWork = true;
            playerController.Job(isWork);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isArea = false;
            isWork = false;
            playerController.Job(isWork);
            hammer.transform.position = new Vector3(
                player.transform.position.x, 
                player.transform.position.y, 
                -0.035f
                ) ;
            hammer.transform.rotation = Quaternion.Euler(0, 0, 90);
            
        }
    }
}
