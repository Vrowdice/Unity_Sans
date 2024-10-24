using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Common")]
    /// <summary>
    /// 기본 메테리얼
    /// 상호작용 되었을 시 메테리얼
    /// </summary>
    [SerializeField]
    Material m_standardInteractMat = null;
    [SerializeField]
    Material m_activeInteractMat = null;
    /// <summary>
    /// 최대 회복 횟수
    /// </summary>
    [SerializeField]
    int m_maxRecoverCount = 2;
    /// <summary>
    /// 회복 할 hp 계수
    /// </summary>
    [SerializeField]
    float m_recoverHp = 40.0f;


    [Header("Camera")]
    /// <summary>
    /// 플레이어 3인칭 카메라 위치 베이스 위치
    /// </summary>
    [SerializeField]
    Transform m_cameraBaseTransform = null;
    /// <summary>
    /// 마우스 감도
    /// </summary>
    [SerializeField]
    float m_mouseSensitivity = 1.0f;
    /// <summary>
    /// 카메라 제한 각도
    /// </summary>
    [SerializeField]
    float m_camMinXRotation = -30.0f;
    [SerializeField]
    float m_camMaxXRotation = 50.0f;

    [Header("Transform")]
    /// <summary>
    /// 중력 속도
    /// </summary>
    [SerializeField]
    public float m_fallSpeed = 12f;
    /// <summary>
    /// 점프 속도
    /// </summary>
    [SerializeField]
    float m_jumpSpeed = 15.0f;
    /// <summary>
    /// 최대 점프 높이
    /// </summary>
    [SerializeField]
    float m_maxJumpHight = 10.0f;
    /// <summary>
    /// 직선 움직임 속도
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 50.0f;

    [Header("Player Infomation")]
    /// <summary>
    /// 최대 체력
    /// </summary>
    [SerializeField]
    int m_maxHp = 0;
    /// <summary>
    /// hp와 late hp 동기화 분기 시간
    /// </summary>
    [SerializeField]
    float m_maxHpSyncTime = 0.0f;
    /// <summary>
    /// 다시 데미지를 입을 시간 간격
    /// </summary>
    [SerializeField]
    float m_canDamageTime = 0.0f;

    /// <summary>
    /// 현재 접근한 상호작용 버튼
    /// </summary>
    private GameObject m_InteractBtnObj = null;
    /// <summary>
    /// 현재 딛고있는 발판
    /// </summary>
    private Scaffold m_groundScaffold = null;
    /// <summary>
    /// ui매니저
    /// </summary>
    private UIManager m_uiManager = null;
    /// <summary>
    /// 리지드바디
    /// </summary>
    private Rigidbody m_rigidbody = null;
    /// <summary>
    /// 현재 회복 가능한 횟수
    /// </summary>
    private int m_recoverCount = 0;
    /// <summary>
    /// 현재 최대 점프 가능 위치
    /// </summary>
    private float m_maxJumpPos = 0.0f;
    /// <summary>
    /// 마우스 회전값 저장
    /// </summary>
    private float m_mouseX = 0f;
    private float m_mouseY = 0f;
    /// <summary>
    /// 체력
    /// </summary>
    private float m_hp = 0;
    /// <summary>
    /// 늦게 정해질 체력 값
    /// </summary>
    private float m_lateHp = 0;
    /// <summary>
    /// 바닥과 접촉 판정일 경우 true
    /// </summary>
    private bool m_groundFlag = false;
    /// <summary>
    /// 어느 벽과 접촉중일 경우 true
    /// </summary>
    private bool m_reachWallFlag = false;
    /// <summary>
    /// 점프 가능한 경우 true
    /// </summary>
    private bool m_canJumpFlag = false;
    /// <summary>
    /// 움직임 가능한 경우 true
    /// </summary>
    private bool m_canMoveFlage = true;
    /// <summary>
    /// 데미지를 입을 수 있는 상황인 경우 true
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
        if (m_canMoveFlage)
        {
            MoveControll();
            JumpControll();
        }

        ConstantGravity();
        CameraControll();
    }
    private void Update()
    {
        MouseBtnDownInputControll();
    }

    /// <summary>
    /// 시작 세팅
    /// </summary>
    void StartSetting()
    {
        m_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        m_rigidbody = GetComponent<Rigidbody>();

        ResetPlayerState();
        m_uiManager.SetSliders(m_maxHp);

        StartCoroutine(IESyncHealth());
    }

    /// <summary>
    /// 정속 중력 부과
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
    /// 데미지 부과
    /// </summary>
    /// <param name="argManageHp">hp 감소</param>
    void GetDamage(float argManageHp)
    {
        if (m_canDamageFlage)
        {
            m_canDamageFlage = false;
            GetLateHp += argManageHp;
            Invoke("CanDamageFlageTrue", m_canDamageTime);

            GameManager.Instance.GetSoundManager.PlayEffectSound(GameManager.Instance.GetRoundData.m_soundData.m_hit);
        }
    }

    /// <summary>
    /// 체력 회복
    /// </summary>
    /// <param name="argManageHp">hp 증가</param>
    void Recover(float argManageHp)
    {
        if(m_recoverCount <= 0)
        {
            return;
        }

        GetRecoverCount--;
        GetHp += argManageHp;
        GetLateHp += argManageHp;

        GameManager.Instance.GetSoundManager.PlayEffectSound(GameManager.Instance.GetRoundData.m_soundData.m_heal);
    }

    /// <summary>
    /// 보조 체력을 기본 체력에 동기화하는 코루틴
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
            }

            float _syncInterval = _healthDifference > 15.0f ? Mathf.Max(0.1f, 4.0f / _healthDifference) : m_maxHpSyncTime;
            yield return new WaitForSeconds(_syncInterval);
        }
    }

    /// <summary>
    /// 다시 데미지 입는 것이 가능하게 해주는
    /// 플래그를 true로 바꿈 invoke 사용
    /// </summary>
    void CanDamageFlageTrue()
    {
        m_canDamageFlage = true;
    }

    /// <summary>
    /// 점프
    /// </summary>
    void JumpControll()
    {
        if (m_canJumpFlag)
        {
            //점프 입력 중
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
            //점프 입력 없음
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
    /// 점프 입력 제한
    /// </summary>
    void CantJump()
    {
        m_canJumpFlag = false;
    }

    /// <summary>
    /// 이동을 위한 키 입력 및 전송
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
    /// 플레이어 카메라 컨트롤
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
    /// 마우스 버튼 클릭 설정
    /// </summary>
    void MouseBtnDownInputControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(m_InteractBtnObj != null)
            {
                if (m_InteractBtnObj.name == "NextBtn")
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

    /// <summary>
    /// 플레이어 상태 초기화
    /// </summary>
    public void ResetPlayerState()
    {
        transform.position = new Vector3(0.0f, 4.0f, 0.0f);
        GetHp = m_maxHp;
        GetLateHp = m_maxHp;
        GetRecoverCount = m_maxRecoverCount;
    }

    public GameObject GetInteractBtnObj
    {
        get { return m_InteractBtnObj; }
        set { m_InteractBtnObj = value; }
    }
    public Scaffold GetGroundScaffold
    {
        get { return m_groundScaffold; }
        set { m_groundScaffold = value; }
    }
    public Material GetStandardInteractMat
    {
        get { return m_standardInteractMat; }
    }
    public Material GetActiveInteractMat
    {
        get { return m_activeInteractMat; }
    }
    public bool GetIsCanDamageFlage
    {
        get { return m_canDamageFlage; }
        set { m_canDamageFlage = value; }
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
    public int GetRecoverCount
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
    public float GetHp
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
            else if (m_lateHp >= m_maxHp)
            {
                m_lateHp = m_maxHp;
            }
            m_uiManager.LateHpSlider(m_lateHp);
        }
    }
}
