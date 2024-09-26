using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
/// </summary>
public enum AtkType
{
    BlueSimpleAtk,
    SimpleAtk,
    PopAtk,

    RangeAtk,

    GravityAtk,

    Scaffold,
}

/// <summary>
/// ���� ������
/// </summary>
public class AtkData
{
    //���� ����
    public int m_order = -1;
    //���� Ÿ��
    public AtkType m_type = new AtkType();
    //�����̴��� �ȿ����̴���
    //�����Ϳ����� 0�� 1�� ǥ��
    //0 = false
    //1 = true
    public bool m_isMove = false;
    //���� �ð�
    public float m_genTime = 0.0f;
    //���� ������Ʈ ������
    public float m_sizeY = 0.0f;
    //���� ��ġ, ���� ����
    //�����̴� ������Ʈ�� �������� �̵���
    //���� ���⵵ �����̼ǿ� ���� �޶���
    public Vector3 m_position = new Vector3();
    public Vector3 m_rotation = new Vector3();
}

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// game manager
    /// </summary>
    static GameManager g_gameManager;

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
    [SerializeField]
    int m_rangeAtkObjBasicSize = 3;
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
    /// �� ���� ������ ����Ʈ
    /// </summary>
    private List<AtkData> m_atkDataList = new List<AtkData>();
    /// <summary>
    /// ��� ����Ʈ�� ���ư��� ���� �ӽ� ���� ����Ʈ
    /// </summary>
    private List<SimpleAtk> m_toWaitTmpList = new List<SimpleAtk>();
    /// <summary>
    /// �÷��̾� ������ �ܼ� �����ϴ� ������Ʈ ��� ť
    /// </summary>
    private Queue<SimpleAtk> m_simpleAtkObjWaitQueue = new Queue<SimpleAtk>();
    /// <summary>
    /// ���Ÿ� ���� ������Ʈ ��� ť
    /// </summary>
    private Queue<RangeAtk> m_rangeAtkObjWaitQueue = new Queue<RangeAtk>();
    /// <summary>
    /// ���忡�� Ȱ��ȭ �� �ܼ� ���� ������Ʈ
    /// ����ϰ� ���� �������� ��� ť�� �̵�
    /// </summary>
    private LinkedList<SimpleAtk> m_activeSimpleAtkObjList = new LinkedList<SimpleAtk>();
    /// <summary>
    /// ���忡�� Ȱ��ȭ �� ���Ÿ� ���� ������Ʈ
    /// ����ϰ� ���� �������� ��� ť�� �̵�
    /// </summary>
    private LinkedList<RangeAtk> m_activeRangeAtkObjQueue = new LinkedList<RangeAtk>();

    /// <summary>
    /// �����ϴ� ������Ʈ�� ������ ��ġ
    /// </summary>
    private Vector3 m_atkObjBasicPos = new Vector3(300.0f, 0.0f, 0.0f);
    /// <summary>
    /// ���� �÷��̾ ����ϴ� ��
    /// 12�� ���� �ð�������� 0, 1, 2, 3
    /// </summary>
    private int m_nowWall = 0;
    /// <summary>
    /// ���� ������
    /// </summary>
    private int m_phase = 0;
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
    /// �� ȸ�� �Ϸ� �÷���
    /// </summary>
    private bool m_wallRotCompleteFlag = false;
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

        AllWallPopAtk();

        SimpleAtk(new Vector3(20.0f, 0.0f, -10.0f), new Vector3(0.0f, -90.0f, 0.0f), new Vector3(5.0f, 20.0f, 5.0f), 8.0f, false);
        SimpleAtk(new Vector3(20.0f, 0.0f, 0.0f), new Vector3(0.0f, -90.0f, 0.0f), new Vector3(5.0f, 20.0f, 5.0f), 5.0f, true);
        SimpleAtk(new Vector3(20.0f, 0.0f, 10.0f), new Vector3(0.0f, -90.0f, 0.0f), new Vector3(5.0f, 20.0f, 5.0f), 3.0f, false);

        RangeAtk(new Vector3(-15.0f, 2.0f, -30.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 5.0f, 5.0f));
        RangeAtk(new Vector3(-5.0f, 2.0f, -30.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 5.0f, 5.0f));
    }
    void Update()
    {
        SimpleAtkMove();
        TimingCheck();

        if (Input.GetMouseButtonDown(0) && !m_isTiming)
        {
            StartTimer();
        }
        else if (Input.GetMouseButtonDown(0) && m_isTiming)
        {
            Debug.Log("��� �ð�: " + m_phaseTime + "��");
            StopTimer();
        }
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

        GetCSVData();
    }
    /// <summary>
    /// start �ʱ� ����
    /// </summary>
    void StartSetting()
    {
        GameObject _simpleAtkParent = new GameObject("SimpleAtkParent");
        GameObject _rangeAtkParent = new GameObject("RangeAtkParent");
        GameObject _obj = null;

        for (int i = 0; i < m_simpleAtkObjCount; i++)
        {
            _obj = Instantiate(m_simpleAtkObj, _simpleAtkParent.transform);
            _obj.transform.position = m_atkObjBasicPos;
            m_simpleAtkObjWaitQueue.Enqueue(_obj.GetComponent<SimpleAtk>());

        }
        for (int i = 0; i < m_rangeAtkObjCount; i++)
        {
            _obj = Instantiate(m_rangeAtkObj, _rangeAtkParent.transform);
            _obj.transform.position = m_atkObjBasicPos;
            m_rangeAtkObjWaitQueue.Enqueue(_obj.GetComponent<RangeAtk>());
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
        List<Dictionary<string, object>> _data = CSVReader.Read("Phase" + m_phase);

        for (int i = 0; i < _data.Count; i++)
        {
            AtkData _atkData = new AtkData();

            _atkData.m_order = int.Parse(_data[i]["order"].ToString());
            _atkData.m_type = StrToAtkType(_data[i]["type"].ToString());
            _atkData.m_isMove = StrToBool(_data[i]["isMove"].ToString());
            _atkData.m_genTime = float.Parse(_data[i]["genTime"].ToString());
            _atkData.m_sizeY = float.Parse(_data[i]["sizeY"].ToString());

            _atkData.m_position.x = float.Parse(_data[i]["positionX"].ToString());
            _atkData.m_position.y = float.Parse(_data[i]["positionY"].ToString());
            _atkData.m_position.z = float.Parse(_data[i]["positionZ"].ToString());

            _atkData.m_rotation.x = float.Parse(_data[i]["rotationX"].ToString());
            _atkData.m_rotation.y = float.Parse(_data[i]["rotationY"].ToString());
            _atkData.m_rotation.z = float.Parse(_data[i]["rotationZ"].ToString());

            m_atkDataList.Add(_atkData);
        }
    }
    /// <summary>
    /// ���� Ÿ�� ���ڿ� �̳����� ����
    /// </summary>
    /// <param name="argStr">���� Ÿ�� ���ڿ�</param>
    /// <returns>���� Ÿ�� �̳�</returns>
    AtkType StrToAtkType(string argStr)
    {
        switch (argStr)
        {
            case "BlueSimpleAtk":
                return AtkType.BlueSimpleAtk;
            case "SimpleAtk":
                return AtkType.SimpleAtk;
            case "PopAtk":
                return AtkType.PopAtk;
            case "RangeAtk":
                return AtkType.RangeAtk;
            case "GravityAtk":
                return AtkType.GravityAtk;
            case "Scaffold":
                return AtkType.Scaffold;
            default:
                Debug.Log(argStr + " not allowed value");
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


    void TimingCheck()
    {
        if (m_isTiming)
        {
            m_phaseStartTime += Time.deltaTime;
            m_phaseTime = Mathf.Floor(m_phaseStartTime) + Mathf.Round((m_phaseStartTime % 1.0f) * 10.0f) / 10.0f;
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
    /// �ܼ� ���� ������Ʈ �̵�
    /// </summary>
    void SimpleAtkMove()
    {
        foreach(SimpleAtk item in m_activeSimpleAtkObjList)
        {
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
                    item.transform.position = m_atkObjBasicPos;
                    item.ResetObj();

                    m_toWaitTmpList.Add(item);
                }
            }

        }

        foreach(SimpleAtk item in m_toWaitTmpList)
        {
            WaitSimpleAtk(item);
        }

        m_toWaitTmpList.Clear();
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
        m_wallRotCompleteFlag = false;

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
        m_playerController.GetComponent<Rigidbody>().useGravity = false;

        while (true)
        {
            // ��� ������ ��
            if (m_ascensionCompleteFlag && m_playerController.transform.position.y <
                m_wallCenterPos.position.y)
            {
                m_playerController.IsCanMoveFlage = false;
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    m_wallCenterPos.position,
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
                    m_playerController.IsCanMoveFlage = true;
                    m_playerController.GetComponent<Rigidbody>().useGravity = true;
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
        m_activeRangeAtkObjQueue.AddLast(m_rangeAtkObjWaitQueue.Dequeue());
        return m_activeRangeAtkObjQueue.Last.Value;
    }

    /// <summary>
    /// �ܼ� ���� ��� ť�� ����
    /// </summary>
    /// <param name="argAtk"></param>
    public SimpleAtk WaitSimpleAtk(SimpleAtk argAtk)
    {
        m_activeSimpleAtkObjList.Remove(argAtk);
        m_simpleAtkObjWaitQueue.Enqueue(argAtk);
        argAtk.transform.position = m_atkObjBasicPos;
        return argAtk;
    }
    /// <summary>
    /// �ܼ� ���� ��� ť�� ����
    /// </summary>
    /// <param name="argAtk"></param>
    public RangeAtk WaitRangeAtk(RangeAtk argAtk)
    {
        m_activeRangeAtkObjQueue.Remove(argAtk);
        m_rangeAtkObjWaitQueue.Enqueue(argAtk);
        argAtk.transform.position = m_atkObjBasicPos;
        return argAtk;
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
            item.gameObject.transform.position = m_atkObjBasicPos;
        }
    }
    /// <summary>
    /// ��ü ���Ÿ� ���� ������Ʈ ����
    /// </summary>
    public void AllRangeAtkObjReset()
    {
        foreach (RangeAtk item in m_activeRangeAtkObjQueue)
        {
            m_rangeAtkObjWaitQueue.Enqueue(item);
        }
        foreach (RangeAtk item in m_rangeAtkObjWaitQueue)
        {
            item.gameObject.transform.position = m_atkObjBasicPos;
        }
    }


    /// <summary>
    /// �ڱ� �ڽ� �ν��Ͻ�
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            return g_gameManager;
        }
    }
    public Material IsAtkObjMat
    {
        get { return m_atkObjMat; }
    }
    public Material IsMovePlayerAtkObjMat
    {
        get { return m_movePlayerAtkObjMat; }
    }
    public Material IsWarnMat
    {
        get { return m_warningMat; }
    }
}
