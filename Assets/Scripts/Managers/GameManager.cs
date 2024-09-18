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
    /// �߷� ���� ��(�ӵ�)
    /// </summary>
    [SerializeField]
    float m_gravityAtkPower = 0.0f;

    /// <summary>
    /// �� ��ȯ �ӵ�
    /// </summary>
    [SerializeField]
    float m_wallDirChangeSpeed = 0.0f;

    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    /// </summary>
    private PlayerController m_playerController = null;

    /// <summary>
    /// �� ���� ������ ����Ʈ
    /// </summary>
    private List<AtkData> m_atkDataList = new List<AtkData>();

    /// <summary>
    /// ���� �÷��̾ ����ϴ� ��
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
    }
    void Start()
    {
        GravityAtk(90.0f);
    }
    void Update()
    {
        
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
            _atkData.m_type = StrToItemType(_data[i]["type"].ToString());
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
    /// ������ Ÿ�� ���ڿ� �̳����� ����
    /// </summary>
    /// <param name="argStr">������ Ÿ�� ���ڿ�</param>
    /// <returns>������ Ÿ�� �̳�</returns>
    AtkType StrToItemType(string argStr)
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
    /// ���� ���� ��ä �� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator AllWallBoneAtk()
    {
        // �� ������ ���
        yield return null;
    }

    /// <summary>
    /// �߷� ����
    /// </summary>
    /// <param name="argDir">���� ����</param>
    void GravityAtk(float argDir)
    {
        StartCoroutine(ChangeWallIE(argDir));
        StartCoroutine(GravityAtkIE());
    }
    /// <summary>
    /// �� ���� �ڷ�ƾ
    /// </summary>
    /// <param name="argDirZ">euler ����</param>
    /// <returns>none</returns>
    IEnumerator ChangeWallIE(float argDirZ)
    {
        m_wallRotCompleteFlag = false;

        // ��ǥ ȸ������ Quaternion���� ��ȯ
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, argDirZ));

        // ��ǥ ������ ������ ������ �ݺ�
        while (Quaternion.Angle(m_wallCenterPos.rotation, targetRotation) > 0.1f)
        {
            // ������ �ӵ��� ȸ��
            m_wallCenterPos.rotation = Quaternion.RotateTowards(
                m_wallCenterPos.rotation,
                targetRotation,
                m_wallDirChangeSpeed * Time.deltaTime);

            // �� ������ ��� �� �ٽ� ȸ��
            yield return null;
        }

        // ��Ȯ�� ��ǥ ������ ����
        transform.rotation = targetRotation;
    }
    /// <summary>
    /// �߷� ����
    /// </summary>
    /// <param name="argDir">�߷� ���� ����</param>
    /// <returns>none</returns>
    IEnumerator GravityAtkIE()
    {
        m_ascensionCompleteFlag = true;
        m_playerController.GetComponent<Rigidbody>().useGravity = false;

        while (true)
        {
            // ��� ������ ��
            if (m_ascensionCompleteFlag && m_playerController.transform.position.y <
                m_wallCenterPos.position.y)
            {
                // ���� ���
                m_playerController.IsCanMoveFlage = false;
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    m_wallCenterPos.position,
                    m_gravityAtkPower * Time.deltaTime);
            }
            else
            {
                // ��� �Ϸ� �÷��׸� ����
                m_ascensionCompleteFlag = false;

                // �ϰ� ������ ��
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    new Vector3(m_playerController.transform.position.x,
                    0f,
                    m_playerController.transform.position.z),
                    m_gravityAtkPower * 4 * Time.deltaTime);

                // ���鿡 �����ϸ� ���� ����
                if (m_playerController.transform.position.y <= 0.0f)
                {
                    m_playerController.IsCanMoveFlage = true;
                    m_playerController.GetComponent<Rigidbody>().useGravity = true;
                    yield break;
                }
            }

            // �� ������ ���
            yield return null;
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
