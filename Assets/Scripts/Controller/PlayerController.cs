using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Common")]
    /// <summary>
    /// �⺻ ���׸���
    /// ��ȣ�ۿ� �Ǿ��� �� ���׸���
    /// </summary>
    [SerializeField]
    Material m_standardInteractMat = null;
    [SerializeField]
    Material m_activeInteractMat = null;
    /// <summary>
    /// ĳ���� �ε��� ���� ����
    /// </summary>
    [SerializeField]
    int m_charactorCode = 0;
    /// <summary>
    /// �ִ� ȸ�� Ƚ��
    /// </summary>
    [SerializeField]
    int m_maxRecoverCount = 2;
    /// <summary>
    /// ȸ�� �� hp ���
    /// </summary>
    [SerializeField]
    float m_recoverHp = 40.0f;


    [Header("Camera")]
    /// <summary>
    /// �÷��̾� 3��Ī ī�޶� ��ġ ���̽� ��ġ
    /// </summary>
    [SerializeField]
    Transform m_cameraBaseTransform = null;
    /// <summary>
    /// ���콺 ����
    /// </summary>
    [SerializeField]
    float m_mouseSensitivity = 1.0f;
    /// <summary>
    /// ī�޶� ���� ����
    /// </summary>
    [SerializeField]
    float m_camMinXRotation = -30.0f;
    [SerializeField]
    float m_camMaxXRotation = 50.0f;

    [Header("Transform")]
    /// <summary>
    /// �߷� �ӵ�
    /// </summary>
    [SerializeField]
    public float m_fallSpeed = 12f;
    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    [SerializeField]
    float m_jumpForce = 15.0f;
    /// <summary>
    /// �ִ� ���� ���� ������
    /// </summary>
    [SerializeField]
    float m_maxjumpGauge = 10.0f;
    /// <summary>
    /// ���� ������ �ӵ�
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 50.0f;

    [Header("Player Infomation")]
    /// <summary>
    /// �ʱ� �÷��̾� ��ġ
    /// </summary>
    [SerializeField]
    Vector3 m_playerResetPos = new Vector3();
    /// <summary>
    /// �ִ� ü��
    /// </summary>
    [SerializeField]
    int m_maxHp = 0;
    /// <summary>
    /// hp�� late hp ����ȭ �б� �ð�
    /// </summary>
    [SerializeField]
    float m_maxHpSyncTime = 0.0f;
    /// <summary>
    /// �ٽ� �������� ���� �ð� ����
    /// </summary>
    [SerializeField]
    float m_canDamageTime = 0.0f;

    /// <summary>
    /// ���� ������ ��ȣ�ۿ� ��ư
    /// </summary>
    private GameObject m_InteractBtnObj = null;
    /// <summary>
    /// ���� ����ִ� ����
    /// </summary>
    private Scaffold m_groundScaffold = null;
    /// <summary>
    /// ui�Ŵ���
    /// </summary>
    private UIManager m_uiManager = null;
    /// <summary>
    /// ������ٵ�
    /// </summary>
    private Rigidbody m_rigidbody = null;
    /// <summary>
    /// �÷��̾� ��Ų ��ġ ����
    /// </summary>
    private Vector3 m_viewPlayerGenPos = new Vector3(0.0f, 1.0f, 0.0f);
    /// <summary>
    /// ���� ȸ�� ������ Ƚ��
    /// </summary>
    private int m_recoverCount = 0;
    /// <summary>
    /// ���� �ִ� ���� ������
    /// </summary>
    private float m_jumpGauge = 0.0f;
    /// <summary>
    /// ���콺 ȸ���� ����
    /// </summary>
    private float m_mouseX = 0f;
    private float m_mouseY = 0f;
    /// <summary>
    /// ü��
    /// </summary>
    private float m_hp = 0;
    /// <summary>
    /// �ʰ� ������ ü�� ��
    /// </summary>
    private float m_lateHp = 0;
    /// <summary>
    /// �ٴڰ� ���� ������ ��� true
    /// </summary>
    private bool m_groundFlag = false;
    /// <summary>
    /// ��� ���� �������� ��� true
    /// </summary>
    private bool m_reachWallFlag = false;
    /// <summary>
    /// ���� ������ ��� true
    /// </summary>
    private bool m_canJumpFlag = false;
    /// <summary>
    /// ������ ������ ��� true
    /// </summary>
    private bool m_canMoveFlag = true;
    /// <summary>
    /// ������ ������ ���
    /// </summary>
    private bool m_playerControllFlag = true;
    /// <summary>
    /// �������� ���� �� �ִ� ��Ȳ�� ��� true
    /// </summary>
    private bool m_canDamageFlage = true;

    private void OnTriggerStay(Collider other)
    {
        if (RoundManager.Instance.GetIsTiming)
        {
            if (other.tag == "Attack")
            {
                GetDamage(-1);
            }
            else if (other.tag == "MovePlayerAttack")
            {
                if (m_rigidbody.velocity.magnitude >= 0.1f)
                {
                    GetDamage(-1);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            m_reachWallFlag = true;
            m_jumpGauge = m_maxjumpGauge;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            m_reachWallFlag = false;
        }
    }

    /// <summary>
    /// start
    /// </summary>
    private void Start()
    {
        StartSetting();
    }

    private void FixedUpdate()
    {
        if(m_playerControllFlag == true)
        {
            if (m_canMoveFlag == true)
            {
                MoveControll();
                JumpControll();
            }

            ConstantGravity();
        }
    }
    private void Update()
    {
        if (m_playerControllFlag == true)
        {
            CameraControll();
        }

        MouseBtnInputControll();

        if(GameManager.Instance != null)
        {
            KeyboardInputControll();
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    void StartSetting()
    {
        //���� �Ҵ�
        m_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        m_rigidbody = GetComponent<Rigidbody>();

        if (GameManager.Instance != null)
        {
            m_charactorCode = GameManager.Instance.CharacterCode;
            //Ŀ�� ���� ��Ȱ��ȭ
            GameManager.Instance.ChangeCursorState(false);
        }


        //�÷��̾� ��ġ �ʱ�ȭ
        ResetPlayerState();

        //�����̴� �ʱ� ����
        m_uiManager.SetSliders(m_maxHp);

        //ü�� ��ũ Ȱ��ȭ
        StartCoroutine(IESyncHealth());

        //�÷��̾� ��Ų ����
        GenViewPlayer();

    }

    /// <summary>
    /// ���� �߷� �ΰ�
    /// </summary>
    void ConstantGravity()
    {
        if (!m_groundFlag && !m_canJumpFlag)
        {
            Vector3 velocity = m_rigidbody.velocity;
            velocity.y = -m_fallSpeed;
            m_rigidbody.velocity = velocity;
        }
        else
        {
            if (m_groundScaffold != null && !m_reachWallFlag)
            {
                Vector3 _scaffoldMovement = m_groundScaffold.transform.forward * m_groundScaffold.m_speed * Time.fixedDeltaTime;
                m_rigidbody.MovePosition(m_rigidbody.position + _scaffoldMovement);
            }
        }
    }

    /// <summary>
    /// ������ �ΰ�
    /// </summary>
    /// <param name="argManageHp">hp ����</param>
    void GetDamage(float argManageHp)
    {
        if (m_canDamageFlage)
        {
            m_canDamageFlage = false;
            LateHp += argManageHp;
            Invoke("CanDamageFlageTrue", m_canDamageTime);

            GameManager.Instance.SoundManager.PlayEffectSound(RoundManager.Instance.SetRoundData.m_soundData.m_hit);
        }
    }

    /// <summary>
    /// ü�� ȸ��
    /// </summary>
    /// <param name="argManageHp">hp ����</param>
    public void Recover(float argManageHp, bool argIsCheat)
    {
        if(m_recoverCount <= 0 && argIsCheat == false)
        {
            return;
        }

        RecoverCount--;
        Hp += argManageHp;
        LateHp += argManageHp;

        GameManager.Instance.SoundManager.PlayEffectSound(RoundManager.Instance.SetRoundData.m_soundData.m_heal);
    }

    /// <summary>
    /// ���� ü���� �⺻ ü�¿� ����ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator IESyncHealth()
    {
        while (true)
        {
            float _healthDifference = Hp - LateHp;
            if (_healthDifference > 0.1f)
            {
                if (Hp > LateHp)
                {
                    Hp -= 1;
                }
            }
            else
            {
                yield return null;
            }
            float _syncInterval = _healthDifference > 15.0f
                ? Mathf.Max(0.1f, 4.0f / _healthDifference)
                : m_maxHpSyncTime;

            yield return new WaitForSeconds(_syncInterval);
        }
    }

    /// <summary>
    /// �ٽ� ������ �Դ� ���� �����ϰ� ���ִ�
    /// �÷��׸� true�� �ٲ� invoke ���
    /// </summary>
    void CanDamageFlageTrue()
    {
        m_canDamageFlage = true;
    }

    /// <summary>
    /// ����
    /// </summary>
    void JumpControll()
    {
        if (m_canJumpFlag)
        {
            // ���� �Է� ��
            if (Input.GetAxisRaw("Jump") >= 0.1f)
            {
                // ���鿡 ���� ���� ���� ����
                if (m_groundFlag)
                {
                    m_groundFlag = false;
                }

                // �ִ� ���� ���̿� �����ϸ� ���� �ߴ�
                if (m_jumpGauge <= 0.0f)
                {
                    CantJump();
                }
                else
                {
                    m_jumpGauge -= 0.01f;
                    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, m_jumpForce, m_rigidbody.velocity.z);
                }
            }
            // ���� �Է� ����
            else
            {
                if (!m_groundFlag)
                {
                    CantJump();
                }
            }
        }
    }

    /// <summary>
    /// ���� �Է� ����
    /// </summary>
    void CantJump()
    {
        m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0.0f, m_rigidbody.velocity.z);
        m_canJumpFlag = false;
    }

    /// <summary>
    /// �̵��� ���� Ű �Է� �� ����
    /// </summary>
    void MoveControll()
    {
        Vector3 _vector = new Vector3();
        _vector.x = Input.GetAxisRaw("Horizontal");
        _vector.z = Input.GetAxisRaw("Vertical");

        _vector = transform.TransformDirection(_vector);
        Vector3 velocity = m_rigidbody.velocity;

        velocity.x = _vector.x * m_moveSpeed;
        velocity.z = _vector.z * m_moveSpeed;

        m_rigidbody.velocity = velocity;
    }

    /// <summary>
    /// �÷��̾� ī�޶� ��Ʈ��
    /// </summary>
    void CameraControll()
    {
        m_mouseX += Input.GetAxis("Mouse X") * m_mouseSensitivity;
        m_mouseY += Input.GetAxis("Mouse Y") * m_mouseSensitivity;
        m_mouseY = Mathf.Clamp(m_mouseY, m_camMinXRotation, m_camMaxXRotation);

        transform.localEulerAngles = new Vector3(0.0f, m_mouseX, 0.0f);
        m_cameraBaseTransform.localEulerAngles = new Vector3(-m_mouseY, 0.0f, 0.0f);

        m_cameraBaseTransform.transform.position = transform.position;
    }

    /// <summary>
    /// ���콺 ��ư Ŭ�� ����
    /// </summary>
    void MouseBtnInputControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(m_InteractBtnObj != null)
            {
                if (m_InteractBtnObj.name == "StartBtn")
                {
                    if (RoundManager.Instance.GetIsGameOver)
                    {
                        RoundManager.Instance.RestartGame();
                        ResetPlayerState();
                    }
                    else
                    {
                        m_InteractBtnObj.GetComponent<MeshRenderer>().material = m_standardInteractMat;
                        m_InteractBtnObj = null;
                        RoundManager.Instance.PhaseStart();
                    }
                }
                else if (m_InteractBtnObj.name == "RecoverBtn")
                {
                    Recover(m_recoverHp, false);
                }
            }
        }
    }
    /// <summary>
    /// Ű���� �Է� ����
    /// </summary>
    void KeyboardInputControll()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.OptionManager.OptionState(true);
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            m_playerControllFlag = false;
            GameManager.Instance.ChangeCursorState(true);
        }
        else
        {
            if(GameManager.Instance.OptionManager.GetOptionState == false)
            {
                m_playerControllFlag = true;
                GameManager.Instance.ChangeCursorState(false);
            }
        }
    }

    /// <summary>
    /// �÷��̾� ��Ų ����
    /// </summary>
    void GenViewPlayer()
    {
        GameObject _obj = Instantiate(GameManager.Instance.GetCharactorData(m_charactorCode).m_object, transform);
        _obj.transform.localPosition = m_viewPlayerGenPos;
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="argState">���� ���� ����</param>
    public void JumpState(bool argState)
    {
        if(argState == true)
        {
            IsCanJumpFlag = true;
            IsGroundFlag = true;
            m_jumpGauge = m_maxjumpGauge;
        }
        else
        {
            IsCanJumpFlag = false;
            IsGroundFlag = false;
        }
    }

    /// <summary>
    /// �÷��̾� ��ġ �ʱ�ȭ
    /// </summary>
    public void ResetPlayerPosition()
    {
        transform.position = m_playerResetPos;
    }

    /// <summary>
    /// �÷��̾� ���� �ʱ�ȭ
    /// </summary>
    public void ResetPlayerState()
    {
        ResetPlayerPosition();
        Hp = m_maxHp;
        LateHp = m_maxHp;
        RecoverCount = m_maxRecoverCount;
    }

    public Material StandardInteractMat
    {
        get { return m_standardInteractMat; }
    }
    public Material ActiveInteractMat
    {
        get { return m_activeInteractMat; }
    }
    public GameObject InteractBtnObj
    {
        get { return m_InteractBtnObj; }
        set { m_InteractBtnObj = value; }
    }
    public Scaffold GroundScaffold
    {
        get { return m_groundScaffold; }
        set { m_groundScaffold = value; }
    }
    public bool IsCanDamageFlage
    {
        get { return m_canDamageFlage; }
        set { m_canDamageFlage = value; }
    }
    public bool IsCanJumpFlag
    {
        get { return m_canJumpFlag; }
        set { m_canJumpFlag = value; }
    }
    public bool IsGroundFlag
    {
        get { return m_groundFlag; }
        set { m_groundFlag = value; }
    }
    public bool CanMoveFlage
    {
        get { return m_canMoveFlag; }
        set { m_canMoveFlag = value; }
    }
    public bool PlayerControllFlag
    {
        get { return m_playerControllFlag; }
        set { m_playerControllFlag = value; }
    }
    public bool Gravity
    {
        get { return m_rigidbody.useGravity; }
        set { m_rigidbody.useGravity = value; }
    }
    public int RecoverCount
    {
        get { return m_recoverCount; }
        set
        {
            m_recoverCount = value;

            if(m_recoverCount <= 0)
            {
                m_recoverCount = 0;
            }
            if(m_recoverCount >= m_maxRecoverCount)
            {
                m_recoverCount = m_maxRecoverCount;
            }

            RoundManager.Instance.GetUIObjManager.SetRecoverCountTextObj(m_recoverCount);
        }
    }
    public float Hp
    {
        get 
        { 
            return m_hp; 
        }
        private set
        {
            m_hp = value;
            if (m_hp <= 0)
            {
                RoundManager.Instance.GameOver(false);
            }
            else if(m_hp > m_maxHp)
            {
                m_hp = m_maxHp;
            }
            m_uiManager.HpSlider(m_hp);
        }
    }
    public float LateHp
    {
        get
        {
            return m_lateHp;
        }
        private set
        {
            m_lateHp = value;
            if (m_lateHp <= 1)
            {
                m_lateHp = 1;
                Hp -= 1;
            }
            else if (m_lateHp > m_maxHp)
            {
                m_lateHp = m_maxHp;
            }
            m_uiManager.LateHpSlider(m_lateHp);
        }
    }
}
