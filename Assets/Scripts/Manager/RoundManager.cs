using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�� �������� ���� ������ �� ���� �������� ó���� Ȯ���ϱ� ���� �ʿ�
//m_type
//BlueSimpleAtk(ĳ���Ͱ� �����̸� ������ ���� �ϳ� ���� Vector.forward �������η� �̵�),
//SimpleAtk(�Ϲ� �ܼ� ���ٱ� ���� ���� ���� �ϳ� ���� Vector.forward �������η� �̵�),
//PopAtk(���鿡 ��ü ���ٱ� ������ ����),
//RangeAtk(������ ������ ���� ���������� ���� forward �������� ������ ����),
//GravityAtk(�� �������� ����ٰ� �浹��Ŵ ������ 12�ú��� �ð�������� �迭
//180.0f�� 0�� ����, -90.0f�� 1�� ����, 0.0f�� 2�� ����, 90.0f�� 3������),
//scaffold(������ ������ Vector.forward �������� �̵�),
//m_isMove
//�����̴� ������Ʈ�� ��� 0 = false, 1 = true
//m_genTime
//����� ���۵� �ķκ��� ���� �ð��� ���� �� gentime���� ��ü�� ������ ���� ������ gen time�� �� �����ϴ� ���� �ʿ�
//m_sizeY
//���� ������Ʈ ������
//m_position
//���� ��ġ �÷��̾ ������ ������ 40 * 40�� ������ �����̴� ������Ʈ�� �� �ۿ� �����ϴ� ���� �Ϲ���
//m_rotation
//���� ������ �����ϸ� �����̴� ������ �����ϰ� ��ü�� �������� ������ �����ؼ� ������ ������ ���� ���� �ְ� ������ ������ ���� ���� ����


public class RoundManager : MonoBehaviour
{
    /// <summary>
    /// game manager
    /// </summary>
    static RoundManager g_gameManager;

    [Header("Common")]
    /// <summary>
    /// ���� ���� ������
    /// �ش� �����ͷ� ������ �����͸� �ҷ���
    /// 
    /// ������ �ø���������
    /// </summary>
    [SerializeField]
    RoundData m_roundData = null;
    /// <summary>
    /// ���� ������
    /// �������� ����� ǥ���ϴ� ���� �ƴ�
    /// ������ �ҷ��� �������� ����� ǥ����
    /// 
    /// ������ �ø���������
    /// </summary>
    [SerializeField]
    int m_phase = 0;

    [Header("Wall")]
    /// <summary>
    /// �� ��ȯ �ӵ�
    /// </summary>
    [SerializeField]
    float m_wallDirChangeSpeed = 150.0f;
    /// <summary>
    /// �� ���� ũ��
    /// </summary>
    [SerializeField]
    int m_wallHalfSize = 20;
    /// <summary>
    /// �� ���׸���
    /// </summary>
    [SerializeField]
    Material m_wallMat = null;
    /// <summary>
    /// ������ �� ���׸���
    /// </summary>
    [SerializeField]
    Material m_transparentWallMat = null;

    [Header("Atteck")]
    /// <summary>
    /// Ƣ������� ���� ��� ������Ʈ
    /// </summary>
    [SerializeField]
    GameObject m_popWarningObj = null;
    /// <summary>
    /// �⺻ ���׸���
    /// </summary>
    [SerializeField]
    Material m_atkObjMat = null;
    /// <summary>
    /// �����̴� �÷��̾� ���� ���׸���
    /// </summary>
    [SerializeField]
    Material m_movePlayerAtkObjMat = null;
    /// <summary>
    /// ��� ���׸���
    /// </summary>
    [SerializeField]
    Material m_warningMat = null;

    [Header("Scaffold")]
    /// <summary>
    /// ���� ������Ʈ ������
    /// </summary>
    [SerializeField]
    GameObject m_scaffoldObj = null;
    /// <summary>
    /// �ִ� ���� ������Ʈ ��
    /// </summary>
    [SerializeField]
    int m_scaffoldObjCount = 50;

    [Header("Simple Atteck")]
    /// <summary>
    /// ���� ������Ʈ ������
    /// </summary>
    [SerializeField]
    GameObject m_simpleAtkObj = null;
    /// <summary>
    /// �ִ� ���� ������Ʈ ��
    /// </summary>
    [SerializeField]
    int m_simpleAtkObjCount = 200;
    /// <summary>
    /// ���� ������Ʈ �⺻ ������
    /// </summary>
    [SerializeField]
    int m_simpleAtkObjBasicSize = 3;


    [Header("Range Atteck")]
    [SerializeField]
    GameObject m_rangeAtkObj = null;
    [SerializeField]
    int m_rangeAtkObjCount = 30;
    /// <summary>
    /// ���Ÿ� ���� ��� �ð�
    /// </summary>
    [SerializeField]
    float m_rangeAtkWarnTime = 2.0f;
    /// <summary>
    /// ���Ÿ� ���� ���� �ð�
    /// </summary>
    [SerializeField]
    float m_rangeAtkTime = 2.0f;

