using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �� ������ ����, ���� ���� ������ �̵� ���� ����մϴ�
/// �ܵ����� ������ �����ؾ��մϴ�
/// ���� ������ ����Ʈ�� �ε����� ��ũ���ͺ� ������Ʈ ���� ���� ������ �ε����� ��ġ�ؾ��մϴ�
/// </summary>
public class GameDataManager : MonoBehaviour
{
    /// <summary>
    /// �ڱ� �ڽ�
    /// </summary>
    static GameDataManager g_gameDataManager = null;

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    [SerializeField]
    List<RoundData> m_roundDataList = new List<RoundData>();

    /// <summary>
    /// ���� ���� �ε���
    /// </summary>
    private int m_roundIndex = 0;
    /// <summary>
    /// ��
    /// </summary>
    private long m_money = 0;

    private void Awake()
    {
        if (g_gameDataManager == null)
        {
            g_gameDataManager = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���� ������ ��������
    /// </summary>
    /// <param name="argIndex">���� �ε���</param>
    /// <returns>���� ������</returns>
    public RoundData GetRoundData(int argIndex)
    {
        return m_roundDataList[argIndex];
    }

    public static GameDataManager Instance
    {
        get { return g_gameDataManager; }
    }
    public List<RoundData> GetRoundDataList
    {
        get { return m_roundDataList; }
    }
    public int GetRoundIndex
    {
        get { return m_roundIndex; }
        set 
        {
            m_roundIndex = value;
            if(m_roundIndex <= 0)
            {
                m_roundIndex = 0;
            }
        }
    }
    public long GetMoney
    {
        get { return m_money; }
        set
        {
            m_money = value;
            if(m_money <= 0)
            {
                m_money = 0;
            }
        }
    }

}
