using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    float maxTime = 1.5f;//�X�P�[���A�b�v�ƃ_�E���ɂ����鎞��
    float maxScale = 1.1f;//�ő�X�P�[���̒l_
    int sceneNum = 0;//UI����̂��߂̃V�[���ԍ�

    [SerializeField]
    [Header("�X�P�[���̃J�[�u����")]
    AnimationCurve scaleCurve;

    [SerializeField]
    [Header("UI�v�f")]
    GameObject[] uiElements;

    /*float bombScaleMaxTime = 0.7f;//BombUI�̍ő�X�P�[���ɂȂ�܂ł̎���
    Vector3 tagetScale;//Bomb��tagetScale*/

    [SerializeField]
    [Header("TitleImage�̃I�u�W�F�N�g")]
    GameObject titleImage;

    //float startTime = 0f;//���e����Ԃ܂ł̎���

    //private Vector3 bombOriginalScale;//TitleImage�̌��̃X�P�[��

    float inputCooldown = 0.25f;//���͐����̎���

    private float lastInputTime = 0f;//���͐��������邽�߂̎���

    //float speed = 0.02f;//�t�F�[�h�A�E�g����X�s�[�h

    //float bombFadeOutSpeed = 0.07f;//�������t�F�[�h�A�E�g����X�s�[�h

    /*[SerializeField]
    [Header("�t�F�[�h����C���[�W")]
    Image fadeImage;

    [SerializeField]
    [Header("Bomb�̃��N�g�g�����X�t�H�[��(RectTransform�^)")]
    RectTransform bombUI;

    [SerializeField]
    [Header("Bomb�̃C���[�W(Image�^)")]
    Image bombImage;

    [SerializeField]
    [Header("Bomb�̃C���[�W(GameObject�^)")]
    GameObject bombImageObject;

    [SerializeField]
    [Header("Bomb�̃X�v���C�g")]
    Sprite newSprite;

    [SerializeField]
    [Header("�J�n�ʒu")]
    Vector2 startPos;

    [SerializeField]
    [Header("�ڕW�n�_")]
    Vector2 targetPos;

    float launchSpeed = 1000f;//���e����ԃX�s�[�h

    float gravity = -2000f;//���̏d��

    float rotationSpeed = 360f;//���e�̉�]���x*/

    [SerializeField]
    [Header("���f��")]
    AudioSource SE;

    [SerializeField]
    [Header("���艹")]
    AudioClip DecisionSE;

    [SerializeField]
    [Header("�I����")]
    AudioClip selectSE;

    [SerializeField]
    [Header("�^�C�g��BGM")]
    AudioSource titleBGM;

    bool hasPlayedSound = false;//����炷���߂�bool

    //float timeElapsed = 0f;//���e�����ł鎞��

    Vector3 velocity;//���x���v�Z���邽�߂̔�

    float ed, green, blue, alfa;//FadeImage��color�����ނ��߂̂���

    float red2, green2, blue2, alfa2;//BombImage��color�����ނ��߂̂���

    bool Out = false;//FadeOut��bool

    //BomberGentleman inputActions;//InputManager�̃X�N���v�g
    InputAction stickAction;//Stick����̏���

    Coroutine currentCoroutine; // ���ݎ��s���̃R���[�`����ێ�

    [SerializeField]
    GameObject[] players;//�v���C���[�I�u�W�F�N�g

    [SerializeField]
    GameObject energyCore;//energy�I�u�W�F�N�g

    public float energyCoreThrowTimer = 0f;

    [SerializeField] Animator animator2;//�v���C���[2�̃A�j���[�V����
    [SerializeField] Animator animator3;//�v���C���[3�̃A�j���[�V����

    [SerializeField] Rigidbody enertyCoreRB;//energy�R�A�̃��W�b�h�{�f�B
    [SerializeField] bool[] startAnim;//�ǂ̃A�j���[�V�����𗬂���

    [SerializeField] Quaternion targetRotation; // �ڕW�̉�]�p�x
    bool isRotating = false;   // ��]�����ǂ����̃t���O

    float start = 0f;

    [SerializeField]
    [Header("battery�Ɏw�����ރG�l���M�[�i���߂���j")]
    GameObject[] energy;

    [SerializeField]
    [Header("Energy���w�����ޓ��ꕨ")]
    GameObject battery;//StartAnim[1](true)�̎��Ɏg������

    bool isReturning = false;
    float returnTimer = 0f; // 0.5�b��̏����p�^�C�}�[
    Quaternion originalRotation; // ���̉�]�l

    float player4StartTime = 0f;//�A�j���[�V����3�𗬂����߂̃^�C�}�[

    [SerializeField]
    [Header("takeEnergyCore�����邽�߂̂���")]
    GameObject takeEnergyCore;

    float backTime = 0f;

    float setTime = 0f;

    [SerializeField] bool[] isMove;

    [SerializeField]
    [Header("�R���x�A�̃A�j���[�^�[")]
    Animator conveyorAnim;

    bool hasReachedPosition = false; // ��~�������ǂ���
    bool isMovingAgain = false; // �ĊJ�������ǂ���

    Vector3 force;

    /*private void Awake()
    {
        inputActions = new BomberGentleman();
        inputActions.Enable();

        stickAction = inputActions.Player.Move; // Move�̒���LeftStick������

        scaleCurve = null; // ScaleCurve�̐ݒ������
    }*/

    void Start()
    {
        // �����ʒu��ݒ�
        //bombUI.anchoredPosition = startPos;

        // �^�[�Q�b�g�܂ł̋����ƕ������v�Z
        //Vector2 direction = targetPos - startPos;

        // ���������̎��Ԃ��v�Z
        //float timeToTarget = direction.magnitude / launchSpeed;

        // ���������̑��x���v�Z
        //float verticalSpeed = (direction.y - 0.5f * gravity * Mathf.Pow(timeToTarget, 2)) / timeToTarget;

        // �������x��ݒ�
        //velocity = new Vector2(direction.normalized.x * launchSpeed, verticalSpeed);

        //bombOriginalScale = bombImage.transform.localScale;

        //tagetScale = new Vector3(11, 11, 11);//�����̃}�b�N�X�X�P�[��

        Application.targetFrameRate = 60;//�t���[�����[�g�̌Œ�

        // �J�[�u���ݒ肳��Ă��Ȃ��ꍇ�A�f�t�H���g�̃J�[�u���쐬
        if (scaleCurve == null || scaleCurve.length == 0)
        {
            scaleCurve = new AnimationCurve(
                new Keyframe(0f, 0f),    // �X�^�[�g
                new Keyframe(0.5f, 1f), // �X�P�[���A�b�v
                new Keyframe(1f, 0f)    // �X�P�[���_�E��
            );
        }

        //fadeImage��Image��false�������ꍇ
        /*if (!fadeImage.enabled)
        {
            fadeImage.enabled = true;
        }*/

        //titleImage.SetActive(false);

        //Out = true;//Out��bool��true�ɂ���FadeOut�����s

        //startAnim[0] = true;

        titleBGM.Play();

        // �������Ƃ��čŏ���UI�𓮂���
        //StartAnimationForScene();
    }

    void Update()
    {
        //videoPlayer.Play();//������Đ�

        /*if (Time.time - lastInputTime >= inputCooldown) // ���͐���
        {
            float verticalInput = stickAction.ReadValue<Vector2>().x;

            if (verticalInput > 0 && sceneNum < 2)
            {
                sceneNum++;
                lastInputTime = Time.time;
                SE.PlayOneShot(selectSE);
                StartAnimationForScene();
            }
            else if (verticalInput < 0 && sceneNum > 0)
            {
                sceneNum--;
                lastInputTime = Time.time;
                SE.PlayOneShot(selectSE); StartAnimationForScene();
            }
        }*/



        //if (inputActions.Player.Start.triggered)
        {
            switch (sceneNum)
            {
                case 0:
                    SE.PlayOneShot(DecisionSE);//���艹��炷
                    Invoke("SelectScene", 0.3f);//�Q�[���V�[���Ɉړ�
                    break;
                case 1:
                    SE.PlayOneShot(DecisionSE);//���艹��炷
                    Invoke("SelectScene", 0.3f);//�����V�[���Ɉړ�
                    break;
                case 2:
                    SE.PlayOneShot(DecisionSE);//���艹��炷
                    Invoke("SelectScene", 0.3f);//�Q�[�����I��
                    break;
            }
        }

        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(i == sceneNum);
        }

        /*if (Out)//Out��bool��true�̏ꍇ�t�F�[�h�A�E�g
        {
            FadeOut();
        }*/

        /*if (startTime < 3f)//3�b���^�C�}�[�����Ȃ�������
        {
            startTime += Time.deltaTime;
        }

        if (startTime > 1f)
        {
            // �O��̈ʒu��ۑ�
            Vector2 previousPosition = bombUI.anchoredPosition;

            // ���Ԍo�߂����Z
            timeElapsed += Time.deltaTime;

            // �������̍��W�v�Z
            float x = velocity.x * timeElapsed;
            float y = velocity.y * timeElapsed + 0.5f * gravity * Mathf.Pow(timeElapsed, 2);

            // �V�����ʒu���v�Z
            Vector2 newPosition = new Vector2(startPos.x + x, startPos.y + y);

            // ��]���v�Z
            float rotationAngle = timeElapsed * rotationSpeed; // ���Ԃɉ�������]
            bombUI.rotation = Quaternion.Euler(0, 0, rotationAngle); // ��]�̍X�V

            // �t���[���ԂŖڕW�n�_�𒴂��������m�F
            if (HasPassedTarget(previousPosition, newPosition, targetPos))
            {
                // �ڕW�n�_�Ɉʒu���Œ�
                bombUI.anchoredPosition = targetPos;

                // �}���ɉ�]������
                float finalRotationAngle = 3000f; // ��]�̍ŏI�p�x�i�Ⴆ��720�x�ɐݒ�j

                // ��]���X���[�Y�ɕ�Ԃ���R���[�`�����J�n
                StartCoroutine(RotateBombSmoothly(finalRotationAngle));

                // ��]�����Z�b�g����ꍇ�́A�������Ԃ������Ă��猳�ɖ߂�
                if (!hasPlayedSound)
                {
                    //SE.PlayOneShot(bombSE);
                    hasPlayedSound = true;
                }

                timeElapsed = 0f; // �o�ߎ��Ԃ����Z�b�g
                bombImage.sprite = newSprite; // ���e�C���[�W���甚���C���[�W�ɕύX

                StartCoroutine(BombScaleUp()); // �����A�j���[�V�����̃R�[���`�����Ăяo��
                if (startTime > 2.7f)
                {
                    titleImage.SetActive(true); // �^�C�g����\��
                }
                Invoke("BombFadeOut", 0.7f);//�������\������Ă���0.7�b��Ƀt�F�[�h�A�E�g������
            }
            else
            {
                // �ʒu���X�V
                bombUI.anchoredPosition = newPosition;
            }
        }*/

        // �t���[���ԂŖڕW�n�_��ʂ�߂��������m�F����
        /*bool HasPassedTarget(Vector2 previousPosition, Vector2 currentPosition, Vector2 targetPosition)
        {
            // ���݂ƑO��̈ʒu�̊ԂŖڕW�n�_������ł��邩
            return Vector2.Distance(previousPosition, targetPosition) < Vector2.Distance(currentPosition, targetPosition);
        }

        //���d�œ|���A�j���[�V����
        if (startAnim[0])
        {
            ChangeAnim1();
        }

        //Energy���`���[�W����A�j���[�V����
        if (startAnim[1])
        {
            ChangeAnim2();
        }

        //�G�l���M�[�R�A��D���A�j���[�V����
        if (startAnim[2])
        {
            ChangeAnim3();
        }*/
    }


    //�X�^�[�g���̃t�F�[�h�A�E�g�̏���
    /*void FadeOut()
    {
        fadeImage.enabled = true;
        alfa -= speed;
        Alpha();
        if (alfa <= 0)
        {
            Out = false;
            fadeImage.enabled = false;
        }
    }*/


    /*void Alpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }*/


    //�����̃t�F�[�h�A�E�g
    /*void BombFadeOut()
    {
        alfa2 -= bombFadeOutSpeed;
        Alpha2();
        if (alfa2 <= 0)
        {
            bombImage.enabled = fadeImage;
        }
    }*/


    /*void Alpha2()
    {
        bombImage.color = new Color(red2, green2, blue2, alfa2);
    }*/


    /*private void StartRotation()
    {
        // �ڕW�̉�]��ݒ�i0, -90, 0 �̉�]�j
        targetRotation = Quaternion.Euler(0, -90, 0);
        originalRotation = players[2].transform.rotation; // ���̉�]��ۑ�
        isRotating = true;
    }*/


    void SelectScene()
    {
        switch (sceneNum)
        {
            case 0:
                SceneManager.LoadScene("MainGameScene");
                break;
            case 1:
                SceneManager.LoadScene("RuleScene");
                break;
            case 2:
                Application.Quit();
                break;
        }
    }


    //startAnim[0]��true�̎��ɗ���
    /*void ChangeAnim1()
    {
        //�v���C���[���S�g���鏈��
        if (players[0].transform.position.x > -2.05f)
        {
            players[0].transform.Translate(Vector3.back * 3 * Time.deltaTime);
            players[1].transform.Translate(Vector3.forward * 3 * Time.deltaTime);
        }

        if (players[0].transform.position.x < -2.05f)
        {
            if (energyCoreThrowTimer < 3f)
            {
                energyCoreThrowTimer += Time.deltaTime;
            }

            //�v���C���[�����e�𓊂��鏈��
            if (energyCoreThrowTimer > 1f)
            {
                animator3.SetBool("IsThrow", true);
                force = new Vector3(-29f, 0f, 0f);
                enertyCoreRB.AddForce(force);
                enertyCoreRB.constraints = RigidbodyConstraints.None;
            }

            //�v���C���[���O�i���鏈��
            if (energyCoreThrowTimer > 3)
            {
                players[0].transform.Translate(Vector3.back * 3 * Time.deltaTime);
                players[1].transform.Translate(Vector3.forward * 3 * Time.deltaTime);
                conveyorAnim.speed = 1;//�R���x�A�A�j���[�V�������X�^�[�g
            }
            else
            {
                conveyorAnim.speed = 0;//�R���x�A�A�j���[�V�������~�߂�
            }

            //���ׂĂ����Z�b�g����
            if (players[0].transform.position.x < -18.5f)
            {
                players[0].transform.position = new Vector3(15.2f, -1.26f, 0.78f);
                players[1].transform.position = new Vector3(19.45f, -1.26f, 0.78f);
                startAnim[0] = false;
                force = Vector3.zero;
                animator2.Rebind();
                animator3.Rebind();
                energyCore.transform.position = new Vector3(19.7f, -0.42f, 0.78f);
                enertyCoreRB.constraints = RigidbodyConstraints.FreezeAll;
                enertyCoreRB.velocity = Vector3.zero;
                enertyCoreRB.transform.rotation = new Quaternion(0, 90, 0, 0);
                energyCoreThrowTimer = 0f;
                startAnim[1] = true;
            }
        }
    }

    //startAnim[1]��true�̎��ɗ���
    void ChangeAnim2()
    {
        if (start < 11)
        {
            start += Time.deltaTime;
        }

        if (players[2].transform.position.x > 1.86f)
        {
            //�v���C���[��O�i������
            players[2].transform.Translate(Vector3.left * 3 * Time.deltaTime);
            battery.transform.Translate(Vector3.right * 3 * Time.deltaTime);

            //�o�b�e���[��u��
            if (players[2].transform.position.x < 1.86f)
            {
                StartRotation();

                energy[0].SetActive(false);
                energy[1].SetActive(true);
                conveyorAnim.speed = 0;//�R���x�A�A�j���[�V�������~�߂�
            }
        }

        if (start > 7f)
        {
            players[2].transform.Translate(Vector3.left * 3 * Time.deltaTime);
            battery.transform.Translate(Vector3.right * 3 * Time.deltaTime);
            conveyorAnim.speed = 1;//�R���x�A�A�j���[�V�������X�^�[�g
        }

        //���ׂĂ����Z�b�g
        if (players[2].transform.position.x < -13)
        {
            startAnim[1] = false;
            players[2].transform.position = new Vector3(19.79f, -1.26f, 0.78f);
            battery.transform.position = new Vector3(16.15f, -1.9f, 1.26f);
            players[2].transform.rotation = new Quaternion(0, 0, 0, 0);
            energy[0].SetActive(true);
            energy[1].SetActive(false);
            start = 0f;
            startAnim[2] = true;
        }

        //��]�����邽�߂̏���
        if (isRotating)
        {
            float step = rotationSpeed * Time.deltaTime; // �X���[�Y�ȉ�]
            players[2].transform.rotation = Quaternion.RotateTowards(players[2].transform.rotation, targetRotation, step);

            // �ڕW�p�x�ɏ\���߂Â������]���I��
            if (Quaternion.Angle(players[2].transform.rotation, targetRotation) < 0.1f)
            {
                players[2].transform.rotation = targetRotation; // ���S�ɖڕW�p�x�ɃX�i�b�v
                isRotating = false; // ��]�I��
                returnTimer = 0.5f; // 0.5�b�҂��߂̃^�C�}�[�ݒ�
                isReturning = true; // ���ɖ߂�����
            }
        }

        //��]�����ɖ߂�����
        if (isReturning)
        {
            if (returnTimer > 0)
            {
                returnTimer -= Time.deltaTime; // 0.5�b�̑ҋ@���Ԃ��J�E���g�_�E��
            }
            else
            {
                float step = rotationSpeed * Time.deltaTime; // ���̉�]�ւ̃X���[�Y�ȉ�]
                players[2].transform.rotation = Quaternion.RotateTowards(players[2].transform.rotation, originalRotation, step);

                // ���̉�]�ɖ߂����珈�����I��
                if (Quaternion.Angle(players[2].transform.rotation, originalRotation) < 0.1f)
                {
                    players[2].transform.rotation = originalRotation; // ���S�Ɍ��̊p�x�ɃX�i�b�v
                    isReturning = false; // ���ɖ߂������I��
                }
            }
        }
    }

    //startAnim[2]��true�̎��ɗ���
    void ChangeAnim3()
    {
        if (players[3].transform.position.x > -2.05f && backTime == 0)
        {
            //�v���C���[��O�i������
            players[3].transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            players[4].transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            takeEnergyCore.transform.Translate(Vector3.left * 3 * Time.deltaTime);

            hasReachedPosition = false; // ��~��Ԃ����Z�b�g
            isMovingAgain = false; // �ĊJ�����Z�b�g
        }

        if (players[3].transform.position.x <= -2.05f && backTime == 0 && !hasReachedPosition)
        {
            conveyorAnim.speed = 0; // �R���x�A���~
            hasReachedPosition = true; // ��~�t���O��ݒ�
        }

        // �^�C�}�[�𓮂���
        if (player4StartTime < 8f)
        {
            player4StartTime += Time.deltaTime;
        }

        // �v���C���[4�̓���
        if (players[4].transform.position.x > -0.28f && player4StartTime > 6.1f && backTime == 0)
        {
            //�G�l���M�[�R�A���Ƃ�v���C���[������O�i������
            players[4].transform.Translate(Vector3.forward * 3 * Time.deltaTime);
        }

        // �G�l���M�[�R�A�̈ʒu�𒲐�
        if (players[4].transform.position.x > -0.7f && player4StartTime > 7f)
        {
            if (setTime == 0)
            {
                //�G�l���M�[�R�A��player[4]�̏�Ɉړ�������
                takeEnergyCore.transform.position = new Vector3(-0.017f, -0.42f, 0.78f);
            }
            if (setTime < 1f && startAnim[2])
            {
                setTime += Time.deltaTime;
                isMove[0] = true;
            }
            backTime += Time.deltaTime;
        }

        if (backTime > 0.5f && players[4].transform.position.x < 1.5f && isMove[0])
        {
            //�v���C���[�����e���Ƃ�������ɉ�����
            players[4].transform.Translate(Vector3.back * 3 * Time.deltaTime);
            takeEnergyCore.transform.Translate(Vector3.right * 3 * Time.deltaTime);
        }

        // ���̃A�j���[�V�����̏���
        if (backTime > 2)
        {
            isMove[0] = false;
            isMove[1] = true;
        }


        if (backTime > 2.5f && players[3].transform.position.x > -18 && isMove[1])
        {
            // �v���C���[�̓������ĊJ
            players[3].transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            players[4].transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            takeEnergyCore.transform.Translate(Vector3.left * 3 * Time.deltaTime);

            if (!isMovingAgain)
            {
                conveyorAnim.speed = 1; // �R���x�A���Ďn��
                isMovingAgain = true; // �ĊJ�t���O��ݒ�
            }
        }

        //���ׂĂ����Z�b�g
        if (players[3].transform.position.x < -17.6f)
        {
            startAnim[2] = false;
            backTime = 0f;
            player4StartTime = 0f;
            setTime = 0f;
            players[3].transform.position = new Vector3(14.46f, -1.26f, 0.78f);
            players[4].transform.position = new Vector3(18f, -1.26f, 0.78f);
            takeEnergyCore.transform.position = new Vector3(14.75f, -0.42f, 0.78f);
            isMove[1] = false;
            startAnim[0] = true;
            hasReachedPosition = false;
            isMovingAgain = false;
        }
    }*/

    //�R�[���`����؂�ւ��鏈��
    void StartAnimationForScene()
    {
        // ���݂̃R���[�`�����~
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);

            // ���݂�UI�̃X�P�[�������ɖ߂�
            GameObject currentUI = uiElements[sceneNum];
            currentUI.transform.localScale = Vector3.one;  // ���̃X�P�[���Ƀ��Z�b�g
        }

        // �I�����ꂽ UI �̏����X�P�[�����擾
        GameObject targetUI = uiElements[sceneNum];
        Vector3 originalScale = targetUI.transform.localScale;

        // �V�����R���[�`�����J�n
        currentCoroutine = StartCoroutine(SelectUIScaleLoop(targetUI, originalScale));
    }


    //Scale��Up��Down�̏���
    IEnumerator SelectUIScaleLoop(GameObject targetUI, Vector3 originalScale)
    {
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * maxScale;

        //�������[�v
        while (true)
        {
            while (elapsedTime < maxTime)
            {
                elapsedTime += Time.deltaTime;

                // �J�[�u�ɏ]�����X�P�[���l���v�Z
                float t = elapsedTime / maxTime;
                float scaleFactor = scaleCurve.Evaluate(t);
                targetUI.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleFactor);

                yield return null;
            }

            elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g
        }
    }


    //BombImage�̃X�P�[���A�b�v
    /*IEnumerator BombScaleUp()
    {
        float timer = 0f;

        while (timer < bombScaleMaxTime)
        {
            timer += Time.deltaTime;
            float scaleTime = timer / bombScaleMaxTime;
            bombImageObject.transform.localScale = Vector3.Lerp(bombOriginalScale, tagetScale, scaleTime);

            yield return null;
        }

        bombImageObject.transform.localScale = tagetScale;
    }


    //�����̉�]����
    IEnumerator RotateBombSmoothly(float targetAngle)
    {
        float rotationDuration = 2f; // ��]�̏��v����
        float startAngle = bombUI.rotation.eulerAngles.z; // ���݂̉�]�p�x
        float timeElapsed = 0f;

        while (timeElapsed < rotationDuration)
        {
            // ��Ԃŉ�]���v�Z
            float angle = Mathf.Lerp(startAngle, targetAngle, timeElapsed / rotationDuration);
            bombUI.rotation = Quaternion.Euler(0, 0, angle);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        bombUI.rotation = Quaternion.Euler(0, 0, targetAngle); // �ŏI�I�ȉ�]�p�x��ݒ�
    }*/
}
