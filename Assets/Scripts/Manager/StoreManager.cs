using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    /// <summary>
    /// ���� �г� ������
    /// </summary>
    [SerializeField]
    GameObject m_storePanelPrefeb = null;
    /// <summary>
    /// ĳ���� ���� ��ư ������
    /// </summary>
    [SerializeField]
    GameObject m_characterSelectBtnPrefeb = null;

    /// <summary>
    /// Ȯ�� ����ġ ����
    /// </summary>
    [SerializeField]
    float m_legendProWeight = 0.0f;
    [SerializeField]
    float m_epicProWeight = 0.0f;
    [SerializeField]
    float m_rareProWeight = 0.0f;
    [SerializeField]
    float m_normalProWeight = 0.0f;

    /// <summary>
    /// ĳ���� ���� ��ư ����Ʈ
    /// </summary>
    private List<CharacterSelectBtn> m_characterSelectBtnList = new List<CharacterSelectBtn>();

    /// <summary>
    /// �κ��丮 �г� Ʈ������
    /// </summary>
    private Transform m_inventoryPanelContentTrans = null;
    /// <summary>
    /// �̱� ��ư
    /// </summary>
    private Button m_rollBtn = null;
    /// <summary>
    /// �ι��丮 â�� ���� ��ư
    /// </summary>
    private Button m_invectoryBtn = null;

    // Start is called before the first frame update
    void Start()
    {
        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ��ŸƮ ����
    /// </summary>
    void StartSetting()
    {
        //�� UI���� �Ҵ�
        if (m_storePanelPrefeb != null || GameManager.Instance != null)
        {
            GameObject _storePanel = Instantiate(m_storePanelPrefeb, GameManager.Instance.GetCanvasTrans);
            m_storePanelPrefeb = _storePanel;

            Transform _inventoryPanel = _storePanel.transform.Find("InventoryPanel");
            m_inventoryPanelContentTrans = _inventoryPanel.GetChild(0);

            m_invectoryBtn = _storePanel.transform.Find("InventoryBtn").GetComponent<Button>();
            m_rollBtn = _storePanel.transform.Find("RollBtn").GetComponent<Button>();
        }

        //��ư �۾� �Ҵ�
        if(m_invectoryBtn != null)
        {
            m_invectoryBtn.onClick.AddListener(() => ResetInventoryPanel());
        }
        if(m_rollBtn != null)
        {
            m_invectoryBtn.onClick.AddListener(() => RollCharacter());
        }
    }

    /// <summary>
    /// ĳ���� �̱�
    /// </summary>
    public void RollCharacter()
    {

    }

    /// <summary>
    /// �������� ĳ���� ����
    /// </summary>
    /// <param name="argCharacterIndex">ĳ���� �ε���</param>
    public bool SelectCharacter(int argCharacterIndex)
    {
        if (GameManager.Instance.GetHaveCharactorList[argCharacterIndex] == false || argCharacterIndex <= -1)
        {
            GameManager.Instance.Alert("Don't have this character!");
            return false;
        }

        GameManager.Instance.SetCharacterIndex = argCharacterIndex;

        for(int i = 0; i < GameManager.Instance.GetHaveCharactorList.Count; i++)
        {
            if(GameManager.Instance.GetHaveCharactorList[i] == true)
            {
                m_characterSelectBtnList[i].CheckImageActive(true);
            }
            else
            {
                m_characterSelectBtnList[i].CheckImageActive(false);
            }
        }
        return true;
    }

    /// <summary>
    /// �κ��丮 �г� �ʱ�ȭ
    /// </summary>
    public void ResetInventoryPanel()
    {
        if (m_inventoryPanelContentTrans == null)
        {
            Debug.Log("no inv trans");
            return;
        }

        for (int i = 0; i < m_inventoryPanelContentTrans.childCount; i++)
        {
            Destroy(m_inventoryPanelContentTrans.GetChild(i).gameObject);
        }
        m_characterSelectBtnList.Clear();

        List<CharacterData> _characterData = GameManager.Instance.GetCharacterDataList;
        List<bool> _haveCharacterData = GameManager.Instance.GetHaveCharactorList;
        for (int i = 0; i < _characterData.Count; i++)
        {
            CharacterSelectBtn _btn = Instantiate(m_characterSelectBtnPrefeb, m_inventoryPanelContentTrans).GetComponent<CharacterSelectBtn>();
            m_characterSelectBtnList.Add(_btn);

            _btn.transform.GetChild(0).GetComponent<Text>().text = _characterData[i].m_name;

            if (_haveCharacterData[i] == true)
            {
                GameObject _sampleObj = Instantiate(_characterData[i].m_object, _btn.transform);
                _sampleObj.transform.localPosition = Vector3.zero;
                _sampleObj.transform.localScale = _sampleObj.transform.localScale * 100;

                _btn.ResetBtn(i, false, this);

                _btn.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                _btn.ResetBtn(i, false, this);

                _btn.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        SelectCharacter(GameManager.Instance.SetCharacterIndex);
    }
}
