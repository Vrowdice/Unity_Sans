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
    float m_jumpSpeed = 15.0f;
    /// <summary>
    /// �ִ� ���� ����
    /// </summary>
    [SerializeField]
    float m_maxJumpHight = 10.0f;
    /// <summary>
    /// ���� ������ �ӵ�
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 50.0f;

    [Header("Player Infomation")]
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
    /// ���� ȸ�� ������ Ƚ��
    /// </summary>
    private int m_recoverCount = 0;
    /// <summary>
    /// ���� �ִ� ���� ���� ��ġ
    /// </summary>
    private float m_maxJumpPos = 0.0f;
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
        if (GameManager.Instance.GetIsTiming)
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
            CameraControll();
        }
    }
    private void Update()
    {
        MouseBtnInputControll();
        KeyboardInputControll();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    void StartSetting()
    {
        m_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        m_rigidbody = GetComponent<Rigidbody>();

        ResetPlayerState();
        m_uiManager.SetSliders(m_maxHp);

        StartCoroutine(IESyncHealth());

        GameDataManager.Instance.CursorState(false);
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
            SetLateHp += argManageHp;
            Invoke("CanDamageFlageTrue", m_canDamageTime);

            GameDataManager.Instance.GetSoundManager.PlayEffectSound(GameManager.Instance.SetRoundData.m_soundData.m_hit);
        }
    }

    /// <summary>
    /// ü�� ȸ��
    /// </summary>
    /// <param name="argManageHp">hp ����</param>
    void Recover(float argManageHp)
    {
        if(m_recoverCount <= 0)
        {
            return;
        }

        SetRecoverCount--;
        SetHp += argManageHp;
        SetLateHp += argManageHp;

        GameDataManager.Instance.GetSoundManager.PlayEffectSound(GameManager.Instance.SetRoundData.m_soundData.m_heal);
    }

    /// <summary>
    /// ���� ü���� �⺻ ü�¿� ����ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator IESyncHealth()
    {
        while (true)
        {
            float _healthDifference = Mathf.Abs(SetHp - SetLateHp);
            if (_healthDifference > 0.1f)
            {
                if (SetHp > SetLateHp)
                {
                    SetHp -= 1;
                }
            }

            float _syncInterval = _healthDifference > 15.0f ? Mathf.Max(0.1f, 4.0f / _healthDifference) : m_maxHpSyncTime;
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
            //���� �Է� ��
            if (Input.GetAxisRaw("Jump") >= 0.1f)
            {
                if (m_groundFlag)
                {
                    m_maxJumpPos = transform.position.y + m_maxJumpHight;
                }

                if (transform.position.y >= m_maxJumpPos - 0.3f)
                {
                    CantJump();
                }

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(transform.position.x, m_maxJumpPos, transform.position.z),
                    m_jumpSpeed * Time.fixedDeltaTime);
            }
            //���� �Է� ����
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
                    if (GameManager.Instance.GetIsGameOver)
                    {
                        GameManager.Instance.RestartGame();
                        ResetPlayerState();
                    }
                    else
                    {
                        m_InteractBtnObj.GetComponent<MeshRenderer>().material = m_standardInteractMat;
                        m_InteractBtnObj = null;
                        GameManager.Instance.PhaseStart();
                    }
                }
                else if (m_InteractBtnObj.name == "RecoverBtn")
                {
                    Recover(m_recoverHp);
                }
            }
        }
    }
    void KeyboardInputControll()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameDataManager.Instance.GetOptionManager.OptionState(true);
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            m_playerControllFlag = false;
            GameDataManager.Instance.CursorState(true);
        }
        else
        {
            if(GameDataManager.Instance.GetOptionManager.GetOptionState == false)
            {
                m_playerControllFlag = true;
                GameDataManager.Instance.CursorState(false);
            }
        }
    }

    /// <summary>
    /// �÷��̾� ���� �ʱ�ȭ
    /// </summary>
    public void ResetPlayerState()
    {
        transform.position = new Vector3(0.0f, 4.0f, 0.0f);
        SetHp = m_maxHp;
        SetLateHp = m_maxHp;
        SetRecoverCount = m_maxRecoverCount;
    }

    public Material GetStandardInteractMat
    {
        get { return m_standardInteractMat; }
    }
    public Material GetActiveInteractMat
    {
        get { return m_activeInteractMat; }
    }
    public GameObject SetInteractBtnObj
    {
        get { return m_InteractBtnObj; }
        set { m_InteractBtnObj = value; }
    }
    public Scaffold SetGroundScaffold
    {
        get { return m_groundScaffold; }
        set { m_groundScaffold = value; }
    }
    public bool SetIsCanDamageFlage
    {
        get { return m_canDamageFlage; }
        set { m_canDamageFlage = value; }
    }
    public bool SetIsCanJumpFlag
    {
        get { return m_canJumpFlag; }
        set { m_canJumpFlag = value; }
    }
    public bool SetIsGroundFlag
    {
        get { return m_groundFlag; }
        set { m_groundFlag = value; }
    }
    public bool SetCanMoveFlage
    {
        get { return m_canMoveFlag; }
        set { m_canMoveFlag = value; }
    }
    public bool SetPlayerControllFlag
    {
        get { return m_playerControllFlag; }
        set { m_playerControllFlag = value; }
    }
    public int SetRecoverCount
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

            GameManager.Instance.GetUIObjManager.SetRecoverCountTextObj(m_recoverCount);
        }
    }
    public float SetHp
    {
        get 
        { 
            return m_hp; 
        }
        set 
        {
            m_hp = value;
            if (m_hp <= 0)
            {
                GameManager.Instance.GameOver(false);
            }
            else if(m_hp >= m_maxHp)
            {
                m_hp = m_maxHp;
            }
            m_uiManager.HpSlider(m_hp);
        }
    }
    public float SetLateHp
    {
        get
        {
            return m_lateHp;
        }
        set
        {
            m_lateHp = value;

            if (m_lateHp <= 1)
            {
                m_lateHp = 1;
                SetHp -= 1;
            }
            else if (m_lateHp >= m_maxHp)
            {
                m_lateHp = m_maxHp;
            }
            m_uiManager.LateHpSlider(m_lateHp);
        }
    }
}