    [Header("Pop Atteck")]
    /// <summary>
    /// Ƣ������� ���� �ִ� ����
    /// </summary>
    [SerializeField]
    float m_popAtkMaxHeight = 0.0f;
    /// <summary>
    /// Ƣ������� ���� ���� Ȱ��ȭ �ð�
    /// </summary>
    [SerializeField]
    float m_popAtkActiveTime = 1.5f;
    /// <summary>
    /// Ƣ������� ���� ���� �ӵ�
    /// </summary>
    [SerializeField]
    float m_popAtkSpeed = 100.0f;
    /// <summary>
    /// Ƣ������� ���� ����ϴ� �ð�
    /// </summary>
    [SerializeField]
    float m_popAtkWarnTime = 1.5f;

    [Header("Gravity Atteck")]
    /// <summary>
    /// �߷� ���� ��(�ӵ�)
    /// </summary>
    [SerializeField]
    float m_gravityAtkSpeed = 20.0f;

    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    /// </summary>
    private PlayerController m_playerController = null;
    /// <summary>
    /// UI ������Ʈ ���� ��ũ��Ʈ
    /// </summary>
    private UIObjManager m_uIObjManager = null;

    /// <summary>
    /// (����)�� �߽�
    /// </summary>
    private Transform m_wallCenterPos = null;
    /// <summary>
    /// (����)�� ����Ʈ
    /// </summary>
    private List<GameObject> m_wallList = new List<GameObject>();
    /// <summary>
    /// �ݺ� ���� ������ ����Ʈ
    /// </summary>
    private List<RepeatAtkData> m_repeatAtkDataList = new List<RepeatAtkData>();
    /// <summary>
    /// �� ���� ������ ����Ʈ
    /// </summary>
    private List<AtkData> m_atkDataList = new List<AtkData>();
    /// <summary>
    /// ��� ����Ʈ�� ���ư��� ���� �ӽ� ���� ����Ʈ
    /// </summary>
    private List<SimpleAtk> m_toWaitSimpleAtkTmpList = new List<SimpleAtk>();
    /// <summary>
    /// ��� ����Ʈ�� ���ư��� ���� �ӽ� ���� ����Ʈ
    /// </summary>
    private List<Scaffold> m_toWaitScaffoldTmpList = new List<Scaffold>();

    /// <summary>
    /// �÷��̾� ������ �ܼ� �����ϴ� ������Ʈ ��� ť
    /// </summary>
    private Queue<SimpleAtk> m_simpleAtkObjWaitQueue = new Queue<SimpleAtk>();
    /// <summary>
    /// ���Ÿ� ���� ������Ʈ ��� ť
    /// </summary>
    private Queue<RangeAtk> m_rangeAtkObjWaitQueue = new Queue<RangeAtk>();
    /// <summary>
    /// ���Ÿ� ���� ������Ʈ ��� ť
    /// </summary>
    private Queue<Scaffold> m_scaffoldObjWaitQueue = new Queue<Scaffold>();

    /// <summary>
    /// ���忡�� Ȱ��ȭ �� �ܼ� ���� ������Ʈ
    /// ����ϰ� ���� �������� ��� ť�� �̵�
    /// </summary>
    private LinkedList<SimpleAtk> m_activeSimpleAtkObjList = new LinkedList<SimpleAtk>();
    /// <summary>
    /// ���忡�� Ȱ��ȭ �� ���Ÿ� ���� ������Ʈ
    /// ����ϰ� ���� �������� ��� ť�� �̵�
    /// </summary>
    private LinkedList<RangeAtk> m_activeRangeAtkObjList = new LinkedList<RangeAtk>();
    /// <summary>
    /// ���忡�� Ȱ��ȭ �� ���� ������Ʈ
    /// ����ϰ� ���� �������� ��� ť�� �̵�
    /// </summary>
    private LinkedList<Scaffold> m_activeScaffoldObjList = new LinkedList<Scaffold>();

    /// <summary>
    /// �����ϴ� ������Ʈ�� ������ ��ġ
    /// </summary>
    private Vector3 m_objBasicPos = new Vector3(300.0f, 0.0f, 0.0f);
    /// <summary>
    /// ���� �÷��̾ ����ϴ� ��
    /// 12�� ���� �ð�������� 0, 1, 2, 3
    /// </summary>
    //private int m_nowWall = 0;
    /// <summary>
    /// ���� �������� ���� ���� ��Ȳ
    /// </summary>
    private int m_atkIndex = 0;
    /// <summary>
    /// ���� ����� ���� �ð�
    /// </summary>
    private float m_phaseTime = 0.0f;
    /// <summary>
    /// ���� ����� ���� �ð� ���
    /// </summary>
    private float m_phaseStartTime = 0.0f;
    /// <summary>
    /// ù��° ��������
    /// </summary>
    private bool m_isFirstStart = true;
    /// <summary>
    /// Ÿ�̹� �������ΰ�
    /// </summary>
    private bool m_isTiming = false;
    /// <summary>
    /// ���� ���� �÷���
    /// </summary>
    private bool m_isGameOver = false;
    /// <summary>
    /// ��� �Ϸ� �÷���
    /// </summary>
    private bool m_ascensionCompleteFlag = false;

