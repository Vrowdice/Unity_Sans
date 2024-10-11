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


/// <summary>
/// �� ���� Ÿ�� �̳�
/// ������ �󿡼��� �� �ε����� �ο�����
/// </summary>
public enum AtkType
{
    SimpleAtk, //1 index
    BlueSimpleAtk, //2 index
    PopAtk, //3 index
    RangeAtk, //4 index
    GravityAtk, //5 index
    Scaffold, //6 index
}

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// game manager
    /// </summary>
    static GameManager g_gameManager;

    [Header("Common")]
    /// <summary>
    /// ���� ������
    /// </summary>
    [SerializeField]
    int m_phase = 0;
    /// <summary>
    /// ����� ������ ��ٸ��� �ð�
    /// </summary>
    [SerializeField]
    float m_phaseOverWaitTime = 4.0f;

    [Header("Wall")]
    /// <summary>
    /// (����)�� �߽�
    /// </summary>
    [SerializeField]
    Transform m_wallCenterPos = null;
    /// <summary>
    /// (����)�� ����Ʈ
    /// </summary>
    [SerializeField]
    List<GameObject> m_wallList = new List<GameObject>();
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

    private void Awake()
    {
        AwakeSetting();
    }
    void Start()
    {
        StartSetting();
        PhaseStart();
    }
    void Update()
    {
        MoveObj();
        TimingCheck();
    }

    /// <summary>
    /// awake �ʱ� ����
    /// </summary>
    void AwakeSetting()
    {
        if (g_gameManager == null)
        {
            g_gameManager = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);

        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        m_uIObjManager = GameObject.Find("UIObj").GetComponent<UIObjManager>();

        GetCSVData();
    }
    /// <summary>
    /// start �ʱ� ����
    /// </summary>
    void StartSetting()
    {
        GameObject _simpleAtkParent = new GameObject("SimpleAtkParent");
        GameObject _rangeAtkParent = new GameObject("RangeAtkParent");
        GameObject _scaffoldParent = new GameObject("ScaffoldParent");

        GameObject _obj = null;
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

        StartCoroutine(IEChangeWall(0.0f));

        _obj = Instantiate(m_popWarningObj);
        m_popWarningObj = _obj;
        m_popWarningObj.SetActive(false);
    }
    
    /// <summary>
    /// CSV ������ �����ͼ� ����Ʈ�� ����
    /// </summary>
    void GetCSVData()
    {
        int _tmpRepeatAtkIndex = -1;
        int _tmpRepeatAtkOrder = 0;
        m_atkDataList = new List<AtkData>();
        List<Dictionary<string, object>> _data = CSVReader.Read("Phase" + m_phase);

        if(_data == null)
        {
            GameOver(true);
            return;
        }

        for (int i = 0; i < _data.Count; i++)
        {
            if (_data[i]["order"].ToString() == "")
            {
                Debug.Log("om");
                continue;
            }

            AtkData _atkData = new AtkData();

            _atkData.m_order = int.Parse(_data[i]["order"].ToString());
            _atkData.m_type = StrToAtkType(int.Parse(_data[i]["type"].ToString()));
            _atkData.m_isMove = StrToBool(_data[i]["isMove"].ToString());
            _atkData.m_genTime = float.Parse(_data[i]["genTime"].ToString());
            _atkData.m_speed = float.Parse(_data[i]["speed"].ToString());

            _atkData.m_size.x = float.Parse(_data[i]["sizeX"].ToString());
            _atkData.m_size.y = float.Parse(_data[i]["sizeY"].ToString());
            _atkData.m_size.z = float.Parse(_data[i]["sizeZ"].ToString());

            _atkData.m_position.x = float.Parse(_data[i]["positionX"].ToString());
            _atkData.m_position.y = float.Parse(_data[i]["positionY"].ToString());
            _atkData.m_position.z = float.Parse(_data[i]["positionZ"].ToString());

            _atkData.m_rotation.x = float.Parse(_data[i]["rotationX"].ToString());
            _atkData.m_rotation.y = float.Parse(_data[i]["rotationY"].ToString());
            _atkData.m_rotation.z = float.Parse(_data[i]["rotationZ"].ToString());

            if (_atkData.m_order < 0)
            {
                if(_tmpRepeatAtkOrder != _atkData.m_order)
                {
                    _tmpRepeatAtkOrder = _atkData.m_order;

                    RepeatAtkData _repeatAtkData = new RepeatAtkData();
                    _repeatAtkData.m_atkData = _atkData;
                    _repeatAtkData.m_repeatStartTime = _atkData.m_genTime;
                    _repeatAtkData.m_repeatTime = 0.0f;
                    _repeatAtkData.m_repeatOverTime = 0.0f;

                    m_repeatAtkDataList.Add(_repeatAtkData);
                    _tmpRepeatAtkIndex++;
                }
                else
                {
                    if(m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatTime == 0.0f)
                    {
                        m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatTime = m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatStartTime - _atkData.m_genTime;
                    }
                    else if(m_repeatAtkDataList[_tmpRepeatAtkIndex].m_repeatOverTime == 0.0f)
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
    }
    /// <summary>
    /// ���� Ÿ�� ���ڿ� �̳����� ����
    /// </summary>
    /// <param name="argStr">���� Ÿ�� ���ڿ�</param>
    /// <returns>���� Ÿ�� �̳�</returns>
    AtkType StrToAtkType(int argIndex)
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
    bool StrToBool(string argStr)
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
            m_uIObjManager.ActiveUIObj(false);
            StartTimer();
        }
    }
    /// <summary>
    /// ���۵� ������ ������
    /// </summary>
    public void PhaseOver()
    {
        m_uIObjManager.ActiveUIObj(true);
        m_uIObjManager.SetRecoverCountTextObj(m_playerController.GetRecoverCount);

        m_phase++;
        m_phaseTime = 0.0f;
        m_phaseStartTime = 0.0f;

        m_atkIndex = 0;
        StopTimer();
        GetCSVData();
    }
    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public void GameOver(bool argWinOrDefeat)
    {
        m_isGameOver = true;

        ResetAllObj();

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

    void RepeatGen()
    {

    }
    /// <summary>
    /// Ÿ�̹� üũ ��ƾ
    /// </summary>
    void TimingCheck()
    {
        if(m_atkDataList == null)
        {
            return;
        }

        if (m_isTiming)
        {
            m_phaseStartTime += Time.deltaTime;
            m_phaseTime = Mathf.Floor(m_phaseStartTime) + Mathf.Round((m_phaseStartTime % 1.0f) * 10.0f) / 10.0f;

            //�ݺ� ���� �۵�
            if(m_repeatAtkDataList.Count > 0)
            {
                foreach (RepeatAtkData item in m_repeatAtkDataList)
                {
                    if (item.m_repeatOverTime >= m_phaseTime && item.m_repeatOverTime > 0.0f)
                    {
                        m_repeatAtkDataList.Remove(item);
                        continue;
                    }

                    if (item.m_repeatStartTime <= m_phaseTime
                        && item.m_repeatedTime != m_phaseTime
                        && item.m_repeatedTime == 0.0f)
                    {
                        GenObjAsAtkData(item.m_atkData);
                        continue;
                    }

                    if (m_phaseTime % item.m_repeatTime == 0
                        && item.m_repeatedTime != m_phaseTime)
                    {
                        item.m_repeatedTime = m_phaseTime;
                        GenObjAsAtkData(item.m_atkData);
                    }
                }
            }
            
            //�Ϲ� ���� ���� �۵�
            if(m_atkDataList.Count > 0)
            {
                if (m_atkDataList.Count <= m_atkIndex)
                {
                    if (m_phaseTime >= m_atkDataList[m_atkIndex - 1].m_genTime + m_phaseOverWaitTime)
                    {
                        PhaseOver();
                    }
                }
                else
                {
                    if (m_atkDataList[m_atkIndex].m_genTime <= m_phaseTime)
                    {
                        GenObjAsAtkData(m_atkDataList[m_atkIndex]);

                        m_atkIndex++;
                    }
                }
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
                item.transform.Translate(Vector3.forward * item.m_speed * Time.deltaTime);

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
                item.transform.Translate(Vector3.forward * item.m_speed * Time.deltaTime);

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
                GravityAtk(argRotation.y);
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
                m_playerController.GetCanMoveFlage = false;
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    new Vector3(m_playerController.transform.position.x,
                    m_wallCenterPos.position.y,
                    m_playerController.transform.position.z),
                    m_gravityAtkSpeed * Time.deltaTime);
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
                    m_playerController.GetCanMoveFlage = true;
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
    }
    
    public static GameManager Instance
    {
        get
        {
            return g_gameManager;
        }
    }
    public UIObjManager GetUIObjManager
    {
        get { return m_uIObjManager; }
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
    public bool GetIsTiming
    {
        get { return m_isTiming; }
    }
    public bool GetIsGameOver
    {
        get { return m_isGameOver; }
    }
}
