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
    Transform m_viewPlayerTransform;

    [Header("Camera")]
    /// <summary>
    /// �÷��̾� 3��Ī ī�޶� ��ġ ���̽� ��ġ
    /// </summary>
    [SerializeField]
    Transform m_cameraBaseTransform;
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
    float m_hpSyncTime = 0.0f;

    /// <summary>
    /// �ٽ� �������� ���� �ð� ����
    /// </summary>
    [SerializeField]
    float m_canDamageTime = 0.0f;


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
    /// hp�� ��ũ���� ��� true
    /// </summary>
    private bool m_hpSyncFlag = false;

    /// <summary>
    /// ü��
    /// </summary>
    private int m_hp = 0;

    /// <summary>
    /// �ʰ� ������ ü�� ��
    /// </summary>
    private int m_lateHp = 0;

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
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            if (m_canDamageFlage)
            {
                m_canDamageFlage = false;
                IsLateHp -= 1;
                Invoke("CanDamageFlageTrue", m_canDamageTime);
            }
        }
    }

    /// <summary>
    /// hp�� late hp�� ��ũ ����
    /// invoke repeat ���
    /// </summary>
    void HpSync()
    {
        if (m_hpSyncFlag)
        {
            if(IsHp >= IsLateHp || IsLateHp <= 1)
            {
                m_hpSyncFlag = false;
                CancelInvoke("HpSync");
            }
            else
            {
                IsHp -= 1;
            }
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
    public bool IsCanMoveFlage
    {
        get { return m_canMoveFlage; }
        set { m_canMoveFlage = value; }
    }
    public int IsHp
    {
        get 
        { 
            return m_hp; 
        }
        set 
        {
            m_hp = value;
            if (IsHp <= 0)
            {
                //gameover
                Debug.Log("gameOver");
            }
            m_uiManager.HpSlider(m_hp);
        }
    }
    public int IsLateHp
    {
        get
        {
            return m_lateHp;
        }
        set
        {
            m_lateHp = value;

            if (!m_hpSyncFlag)
            {
                if(m_hp != m_lateHp)
                {
                    m_hpSyncFlag = true;
                    InvokeRepeating("HpSync", m_canDamageTime, m_hpSyncTime);
                }
            }

            if (IsHp - 20 >= IsLateHp)
            {
                IsHp -= 1;
            }
            if (m_lateHp <= 1)
            {
                m_lateHp = 1;
                IsHp -= 1;
            }

            m_uiManager.LateHpSlider(m_lateHp);
        }
    }
}