    private void OnEnable()
    {
        OnEnableSetting();
    }
    void Start()
    {
        StartSetting();
    }
    private void FixedUpdate()
    {
        MoveObj();
        TimingCheck();
    }

    /// <summary>
    /// �ʱ� ����
    /// </summary>
    private void OnEnableSetting()
    {
        //���� �Ҵ�
        g_gameManager = this;

        //ã�� �� �Ҵ�
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        m_uIObjManager = GameObject.Find("UIObj").GetComponent<UIObjManager>();
        m_wallCenterPos = GameObject.Find("MapCenter").transform.Find("WallCenter").transform;

        //�� ����Ʈ�� �� �Ҵ�
        for(int i = 0; i < 3; i++)
        {
            m_wallList.Add(m_wallCenterPos.GetChild(i).gameObject);
        }

        //���� �ν��Ͻ� �Ҵ�
        if(GameManager.Instance != null)
        {
            m_roundData = GameManager.Instance.GetRoundData(GameManager.Instance.SetRoundIndex);
        }
    }
    /// <summary>
    /// start �ʱ� ����
    /// </summary>
    private void StartSetting()
    {
        GetCSVData();

        //���� ������Ʈ �� ������Ʈ ���� �� ����
        GameObject _simpleAtkParent = new GameObject("SimpleAtkParent");
        GameObject _rangeAtkParent = new GameObject("RangeAtkParent");
        GameObject _scaffoldParent = new GameObject("ScaffoldParent");

        GameObject _obj = null;
        //�� �� ������Ʈ�� ���� ������Ʈ �Ҵ�
        for (int i = 0; i < m_simpleAtkObjCount; i++)
        {
            _obj = Instantiate(m_simpleAtkObj, _simpleAtkParent.transform);
            _obj.transform.position = m_objBasicPos;
            m_simpleAtkObjWaitQueue.Enqueue(_obj.GetComponent<SimpleAtk>());

        }
        for (int i = 0; i < m_rangeAtkObjCount; i++)
        {
            _obj = Instantiate(m_rangeAtkObj, _rangeAtkParent.transform);
            _obj.transform.position = m_objBasicPos;
            m_rangeAtkObjWaitQueue.Enqueue(_obj.GetComponent<RangeAtk>());
        }
        for (int i = 0; i < m_scaffoldObjCount; i++)
        {
            _obj = Instantiate(m_scaffoldObj, _scaffoldParent.transform);
            _obj.transform.position = m_objBasicPos;
            m_scaffoldObjWaitQueue.Enqueue(_obj.GetComponent<Scaffold>());
        }

        //�� �ʱ� ���·� ����
        StartCoroutine(IEChangeWall(0.0f));

        //Ƣ����� ���� ��� ���� �� �ʱ� ���� ����
        _obj = Instantiate(m_popWarningObj);
        m_popWarningObj = _obj;
        m_popWarningObj.SetActive(false);
    }

