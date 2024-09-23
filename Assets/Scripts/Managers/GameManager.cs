using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ���� Ÿ�� �̳�
/// </summary>
public enum AtkType
{
    BlueHorizontalBone,
    BlueVerticalBone,

    HorizontalBone,
    VerticalBone,

    PopVerticalBone,

    GasterBlaster,

    Gravity,

    scaffold,
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
    /// ���� ������Ʈ ������
    /// </summary>
    [SerializeField]
    GameObject m_simpleAtkObj = null;
    [SerializeField]
    GameObject m_rangeAtkObj = null;
    /// <summary>
    /// �ִ� ���� ������Ʈ ��
    /// </summary>
    [SerializeField]
    int m_simpleAtkObjCount = 300;
    [SerializeField]
    int m_rangeAtkObjCount = 30;
    /// <summary>
    /// ���� ������Ʈ �⺻ ������
    /// </summary>
    [SerializeField]
    int m_simpleAtkObjBasicSize = 3;
    [SerializeField]
    int m_rangeAtkObjBasicSize = 3;
    /// <summary>
    /// Ƣ������� ���� �ִ� ����
    /// </summary>
    [SerializeField]
    float m_popAtkMaxHeight = 5.0f;
    /// <summary>
    /// Ƣ������� ���� ���� Ȱ��ȭ �ð�
    /// </summary>
    [SerializeField]
    float m_popAtkActiveTime = 1.0f;
    /// <summary>
    /// Ƣ������� ���� ���� �ӵ�
    /// </summary>
    [SerializeField]
    float m_popAtkSpeed = 20.0f;
    /// <summary>
    /// Ƣ������� ���� ����ϴ� �ð�
    /// </summary>
    [SerializeField]
    float m_popAtkWarnTime = 2.0f;
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
    private Queue<SimpleAtk> m_activeSimpleAtkObjQueue = new Queue<SimpleAtk>();
    /// <summary>
    /// ���忡�� Ȱ��ȭ �� ���Ÿ� ���� ������Ʈ
    /// ����ϰ� ���� �������� ��� ť�� �̵�
    /// </summary>
    private Queue<RangeAtk> m_activeRangeAtkObjQueue = new Queue<RangeAtk>();
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
    private int m_phase = 1;

    /// <summary>
    /// ���� ����� ���� �ð�
    /// </summary>
    private float m_phaseTime = 0.0f;
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
    }
    void Update()
    {
        
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
            case "BlueHorizontalBone":
                return AtkType.BlueHorizontalBone;
            case "BlueVerticalBone":
                return AtkType.BlueVerticalBone;
            case "HorizontalBone":
                return AtkType.HorizontalBone;
            case "VerticalBone":
                return AtkType.VerticalBone;
            case "PopVerticalBone":
                return AtkType.PopVerticalBone;
            case "GasterBlaster":
                return AtkType.GasterBlaster;
            case "Gravity":
                return AtkType.Gravity;
            case "scaffold":
                return AtkType.scaffold;
            default:
                Debug.Log("Not allowed value");
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
                _simAtk.transform.position = new Vector3(o, -10.0f, i);
                m_activeSimpleAtkObjQueue.Enqueue(_simAtk);
            }
        }
    }
    /// <summary>
    /// ��ü �ܼ� ���� ���� ����
    /// </summary>
    void AllWallPopAtkStart()
    {
        int _count = m_activeSimpleAtkObjQueue.Count;
        for (int i = 0; i < _count; i++)
        {
            SimpleAtk _simAtk = m_activeSimpleAtkObjQueue.Dequeue();
            _simAtk.PopAtk(m_popAtkActiveTime, m_popAtkSpeed, m_popAtkMaxHeight);
            m_simpleAtkObjWaitQueue.Enqueue(_simAtk);
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
        yield return new WaitForSeconds(m_popAtkActiveTime);

        AllSimpleAtkObjReset();
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
    /// ��ü �ܼ� ���� ������Ʈ ����
    /// </summary>
    public void AllSimpleAtkObjReset()
    {
        foreach (SimpleAtk item in m_activeSimpleAtkObjQueue)
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
}
