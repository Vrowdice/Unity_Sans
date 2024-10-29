using System.Collections;
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
    /// �ɼ� �Ŵ��� ������
    /// </summary>
    [SerializeField]
    GameObject m_optionManagerPrefeb = null;
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
    /// ���� ���� �ε���
    /// </summary>
    private int m_roundIndex = 0;
    /// <summary>
    /// ��
    /// </summary>
    private long m_money = 0;
    /// <summary>
    /// ���� �� ĵ������ Ʈ������
    /// </summary>
    private Transform m_canvasTrans = null;

    private void OnEnable()
    {
        OnEnableSetting();
    }

    private void OnEnableSetting()
    {
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
        SceneManager.sceneLoaded += SceneLoaded;

        m_soundManager = GetComponent<SoundManager>();
        CursorState(true);
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
            m_optionManager = Instantiate(m_optionManagerPrefeb, m_canvasTrans).GetComponent<OptionManager>();
            m_optionManager.OptionState(false);
        }
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

    /// <summary>
    /// ���� ���� ������ �̵�
    /// </summary>
    public void GoMainScene(int argRoundIndex)
    {
        m_soundManager.ResetAudioClip();

        m_roundIndex = argRoundIndex;
        SceneManager.LoadScene("Main");
    }
    /// <summary>
    /// Ÿ��Ʋ ������ �̵�
    /// </summary>
    public void GoTitleScene()
    {
        m_soundManager.ResetAudioClip();

        SceneManager.LoadScene("Title");
        CursorState(true);
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
    public void CursorState(bool argActive)
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

    public static GameDataManager Instance
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
        }
    }

}
