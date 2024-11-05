using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("Round Select Button")]
    /// <summary>
    /// ���� ���� ��ư ������
    /// </summary>
    [SerializeField]
    GameObject m_roundSelectBtnPrefeb = null;
    /// <summary>
    /// ���� ����Ʈ ��ư �ִ� ������
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnMaxSize = 5.0f;
    /// <summary>
    /// ���� ����Ʈ ��ư �ּ� ������
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnMinSize = 3.0f;
    /// <summary>
    /// �� ��ư ���� ���
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnDistance = 1.0f;
    /// <summary>
    /// ���� ���� ��ư x ũ��
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnXSize = 10.0f;

    [Header("Scroll")]
    /// <summary>
    /// ��ũ�� �ӵ�
    /// </summary>
    [SerializeField]
    float m_scrollSpeed = 1000f;
    /// <summary>
    /// ��ũ�� �ε巯���� ����
    /// </summary>
    [SerializeField]
    float m_scrollSmooth = 0.2f;

    /// <summary>
    /// ��׶��� ���� ����� ����� �ҽ�
    /// </summary>
    private AudioSource m_audioSource = null;
    /// <summary>
    /// ���� ���� ǥ�� �ؽ�Ʈ
    /// </summary>
    private TextMeshPro m_roundText = null;
    /// <summary>
    /// �ش� ��ư ����
    /// </summary>
    private RoundSelectBtn m_focusedBtn = null;
    /// <summary>
    /// ���� ���� ��ư ������Ʈ �׷�
    /// </summary>
    private Transform m_roundSelectBtnParentTransform = null;
    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    private Vector3 m_scrollVelocity = Vector3.zero;
    /// <summary>
    /// �巡�� ���� ������ ���콺 ��ġ
    /// </summary>
    private Vector3 dragStartPos;
    /// <summary>
    /// �ִ� ��ũ�� ��ġ
    /// </summary>
    private float m_minScrollPos = 0.0f;
    /// <summary>
    /// �巡�� ������ ����
    /// </summary>
    private bool isDragging = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "InteractBtn")
        {
            m_focusedBtn = other.GetComponent<RoundSelectBtn>();
            m_roundText.text = GameManager.Instance.GetRoundData(m_focusedBtn.GetRoundIndex).m_roundName;
        }
    }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_roundText = GameObject.Find("RoundText").GetComponent<TextMeshPro>();

        RoundSelectBtnSetting();
    }

    void Update()
    {
        ScrollMenu();
        ScrollMenuWithDrag();
        FocusedBtnManage();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void StartRound()
    {
        GameManager.Instance.GoMainScene(m_focusedBtn.GetRoundIndex);
    }

    /// <summary>
    /// ���� ��ư �ʱ� ����
    /// </summary>
    void RoundSelectBtnSetting()
    {
        int _roundDataListCount = GameManager.Instance.GetRoundDataList.Count;
        m_minScrollPos = -(_roundDataListCount - 1) * (m_roundSelectBtnXSize + m_roundSelectBtnDistance);

        m_roundSelectBtnParentTransform = new GameObject("RoundSelectBtnParent").transform;
        m_roundSelectBtnParentTransform.position = new Vector3(0.0f, 3.0f, -2.5f);

        for(int i = 0; i < _roundDataListCount; i++)
        {
            RoundData _roundData = GameManager.Instance.GetRoundData(i);
            GameObject _selectBtnObj = Instantiate(m_roundSelectBtnPrefeb, m_roundSelectBtnParentTransform);
            _selectBtnObj.transform.localPosition = new Vector3(i * (m_roundSelectBtnXSize + m_roundSelectBtnDistance), 0.0f, 0.0f);
            _selectBtnObj.GetComponent<RoundSelectBtn>().SetRoundSelectBtn(this, _roundData.m_roundSprite, _roundData.m_roundIndex);
        }
    }

    /// <summary>
    /// ���� ���� �޴� ��ũ�� ����
    /// </summary>
    void ScrollMenu()
    {
        // ��ǥ ��ġ�� m_roundSelectBtnParentTransform.position���� �ʱ�ȭ
        Vector3 _targetPosition = m_roundSelectBtnParentTransform.position;

        // ���콺 ��ũ��
        float _mouseScroll = Input.GetAxis("Mouse ScrollWheel") * 50;
        _targetPosition += m_roundSelectBtnParentTransform.right * _mouseScroll * 100.0f;

        // ��ġ �Է�
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Moved)
            {
                float _moveX = _touch.deltaPosition.x * m_scrollSpeed * Time.deltaTime * 100.0f;
                _targetPosition += new Vector3(-_moveX, 0, 0);
            }
        }

        // Ű���� �Է�
        float _moveXKeys = Input.GetAxis("Horizontal") * m_scrollSpeed * Time.deltaTime * 100.0f;
        _targetPosition += new Vector3(-_moveXKeys, 0, 0);

        _targetPosition.x = Mathf.Clamp(_targetPosition.x, m_minScrollPos, 0.0f);
        float _distanceToTarget = Vector3.Distance(m_roundSelectBtnParentTransform.position, _targetPosition);
        float _smoothTime = _distanceToTarget < 1f ? 0.1f : m_scrollSmooth;
        m_roundSelectBtnParentTransform.position = Vector3.SmoothDamp(m_roundSelectBtnParentTransform.position, _targetPosition, ref m_scrollVelocity, _smoothTime);
    }
    /// <summary>
    /// ���� ���� �޴� ���콺 �巡�׷� ��ũ��
    /// </summary>
    void ScrollMenuWithDrag()
    {
        // �巡�� ����
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            isDragging = true;
        }

        // �巡�� ��
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 dragDelta = currentMousePos - dragStartPos;

            m_roundSelectBtnParentTransform.position += new Vector3(dragDelta.x * Time.deltaTime * m_scrollSpeed / 7, 0, 0);
            dragStartPos = currentMousePos;
        }

        // �巡�� ����
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        Vector3 clampedPos = m_roundSelectBtnParentTransform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, m_minScrollPos, 0.0f);
        m_roundSelectBtnParentTransform.position = clampedPos;
    }
    /// <summary>
    /// ������ ��ư(�߾ӿ� ����� ��ư) ����
    /// </summary>
    void FocusedBtnManage()
    {
        if (m_focusedBtn != null)
        {
            m_roundSelectBtnParentTransform.position = Vector3.MoveTowards(
                m_roundSelectBtnParentTransform.position,
                new Vector3(-m_focusedBtn.transform.localPosition.x, m_roundSelectBtnParentTransform.position.y, m_roundSelectBtnParentTransform.position.z),
                10.0f * Time.deltaTime);
        }
    }

    float GetRoundSelectBtnMaxSize
    {
        get { return m_roundSelectBtnMaxSize; }
    }
    float GetRoundSelectBtnMinSize
    {
        get { return m_roundSelectBtnMinSize; }
    }
}
