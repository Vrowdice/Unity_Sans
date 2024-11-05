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
    List<CharactorData> m_charactorDataList = new List<CharactorData>();
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
    /// ���� �� ĵ������ Ʈ������
    /// </summary>
    private Transform m_canvasTrans = null;

    /// <summary>
    /// �÷��̾ ������ �ִ� ĳ���� ����Ʈ
    /// true = ������
    /// false = �������� �ƴ�
    /// 
    /// ���� �ʿ�
    /// </summary>
    List<bool> m_haveCharactorList = new List<bool>();
    /// <summary>
    /// �÷��̾ Ŭ���� �� ���� ����Ʈ
    /// true = Ŭ���� ��
    /// false = Ŭ���� ���� ����
    /// 
    /// ���� �ʿ�
    /// </summary>
    List<bool> m_clearRoundList = new List<bool>();
    /// <summary>
    /// ��
    /// 
    /// ���� �ʿ�
    /// </summary>
    private long m_money = 10000;

    private void Awake()
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

    private void OnEnable()
    {
        OnEnableSetting();
    }

    private void OnEnableSetting()
    {
        //���� ����Ʈ ����
        ResetSaveList();
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
            SetMoney = m_money;
        }
    }
    /// <summary>
    /// ���̺� �� ����Ʈ �ʱ�ȭ
    /// ��� false �� �Ҵ�
    /// </summary>
    void ResetSaveList()
    {
        for(int i = 0; i < m_roundDataList.Count; i++)
        {
            m_clearRoundList.Add(false);
        }
        for(int i = 0;  i < m_charactorDataList.Count; i++)
        {
            m_haveCharactorList.Add(false);
        }
    }

    /// <summary>
    /// ĳ���� ������ ��������
    /// </summary>
    /// <param name="argIndex">ĳ���� �ε���</param>
    /// <returns>ĳ���� ������</returns>
    public CharactorData GetCharactorData(int argIndex)
    {
        if(argIndex < 0 || m_charactorDataList.Count < argIndex)
        {
            return null;
        }
        return m_charactorDataList[argIndex];
    }
    /// <summary>
    /// ���� ������ ��������
    /// </summary>
    /// <param name="argIndex">���� �ε���</param>
    /// <returns>���� ������</returns>
    public RoundData GetRoundData(int argIndex)
    {
        if (argIndex < 0 || m_roundDataList.Count < argIndex)
        {
            return null;
        }
        return m_roundDataList[argIndex];
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
    public SoundManager GetSoundManager
    {
        get { return m_soundManager; }
    }
    public OptionManager GetOptionManager
    {
        get { return m_optionManager; }
    }
    public List<RoundData> GetRoundDataList
    {
        get { return m_roundDataList; }
    }
    public Transform GetCanvasTrans
    {
        get { return m_canvasTrans; }
    }

    public int SetRoundIndex
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
    public long SetMoney
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