    /// <summary>
    /// CSV ������ �����ͼ� ����Ʈ�� �����ϰ� �ε� ȭ�� ǥ��
    /// </summary>
    void GetCSVData()
    {
        // ������ �ʱ�ȭ
        int _tmpRepeatAtkIndex = -1;
        int _tmpRepeatAtkOrder = 0;
        m_atkDataList = new List<AtkData>();
        m_repeatAtkDataList = new List<RepeatAtkData>();

        // Phase data name = "Phase + N"
        List<Dictionary<string, object>> _data = CSVReader.Read(m_roundData.m_roundIndex + "/Phase" + m_phase);

        // �����Ͱ� null�̰ų� ������� ��� ó��
        if (_data == null || _data.Count == 0)
        {
            // �����Ͱ� ������ �¸� ó�� �� ����
            GameOver(true);
            return;
        }

        // �����Ͱ� ������ ��� ����Ʈ�� �߰�
        for (int i = 0; i < _data.Count; i++)
        {
            if (_data[i]["order"].ToString() == "")
            {
                continue;
            }

            // ������ ����
            AtkData _atkData = new AtkData
            {
                m_order = int.Parse(_data[i]["order"].ToString()),
                m_type = StrToAtkType(int.Parse(_data[i]["type"].ToString())),
                m_isMove = StrToBool(_data[i]["isMove"].ToString()),
                m_genTime = float.Parse(_data[i]["genTime"].ToString()),
                m_speed = float.Parse(_data[i]["speed"].ToString()),
                m_size = new Vector3(
                    float.Parse(_data[i]["sizeX"].ToString()),
                    float.Parse(_data[i]["sizeY"].ToString()),
                    float.Parse(_data[i]["sizeZ"].ToString())
                ),
                m_position = new Vector3(
                    float.Parse(_data[i]["positionX"].ToString()),
                    float.Parse(_data[i]["positionY"].ToString()),
                    float.Parse(_data[i]["positionZ"].ToString())
                ),
                m_rotation = new Vector3(
                    float.Parse(_data[i]["rotationX"].ToString()),
                    float.Parse(_data[i]["rotationY"].ToString()),
                    float.Parse(_data[i]["rotationZ"].ToString())
                )
            };

            // order�� ������ ��� RepeatAtkData ����
            if (_atkData.m_order < 0)
            {
                if (_tmpRepeatAtkOrder != _atkData.m_order)
                {
                    _tmpRepeatAtkOrder = _atkData.m_order;

                    RepeatAtkData _repeatAtkData = new RepeatAtkData
                    {
                        m_atkData = _atkData,
                        m_repeatStartTime = _atkData.m_genTime,
                        m_repeatTime = 0.0f,
                        m_repeatOverTime = 0.0f
                    };

                    m_repeatAtkDataList.Add(_repeatAtkData);
                    _tmpRepeatAtkIndex++;
                }
                else
                {
                    if (m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatTime == 0.0f)
                    {
                        m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatTime = _atkData.m_genTime - m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatStartTime;
                    }
                    else if (m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatOverTime == 0.0f)
                    {
                        m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatOverTime = _atkData.m_genTime;
                    }
                }
            }
            else
            {
                m_atkDataList.Add(_atkData);
            }
        }

        // �����Ͱ� ���������� �ε�� ��쿡�� PhaseReady ȣ��
        if (m_isFirstStart == true)
        {
            m_uIObjManager.ActiveUIObj(true);
            m_isFirstStart = false;
        }
    }


