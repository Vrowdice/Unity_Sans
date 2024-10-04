using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Common")]
    /// <summary>
    /// �÷��̾� �ݸ��� ��ġ
    /// </summary>
    [SerializeField]
    Transform m_viewPlayerTransform = null;
    /// <summary>
    /// �⺻ ���׸���
    /// ��ȣ�ۿ� �Ǿ��� �� ���׸���
    /// </summary>
    [SerializeField]
    Material m_standardInteractMat = null;
    [SerializeField]
    Material m_activeInteractMat = null;

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
    /// ���� �ӵ�
    /// </summary>
    [SerializeField]
    float m_jumpSpeed = 50.0f;
    /// <summary>
    /// �ִ� ���� ����
    /// </summary>
    [SerializeField]
    float m_MaxJumpHight = 50.0f;
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
    /// ui�Ŵ���
    /// </summary>
    private UIManager m_uiManager = null;
    /// <summary>
    /// ������ٵ�
    /// </summary>
    private Rigidbody m_rigidbody;
    /// <summary>
    /// ���콺 ȸ���� ����
    /// </summary>
    private float m_mouseX = 0f;
    private float m_mouseY = 0f;
    /// <summary>
    /// �ٴڰ� ���� ������ ��� true
    /// </summary>
    private bool m_groundFlag = false;
    /// <summary>
    /// ���� ������ ��� true
    /// </summary>
    private bool m_canJumpFlag = false;
    /// <summary>
    /// ������ ������ ��� true
    /// </summary>
    private bool m_canMoveFlage = true;
    /// <summary>
    /// �������� ���� �� �ִ� ��Ȳ�� ��� true
    /// </summary>
    private bool m_canDamageFlage = true;
    /// <summary>
    /// ü��
    /// </summary>
    private float m_hp = 0;
    /// <summary>
    /// �ʰ� ������ ü�� ��
    /// </summary>
    private float m_lateHp = 0;

    /// <summary>
    /// start
    /// </summary>
    private void Start()
    {
        m_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_hp = m_maxHp;
        m_lateHp = m_maxHp;

        m_uiManager.SetSliders(m_maxHp);

        StartCoroutine(IESyncHealth());
    }
    /// <summary>
    /// update
    /// </summary>
    private void Update()
    {
        if (m_canMoveFlage)
        {
            MoveControll();
            JumpControll();
        }

        CameraControll();
        MouseBtnDownInputControll();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Attack")
        {
            GetDamage(-1);
        }
        else if(other.tag == "MovePlayerAttack")
        {
            if(m_rigidbody.velocity.magnitude >= 0.1f)
            {
                GetDamage(-1);
            }
        }
    }
    /// <summary>
    /// ������ �ΰ�
    /// </summary>
    /// <param name="argManageHp"></param>
    void GetDamage(float argManageHp)
    {
        if (m_canDamageFlage)
        {
            m_canDamageFlage = false;
            GetLateHp += argManageHp;
            Invoke("CanDamageFlageTrue", m_canDamageTime);
        }
    }

    /// <summary>
    /// ���� ü���� �⺻ ü�¿� ����ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator IESyncHealth()
    {
        while (true)
        {
            float _healthDifference = Mathf.Abs(GetHp - GetLateHp);
            if (_healthDifference > 0.1f)
            {
                if (GetHp > GetLateHp)
                {
                    GetHp -= 1;
                }
                else if (GetHp < GetLateHp)
                {
                    GetHp += 1;
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
        //���� �Է� ��
        if(Input.GetAxisRaw("Jump") != 0.0f)
        {
            if (m_canJumpFlag)
            {
                if (transform.position.y >= m_MaxJumpHight)
                {
                    CantJump();
                }
                m_rigidbody.useGravity = false;
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(transform.position.x, m_MaxJumpHight, transform.position.z),
                    m_jumpSpeed * Time.deltaTime);
            }
            else
            {
                if (transform.position.y >= m_MaxJumpHight)
                {
                    CantJump();
                }
            }
        }
        //���� �Է� ����
        else if(Input.GetAxisRaw("Jump") <= 0.0f)
        {
            if (m_canJumpFlag)
            {
                if (!m_groundFlag)
                {
                    m_rigidbody.useGravity = true;
                    CantJump();
                }
            }
            else
            {
                CantJump();
            }
        }
    }
    /// <summary>
    /// ���� �Է� ����
    /// </summary>
    void CantJump()
    {
        m_rigidbody.useGravity = true;
        m_canJumpFlag = false;
    }

    /// <summary>
    /// �̵��� ���� Ű �Է� �� ����
    /// </summary>s
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
    void MouseBtnDownInputControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_InteractBtnObj != null && m_InteractBtnObj.name == "NextBtn")
            {
                GameManager.Instance.PhaseStart();
                m_InteractBtnObj.GetComponent<MeshRenderer>().material = m_standardInteractMat;
                m_InteractBtnObj = null;
            }
        }
    }

    public Material GetStandardInteractMat
    {
        get { return m_standardInteractMat; }
    }
    public Material GetActiveInteractMat
    {
        get { return m_activeInteractMat; }
    }
    public bool GetIsCanJumpFlag
    {
        get { return m_canJumpFlag; }
        set { m_canJumpFlag = value; }
    }
    public bool GetIsGroundFlag
    {
        get { return m_groundFlag; }
        set { m_groundFlag = value; }
    }
    public bool GetCanMoveFlage
    {
        get { return m_canMoveFlage; }
        set { m_canMoveFlage = value; }
    }
    public GameObject GetInteractBtnObj
    {
        get { return m_InteractBtnObj; }
        set { m_InteractBtnObj = value; }
    }
    public float GetHp
    {
        get 
        { 
            return m_hp; 
        }
        set 
        {
            m_hp = value;
            if (GetHp <= 0)
            {
                GameManager.Instance.GameOver();
            }
            m_uiManager.HpSlider(m_hp);
        }
    }
    public float GetLateHp
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
                GetHp -= 1;
            }

            m_uiManager.LateHpSlider(m_lateHp);
        }
    }
}
