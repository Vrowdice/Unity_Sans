using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �� ������ ����, ���� ���� ������ �̵� ���� ����մϴ�
/// �ܵ����� ������ �����ؾ��մϴ�
/// ���� ������ ����Ʈ�� �ε����� ��ũ���ͺ� ������Ʈ ���� ���� ������ �ε����� ��ġ�ؾ��մϴ�
/// 
/// �ΰ� ���
/// 
/// ��� �г� ���� ���
/// ���� ���� ���
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// �ڱ� �ڽ�
    /// </summary>
    static GameManager g_gameDataManager = null;

    /// <summary>
    /// ĳ���� ������ ����Ʈ
    /// </summary>
    [SerializeField]
    List<CharacterData> m_charactorDataList = new List<CharacterData>();
    /// <summary>
    /// ���� ������ ����Ʈ
    /// </summary>
    [SerializeField]
    List<RoundData> m_roundDataList = new List<RoundData>();

    /// <summary>
    /// �ɼ� �Ŵ��� ������
    /// </summary>
    [SerializeField]
    GameObject m_optionManagerPrefeb = null;
    /// <summary>
    /// �� Ȯ�� �г� ������
    /// </summary>
    [SerializeField]
    GameObject m_moneyPanelPrefeb = null;
    /// <summary>
    /// ��� ������Ʈ ������
    /// </summary>
    [SerializeField]
    GameObject m_alertObjPrefeb = null;

    /// <summary>
    /// ���� ��ųʸ�
    /// ���� ������ �˻� �� ���� ���� ���� Ž��
    /// </summary>
    private Dictionary<int, RoundInfo> m_roundDic = new Dictionary<int, RoundInfo>();
    /// <summary>
    /// ĳ���� ��ųʸ�
    /// ĳ���� ������ �˻� �� ���� ���� ���� Ž��
    /// </summary>
    private Dictionary<int, CharacterInfo> m_characterDic = new Dictionary<int, CharacterInfo>();
    /// <summary>
    /// ���� ��ųʸ� ���� ����Ʈ
    /// </summary>
    private List<int> m_roundDicSortList = new List<int>();
    /// <summary>
    /// ĳ���� ��ųʸ� ���� ����Ʈ
    /// </summary>
    private List<int> m_characterDicSortList = new List<int>();

    /// <summary>
    /// ���� �Ŵ���
    /// </summary>
    private SoundManager m_soundManager = null;
    /// <summary>
    /// �ɼ� �Ŵ���
    /// </summary>
    private OptionManager m_optionManager = null;
    /// <summary>
    /// �� ǥ�� �г� ����
    /// </summary>
    private MoneyPanel m_moneyPanel = null;
    /// <summary>
    /// ���� ���� �ε���
    /// </summary>
    private int m_roundIndex = 0;
    /// <summary>
    /// ���� ������� ĳ���� �ε���
    /// </summary>
    private int m_characterKey = 40001;
    /// <summary>
    /// ���� �� ĵ������ Ʈ������
    /// </summary>
    private Transform m_canvasTrans = null;

    /// <summary>
    /// ��
    /// 
    /// ���� �ʿ�
    /// </summary>
    private long m_money = 10000;

    private void Awake()
    {
        AwakeSetting();
    }

    private void OnEnable()
    {
        OnEnableSetting();
    }

    private void OnEnableSetting()
    {
        //���� ���� ��ųʸ� ����
        for(int i = 0; i < m_roundDataList.Count; i++)
        {
            try
            {
                m_roundDic.Add(int.Parse(m_roundDataList[i].name) , new RoundInfo(m_roundDataList[i], false, false));
                m_roundDicSortList.Add(int.Parse(m_roundDataList[i].name));
            }
            catch
            {
                Debug.Log("round data name is not a int " + m_roundDataList[i].name);
            }
        }
        //ĳ���� ���� �񼭴ϸ� ����
        for(int i = 0; i < m_charactorDataList.Count; i++)
        {
            try
            {
                m_characterDic.Add(int.Parse(m_charactorDataList[i].name), new CharacterInfo(m_charactorDataList[i], false));
                m_characterDicSortList.Add(int.Parse(m_charactorDataList[i].name));
            }
            catch
            {
                Debug.Log("character data name is not a int " + m_charactorDataList[i].name);
            }
        }

        m_roundDicSortList.Sort();
        m_characterDicSortList.Sort();

        m_characterDic[m_characterKey].m_isHave = true;
    }

    /// <summary>
    /// ���� �ε� �Ǿ��� ��
    /// gamedatamanager �ܵ� ��� ����
    /// </summary>
    /// <param name="scene">��</param>
    /// <param name="mode">���</param>
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_canvasTrans = GameObject.Find("Canvas").transform;
        if(m_canvasTrans != null)
        {
            //�ɼ� �Ŵ��� ���� �� �Ҵ�
            m_optionManager = Instantiate(m_optionManagerPrefeb, m_canvasTrans).GetComponent<OptionManager>();
            m_optionManager.OptionState(false);

            //�� ǥ�� �г� ���� �� �ʱ�ȭ
            m_moneyPanel = Instantiate(m_moneyPanelPrefeb, m_canvasTrans).GetComponent<MoneyPanel>();
            Money = m_money;
        }
    }
    /// <summary>
    /// �����ũ ����
    /// </summary>
    void AwakeSetting()
    {
        //�̱��� ����
        if (g_gameDataManager == null)
        {
            g_gameDataManager = this;
            SceneManager.sceneLoaded -= SceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //�� �ε��ϴ� ���
        SceneManager.sceneLoaded += SceneLoaded;
        //���� �Ŵ��� �Ҵ�
        m_soundManager = GetComponent<SoundManager>();
        //Ŀ�� ���� ����
        ChangeCursorState(true);
    }

    /// <summary>
    /// ���� ���� ��������
    /// </summary>
    /// <param name="argKey">���� Ű</param>
    /// <returns>���� ����</returns>
    public RoundInfo GetRoundInfo(int argKey)
    {
        try
        {
            return m_roundDic[argKey];
        }
        catch
        {
            Debug.Log("no have key " + argKey);
            return null;
        }
    }
    /// <summary>
    /// ĳ���� ���� ��������
    /// </summary>
    /// <param name="argKey">ĳ���� Ű</param>
    /// <returns>ĳ���� ����</returns>
    public CharacterInfo GetCharacterInfo(int argKey)
    {
        try
        {
            return m_characterDic[argKey];
        }
        catch
        {
            Debug.Log("no have key " + argKey);
            return null;
        }
    }
    /// <summary>
    /// ĳ���� ������ ��������
    /// </summary>
    /// <param name="argKey">ĳ���� �ε���</param>
    /// <returns>ĳ���� ������</returns>
    public CharacterData GetCharactorData(int argKey)
    {
        return GetCharacterInfo(argKey).m_data;
    }
    /// <summary>
    /// ���� ������ ��������
    /// </summary>
    /// <param name="argKey">���� �ε���</param>
    /// <returns>���� ������</returns>
    public RoundData GetRoundData(int argKey)
    {
        return GetRoundInfo(argKey).m_data;
    }

    /// <summary>
    /// �̸����� �� �̵�
    /// </summary>
    /// <param name="argStr">�̵��� ���� �̸�</param>
    public void MoveSceneAsName(string argStr, bool argCursorState)
    {
        m_soundManager.ResetAudioClip();
        SceneManager.LoadScene(argStr);
        ChangeCursorState(argCursorState);
    }
    /// <summary>
    /// ���� ���� ������ �̵�
    /// </summary>
    public void GoMainScene(int argRoundIndex)
    {
        m_roundIndex = argRoundIndex;
        MoveSceneAsName("Main", false);
    }
    /// <summary>
    /// Ÿ��Ʋ ������ �̵�
    /// </summary>
    public void GoTitleScene()
    {
        MoveSceneAsName("Title", true);
    }
    /// <summary>
    /// ���� ������ �̵�
    /// </summary>
    public void GoShopScene()
    {
        MoveSceneAsName("Shop", true);
    }

    /// <summary>
    /// ��� �г� ����
    /// </summary>
    public void Alert(string argAlertStr)
    {
        if(m_canvasTrans != null)
        {
            Instantiate(m_alertObjPrefeb, m_canvasTrans).GetComponent<AlertPanel>().Alert(argAlertStr);
        }
    }

    /// <summary>
    /// Ŀ�� ���� Ȱ��ȭ ��Ȱ��ȭ
    /// </summary>
    /// <param name="argActive">����</param>
    public void ChangeCursorState(bool argActive)
    {
        if(SceneManager.GetActiveScene().name != "Main")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if (argActive == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public static GameManager Instance
    {
        get { return g_gameDataManager; }
    }
    public SoundManager SoundManager
    {
        get { return m_soundManager; }
    }
    public OptionManager OptionManager
    {
        get { return m_optionManager; }
    }
    public Transform CanvasTrans
    {
        get { return m_canvasTrans; }
    }
    public List<int> RoundDicSortList
    {
        get { return m_roundDicSortList; }
    }
    public List<int> CharacterDicSortList
    {
        get { return m_characterDicSortList; }
    }

    public int CharacterCode
    {
        get { return m_characterKey; }
        set
        {
            if(m_characterDic[value].m_isHave == false)
            {
                Debug.Log("no have char");
                return;
            }

            m_characterKey = value;
        }
    }
    public int RoundIndex
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
    public long Money
    {
        get { return m_money; }
        set
        {
            m_money = value;
            if(m_money <= 0)
            {
                m_money = 0;
            }

            if(m_moneyPanel != null)
            {
                m_moneyPanel.SetMoneyText = value.ToString();
            }
        }
    }

}