    /// <summary>
    /// ���� Ÿ�� ���ڿ� �̳����� ����
    /// </summary>
    /// <param name="argStr">���� Ÿ�� ���ڿ�</param>
    /// <returns>���� Ÿ�� �̳�</returns>
    private AtkType StrToAtkType(int argIndex)
    {
        switch (argIndex)
        {
            case 1:
                return AtkType.SimpleAtk;
            case 2:
                return AtkType.BlueSimpleAtk;
            case 3:
                return AtkType.PopAtk;
            case 4:
                return AtkType.RangeAtk;
            case 5:
                return AtkType.GravityAtk;
            case 6:
                return AtkType.Scaffold;
            default:
                Debug.Log(argIndex + " not allowed value");
                return new AtkType();
        }
    }
    /// <summary>
    /// �������� 0, 1 ���ڿ��� bool�������� ��ȯ
    /// </summary>
    /// <param name="argStr">0�̳� 1</param>
    /// <returns>boolen</returns>
    private bool StrToBool(string argStr)
    {
        if(argStr == "0")
        {
            return false;
        }
        else if(argStr == "1")
        {
            return true;
        }

        Debug.Log("Not allowed value");
        return false;
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public void PhaseStart()
    {
        if(m_atkDataList != null && !m_isTiming)
        {
            if (GetPhase == 0)
            {
                GameManager.Instance.GetSoundManager.PlayBackgroundSound(m_roundData.m_soundData.m_backGround);
            }
            m_uIObjManager.ActiveUIObj(false);
            StartTimer();
        }
    }
    /// <summary>
    /// ���۵� ������ ������
    /// </summary>
    public void PhaseOver()
    {
        ResetAllObj();
        
        m_uIObjManager.SetRecoverCountTextObj(m_playerController.SetRecoverCount);

        m_phase++;
        m_phaseTime = 0.0f;
        m_phaseStartTime = 0.0f;
        m_atkIndex = 0;

        m_uIObjManager.ActiveUIObj(true);
        StopTimer();
        GetCSVData();
    }
    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public void GameOver(bool argWinOrDefeat)
    {
        //���� ���� �÷��� ��ȯ
        m_isGameOver = true;

        //�� ����
        if (argWinOrDefeat == true)
        {
            GameManager.Instance.SetMoney += (m_phase - 1) * 10;
        }

        ResetAllObj();

        //���� ���� UI ǥ��
        m_uIObjManager.GameWinState(argWinOrDefeat);

        m_phase = 0;
        GetCSVData();
        StopTimer();
        m_playerController.ResetPlayerState();
    }
    /// <summary>
    /// ���� �����
    /// </summary>
    public void RestartGame()
    {
        m_isGameOver = false;
        m_phase = 0;
        m_phaseTime = 0.0f;
        m_phaseStartTime = 0.0f;
        m_atkIndex = 0;

        PhaseStart();
    }

    /// <summary>
    /// Ÿ�̹� üũ ��ƾ
    /// </summary>
    void TimingCheck()
    {
        if(m_atkDataList == null || !m_isTiming)
        {
            return;
        }

        if(m_roundData.m_phaseOverTime[m_phase] <= m_phaseTime)
        {
            PhaseOver();
        }

        m_phaseStartTime += Time.fixedDeltaTime;
        m_phaseTime = Mathf.Floor(m_phaseStartTime) + Mathf.Round((m_phaseStartTime % 1.0f) * 10.0f) / 10.0f;

        //�ݺ� ���� �۵�
        if (m_repeatAtkDataList.Count > 0)
        {
            for (int i = m_repeatAtkDataList.Count - 1; i >= 0; i--)
            {
                var item = m_repeatAtkDataList[i];

                // �ݺ� ���� ��
                if (item.m_repeatOverTime >= m_phaseTime && item.m_repeatOverTime > 0.0f)
                {
                    m_repeatAtkDataList.RemoveAt(i);
                    continue;
                }

                // ó�� ���� ��
                if (item.m_repeatStartTime + m_roundData.m_phaseStartTimeSet[m_phase] <= m_phaseTime && item.m_toRepeatTime == 0.0f)
                {
                    item.m_toRepeatTime = m_phaseTime + item.m_repeatTime;
                    GenObjAsAtkData(item.m_atkData);
                    continue;
                }

                // �⺻ �ݺ� ����
                if (item.m_toRepeatTime <= m_phaseTime && item.m_repeatStartTime <= m_phaseTime)
                {
                    item.m_toRepeatTime = m_phaseTime + item.m_repeatTime;
                    GenObjAsAtkData(item.m_atkData);
                    continue;
                }
            }

        }

        //�Ϲ� ���� ���� �۵�
        if (m_atkDataList.Count > 0)
        {
            if(m_atkDataList.Count <= m_atkIndex)
            {
                return;
            }
            if (m_atkDataList[m_atkIndex].m_genTime + m_roundData.m_phaseStartTimeSet[m_phase] <= m_phaseTime)
            {
                GenObjAsAtkData(m_atkDataList[m_atkIndex]);
                m_atkIndex++;
            }
        }
    }
    /// <summary>
    /// Ÿ�̸� ����
    /// </summary>
    void StartTimer()
    {
        m_isTiming = true;
        m_phaseStartTime = 0.0f;
        m_phaseTime = 0.0f;
    }
    /// <summary>
    /// Ÿ�̸� ����
    /// </summary>
    void StopTimer()
    {
        m_isTiming = false;
        m_phaseStartTime = 0.0f;
        m_phaseTime = 0.0f;
    }

    /// <summary>
    /// ������Ʈ ������ �̵�
    /// </summary>
    void MoveObj()
    {
        foreach (SimpleAtk item in m_activeSimpleAtkObjList)
        {
            if (m_activeSimpleAtkObjList == null)
            {
                break;
            }
            if (item.m_isMove)
            {
                item.transform.Translate(Vector3.forward * item.m_speed * Time.fixedDeltaTime);

                if (item.transform.position.x <= -m_wallHalfSize * 2 ||
                    item.transform.position.x >= m_wallHalfSize * 2 ||
                    item.transform.position.y <= -m_wallHalfSize ||
                    item.transform.position.y >= m_wallHalfSize * 3 ||
                    item.transform.position.z <= -m_wallHalfSize * 2 ||
                    item.transform.position.z >= m_wallHalfSize * 2)
                {
                    item.transform.position = m_objBasicPos;
                    item.ResetObj();

                    m_toWaitSimpleAtkTmpList.Add(item);
                }
            }

        }
        foreach (Scaffold item in m_activeScaffoldObjList)
        {
            if(m_activeScaffoldObjList == null)
            {
                break;
            }
            if (item.m_isMove)
            {
                item.transform.Translate(Vector3.forward * item.m_speed * Time.fixedDeltaTime);

                if (item.transform.position.x <= -m_wallHalfSize * 2 ||
                    item.transform.position.x >= m_wallHalfSize * 2 ||
                    item.transform.position.y <= -m_wallHalfSize ||
                    item.transform.position.y >= m_wallHalfSize * 3 ||
                    item.transform.position.z <= -m_wallHalfSize * 2 ||
                    item.transform.position.z >= m_wallHalfSize * 2)
                {
                    item.transform.position = m_objBasicPos;
                    item.ResetObj();

                    m_toWaitScaffoldTmpList.Add(item);
                }
            }
        }

        foreach (SimpleAtk item in m_toWaitSimpleAtkTmpList)
        {
            WaitSimpleAtk(item);
        }
        foreach (Scaffold item in m_toWaitScaffoldTmpList)
        {
            WaitScaffold(item);
        }

        m_toWaitSimpleAtkTmpList.Clear();
        m_toWaitScaffoldTmpList.Clear();
    }

    void GenObjAsAtkData(AtkData argAtkData)
    {
        GenObj(
        argAtkData.m_type,
        argAtkData.m_position,
        argAtkData.m_rotation,
        argAtkData.m_size,
        argAtkData.m_speed,
        argAtkData.m_isMove
        );
    }
    /// <summary>
    /// ��ü ����
    /// </summary>
    /// <param name="argAtkType">Ÿ��</param>
    /// <param name="argPosition">��ġ</param>
    /// <param name="argRotation">����</param>
    /// <param name="argSize">ũ��</param>
    /// <param name="argSpeed">�ӵ�</param>
    /// <param name="argIsMove">�����̴��� �ȿ����̴���</param>
    void GenObj(AtkType argAtkType, Vector3 argPosition, Vector3 argRotation, Vector3 argSize, float argSpeed, bool argIsMove)
    {
        switch (argAtkType)
        {
            case AtkType.BlueSimpleAtk:
                SimpleAtk(argPosition, argRotation, new Vector3(argSize.x, argSize.y, argSize.z), argSpeed, true);
                return;
            case AtkType.SimpleAtk:
                SimpleAtk(argPosition, argRotation, new Vector3(argSize.x, argSize.y, argSize.z), argSpeed, false);
                return;
            case AtkType.PopAtk:
                AllWallPopAtk();
                return;
            case AtkType.RangeAtk:
                RangeAtk(argPosition, argRotation, new Vector3(argSize.x, argSize.y, argSize.z));
                return;
            case AtkType.GravityAtk:
                GravityAtk(argRotation.z);
                return;
            case AtkType.Scaffold:
                Scaffold(argPosition, argRotation, new Vector3(argSize.x, argSize.y, argSize.z), argSpeed, argIsMove);
                return;
            default:
                Debug.Log("not allowed value");
                return;
        }
    }
    /// <summary>
    /// �ܼ� ���� ����
    /// </summary>
    /// <param name="argPosition">���� ��ġ</param>
    /// <param name="argRotation">���� ����</param>
    /// <param name="argSize">���� ũ��</param>
    /// <param name="argSpeed">���� �� �ӵ�</param>
    void SimpleAtk(Vector3 argPosition, Vector3 argRotation, Vector3 argSize, float argSpeed, bool argIsMovePlayerAtk)
    {
        SimpleAtk _atk = ActiveSimpleAtk();
        _atk.m_speed = argSpeed;
        _atk.m_isMove = true;
        _atk.ChangeMovePlayerAtk(argIsMovePlayerAtk);

        _atk.transform.position = argPosition;
        _atk.transform.rotation = Quaternion.Euler(argRotation);
        _atk.transform.localScale = argSize;
    }
    /// <summary>
    /// ���Ÿ� ����
    /// </summary>
    /// <param name="argPosition">���� ��ġ</param>
    /// <param name="argRotation">���� ����</param>
    /// <param name="argSize">���� ũ��(���� �ϴ� ������Ʈ�� ũ�⿡ ���)</param>
    /// <param name="argWarnTime">���� ��� �ð�</param>
    /// <param name="argAtkTime">���� �ð�</param>
    void RangeAtk(Vector3 argPosition, Vector3 argRotation, Vector3 argSize)
    {
        RangeAtk _atk = ActiveRangeAtk();
        _atk.StartRangeAtk(m_rangeAtkWarnTime, m_rangeAtkTime);

        _atk.transform.position = argPosition;
        _atk.transform.rotation = Quaternion.Euler(argRotation);
        _atk.transform.localScale = argSize;

        GameManager.Instance.GetSoundManager.PlayEffectSound(m_roundData.m_soundData.m_rangeAtk);
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="argPosition">���� ��ġ</param>
    /// <param name="argRotation">���� ����</param>
    /// <param name="argSize">���� ũ��</param>
    /// <param name="argSpeed">���� �� �ӵ�</param>
    void Scaffold(Vector3 argPosition, Vector3 argRotation, Vector3 argSize, float argSpeed, bool argIsMove)
    {
        Scaffold _atk = ActiveScaffold();
        _atk.m_speed = argSpeed;
        _atk.m_isMove = argIsMove;

        _atk.transform.position = argPosition;
        _atk.transform.rotation = Quaternion.Euler(argRotation);
        _atk.transform.localScale = argSize;
    }

    /// <summary>
    /// ���� ���� ��ä ���� ����
    /// </summary>
    /// <returns></returns>
    void AllWallPopAtk()
    {
        StartCoroutine(IEPopAtk());
    }
    /// <summary>
    /// ��ü �ܼ� ���� ���� �غ�
    /// </summary>
    void AllWallPopAtkReady()
    {
        for(int i = -m_wallHalfSize; i < m_wallHalfSize; i += m_simpleAtkObjBasicSize)
        {
            for(int o = -m_wallHalfSize; o < m_wallHalfSize; o += m_simpleAtkObjBasicSize)
            {
                SimpleAtk _simAtk = m_simpleAtkObjWaitQueue.Dequeue();
                _simAtk.m_isMove = false;

                _simAtk.transform.position = new Vector3(o, -10.0f, i);
                _simAtk.transform.localScale = new Vector3(3.0f, 10.0f, 3.0f);
                m_activeSimpleAtkObjList.AddLast(_simAtk);
            }
        }
    }
    /// <summary>
    /// ��ü �ܼ� ���� ���� ����
    /// </summary>
    void AllWallPopAtkStart()
    {
        foreach(SimpleAtk item in m_activeSimpleAtkObjList)
        {
            if (!item.m_isMove)
            {
                item.PopAtk(m_popAtkActiveTime, m_popAtkSpeed, m_popAtkMaxHeight);
            }
        }

        GameManager.Instance.GetSoundManager.PlayEffectSound(m_roundData.m_soundData.m_popAtk);
    }
    /// <summary>
    /// ��ü �ܼ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator IEPopAtk()
    {
        AllWallPopAtkReady();
        m_popWarningObj.SetActive(true);
        yield return new WaitForSeconds(m_popAtkWarnTime);

        AllWallPopAtkStart();
        m_popWarningObj.SetActive(false);
    }

    /// <summary>
    /// �߷� ����
    /// </summary>
    /// <param name="argDirZ">���� ����</param>
    void GravityAtk(float argDirZ)
    {
        GameManager.Instance.GetSoundManager.PlayEffectSound(m_roundData.m_soundData.m_gravityAtk);

        StartCoroutine(IEChangeWall(argDirZ));
        StartCoroutine(IEGravityAtk());
    }
    /// <summary>
    /// 90���� ���������� ������ �� �ε����� ��ȯ
    /// </summary>
    /// <param name="argDirZ">����</param>
    /// <returns>�� �ε���</returns>
    int AngleToWallIndex(float argDirZ)
    {
        switch (argDirZ)
        {
            case 0.0f:
                return 2;
            case 90.0f:
                return 3;
            case 180.0f:
                return 0;
            case -90.0f:
                return 1;
            default:
                Debug.Log("Not allowed value");
                return 2;
        }
    }
    /// <summary>
    /// �� ���� �ڷ�ƾ
    /// </summary>
    /// <param name="argDirZ">euler ����</param>
    /// <returns>none</returns>
    IEnumerator IEChangeWall(float argDirZ)
    {
        for (int i = 0; i < m_wallList.Count; i++)
        {
            m_wallList[i].gameObject.GetComponent<MeshRenderer>().material = m_transparentWallMat;
        }
        m_wallList[AngleToWallIndex(argDirZ)].gameObject.GetComponent<MeshRenderer>().material = m_wallMat;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, argDirZ));
        while (Quaternion.Angle(m_wallCenterPos.rotation, targetRotation) > 0.1f)
        {
            m_wallCenterPos.rotation = Quaternion.RotateTowards(
                m_wallCenterPos.rotation,
                targetRotation,
                m_wallDirChangeSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
    /// <summary>
    /// �߷� ����
    /// </summary>
    /// <param name="argDir">�߷� ���� ����</param>
    /// <returns>none</returns>
    IEnumerator IEGravityAtk()
    {
        m_ascensionCompleteFlag = true;

        while (true)
        {
            // ��� ������ ��
            if (m_ascensionCompleteFlag && m_playerController.transform.position.y <
                m_wallCenterPos.position.y - 1.0f)
            {
                m_playerController.SetCanMoveFlage = false;
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    new Vector3(m_playerController.transform.position.x,
                    m_wallCenterPos.position.y,
                    m_playerController.transform.position.z),
                    m_gravityAtkSpeed * Time.deltaTime);
                m_playerController.SetGravity = false;
            }
            else
            {
                m_ascensionCompleteFlag = false;

                // �ϰ� ������ ��
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    new Vector3(m_playerController.transform.position.x,
                    0f,
                    m_playerController.transform.position.z),
                    m_gravityAtkSpeed * 4 * Time.deltaTime);
                if (m_playerController.transform.position.y <= 0.0f)
                {
                    m_playerController.SetCanMoveFlage = true;
                    m_playerController.SetGravity = true;

                    GameManager.Instance.GetSoundManager.PlayEffectSound(m_roundData.m_soundData.m_hitGround);

                    yield break;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// �ܼ� ���� Ȱ��ȭ ť�� �̵�
    /// </summary>
    /// <param name="argAtk">���</param>
    /// <returns>���</returns>
    SimpleAtk ActiveSimpleAtk()
    {
        m_activeSimpleAtkObjList.AddLast(m_simpleAtkObjWaitQueue.Dequeue());
        return m_activeSimpleAtkObjList.Last.Value;
    }
    /// <summary>
    /// ���Ÿ� ���� Ȱ��ȭ ť�� �̵�
    /// </summary>
    /// <param name="argAtk">���</param>
    /// <returns>���</returns>
    RangeAtk ActiveRangeAtk()
    {
        m_activeRangeAtkObjList.AddLast(m_rangeAtkObjWaitQueue.Dequeue());
        return m_activeRangeAtkObjList.Last.Value;
    }
    /// <summary>
    /// ���� Ȱ��ȭ ť�� �̵�
    /// </summary>
    /// <param name="argAtk">���</param>
    /// <returns>���</returns>
    Scaffold ActiveScaffold()
    {
        m_activeScaffoldObjList.AddLast(m_scaffoldObjWaitQueue.Dequeue());
        return m_activeScaffoldObjList.Last.Value;
    }

    /// <summary>
    /// �ܼ� ���� ��� ť�� ����
    /// </summary>
    /// <param name="argAtk"></param>
    public SimpleAtk WaitSimpleAtk(SimpleAtk argAtk)
    {
        m_activeSimpleAtkObjList.Remove(argAtk);
        m_simpleAtkObjWaitQueue.Enqueue(argAtk);
        argAtk.transform.position = m_objBasicPos;
        return argAtk;
    }
    /// <summary>
    /// �ܼ� ���� ��� ť�� ����
    /// </summary>
    /// <param name="argAtk"></param>
    public RangeAtk WaitRangeAtk(RangeAtk argAtk)
    {
        m_activeRangeAtkObjList.Remove(argAtk);
        m_rangeAtkObjWaitQueue.Enqueue(argAtk);
        argAtk.transform.position = m_objBasicPos;
        return argAtk;
    }
    /// <summary>
    /// ���� ��� ť�� ����
    /// </summary>
    /// <param name="argAtk"></param>
    public Scaffold WaitScaffold(Scaffold argScaff)
    {
        m_activeScaffoldObjList.Remove(argScaff);
        m_scaffoldObjWaitQueue.Enqueue(argScaff);
        argScaff.transform.position = m_objBasicPos;
        return argScaff;
    }

    /// <summary>
    /// ��ü �ܼ� ���� ������Ʈ ����
    /// </summary>
    public void AllSimpleAtkObjReset()
    {
        foreach (SimpleAtk item in m_activeSimpleAtkObjList)
        {
            m_simpleAtkObjWaitQueue.Enqueue(item);
        }
        foreach (SimpleAtk item in m_simpleAtkObjWaitQueue)
        {
            item.gameObject.transform.position = m_objBasicPos;
        }
    }
    /// <summary>
    /// ��ü ���Ÿ� ���� ������Ʈ ����
    /// </summary>
    public void AllRangeAtkObjReset()
    {
        foreach (RangeAtk item in m_activeRangeAtkObjList)
        {
            m_rangeAtkObjWaitQueue.Enqueue(item);
        }
        foreach (RangeAtk item in m_rangeAtkObjWaitQueue)
        {
            item.gameObject.transform.position = m_objBasicPos;
        }
    }
    /// <summary>
    /// ��ü ���Ÿ� ���� ������Ʈ ����
    /// </summary>
    public void AllScaffoldObjReset()
    {
        foreach (Scaffold item in m_activeScaffoldObjList)
        {
            m_scaffoldObjWaitQueue.Enqueue(item);
        }
        foreach (Scaffold item in m_scaffoldObjWaitQueue)
        {
            item.gameObject.transform.position = m_objBasicPos;
        }
    }
    /// <summary>
    /// ��� ������Ʈ �ʱ�ȭ
    /// </summary>
    public void ResetAllObj()
    {
        AllRangeAtkObjReset();
        AllScaffoldObjReset();
        AllSimpleAtkObjReset();

        m_wallCenterPos.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    
    public static RoundManager Instance
    {
        get { return g_gameManager; }
    }
    public UIObjManager GetUIObjManager
    {
        get { return m_uIObjManager; }
    }
    public PlayerController GetPlayerController
    {
        get { return m_playerController; }
    }
    public Material GetAtkObjMat
    {
        get { return m_atkObjMat; }
    }
    public Material GetMovePlayerAtkObjMat
    {
        get { return m_movePlayerAtkObjMat; }
    }
    public Material GetWarnMat
    {
        get { return m_warningMat; }
    }
    public int GetPhase
    {
        get { return m_phase; }
    }
    public bool GetIsTiming
    {
        get { return m_isTiming; }
    }
    public bool GetIsGameOver
    {
        get { return m_isGameOver; }
    }

    public RoundData SetRoundData
    {
        get { return m_roundData; }
        set { m_roundData = value; }
    }
}
