using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [Header("Panel Setting")]
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

    [Header("Gacha")]
    /// <summary>
    /// Ȯ�� ����ġ ���� ����Ʈ
    /// 
    /// 0 = legend
    /// 1 = epic
    /// 2 = rare
    /// 3 = normal
    /// 
    /// �� ����ġ ���� �� ���� ����� ĳ���ʹ�
    /// �������� ����
    /// </summary>
    [SerializeField]
    List<float> m_proWeightList = new List<float>();

    /// <summary>
    /// ��í ������ �Ϳ� �ʿ��� ��
    /// </summary>
    [SerializeField]
    long m_gachaMoney = 0;

    /// <summary>
    /// ĳ���� ���� ��ư ����Ʈ
    /// </summary>
    private List<CharacterSelectBtn> m_characterSelectBtnList = new List<CharacterSelectBtn>();
    /// <summary>
    /// �κ��丮 �г� Ʈ������
    /// </summary>
    private Transform m_inventoryPanelContentTrans = null;
    /// <summary>
    /// ��í�� ������ ���� ĳ������ ���� ������Ʈ�� ǥ��
    /// </summary>
    private GameObject m_sampleCharacterObj = null;
    /// <summary>
    /// ��í�� ������ �� �̹����� ǥ������ ����
    /// </summary>
    private Image m_rollReadyImage = null;
    /// <summary>
    /// ��í ���� ���� �ؽ�Ʈ
    /// </summary>
    private Text m_gachaInfoText = null;
    /// <summary>
    /// �̱� ��ư
    /// </summary>
    private Button m_rollBtn = null;
    /// <summary>
    /// �ι��丮 â�� ���� ��ư
    /// </summary>
    private Button m_invectoryBtn = null;

    /// <summary>
    /// ��� ����ġ�� ���� ���
    /// </summary>
    private float m_allProWeightCount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartSetting();
    }

    /// <summary>
    /// ��ŸƮ ����
    /// </summary>
    void StartSetting()
    {
        //�� UI���� �Ҵ�
        if (m_storePanelPrefeb != null || GameManager.Instance != null)
        {
            GameObject _storePanel = Instantiate(m_storePanelPrefeb, GameManager.Instance.CanvasTrans);
            m_storePanelPrefeb = _storePanel;

            Transform _inventoryPanel = _storePanel.transform.Find("InventoryPanel");
            m_inventoryPanelContentTrans = _inventoryPanel.GetChild(0);

            m_invectoryBtn = _storePanel.transform.Find("InventoryBtn").GetComponent<Button>();
            m_rollBtn = _storePanel.transform.Find("RollBtn").GetComponent<Button>();
            m_gachaInfoText = _storePanel.transform.Find("GachaInfoText").GetComponent<Text>();
            m_rollReadyImage = _storePanel.transform.Find("RollReadyImage").GetComponent<Image>();
            _storePanel.transform.Find("GachaMoneyText").GetComponent<Text>().text = "Gacha Money : " + m_gachaMoney;
        }

        //��ư �۾� �Ҵ�
        if(m_invectoryBtn != null)
        {
            m_invectoryBtn.onClick.AddListener(() => ResetInventoryPanel());
        }
        if(m_rollBtn != null)
        {
            m_rollBtn.onClick.AddListener(() => RollCharacter());
        }
        
        //��� ����ġ�� ���� �� ���
        for(int i = 0; i < m_proWeightList.Count; i++)
        {
            m_allProWeightCount = m_allProWeightCount + m_proWeightList[i];
        }
    }

    /// <summary>
    /// ĳ���� Ÿ���� ��︮�� ���� ����
    /// </summary>
    /// <param name="argType">ĳ���� Ÿ��</param>
    /// <returns>��</returns>
    Color CharTypeToColor(CharacterType argType)
    {
        switch (argType)
        {
            case CharacterType.Legend:
                return Color.yellow;
            case CharacterType.Epic:
                return Color.magenta;
            case CharacterType.Rare:
                return Color.blue;
            case CharacterType.Normal:
                return Color.white;
            default:
                return Color.white;
        }
    }

    /// <summary>
    /// ĳ���� �̱�
    /// </summary>
    public void RollCharacter()
    {
        if(GameManager.Instance.Money <= m_gachaMoney)
        {
            GameManager.Instance.Alert("Not Enough Money!");
            return;
        }

        int _characterKey = 0;
        float _rand = Random.Range(0.0f, m_allProWeightCount);
        List<int> _charSortList = GameManager.Instance.CharacterDicSortList;
        List<int> _selectedCharList = new List<int>();

        //���ϸ鼭 ����ġ ���
        float _weight = 0.0f;
        for(int i = 0; i < m_proWeightList.Count; i++)
        {
            _weight = _weight + m_proWeightList[i];
            if(_weight >= _rand)
            {
                _characterKey = (i + 1) * 10000;

                for(int j = 0; j < _charSortList.Count; j++)
                {
                    if (_charSortList[j] / _characterKey == 1)
                    {
                        _selectedCharList.Add(_charSortList[j]);
                    }
                }
                
            }
        }

        //���� ��� ���� �� ��� �ȿ��� �ε��� ���� ����
        int _randInt = Random.Range(0, _selectedCharList.Count);
        CharacterInfo _characterInfo = GameManager.Instance.GetCharacterInfo(_selectedCharList[_randInt]);

        //�̹� ������ ���� ��� �Ϻ� �ݾ� ��ȯ
        if (_characterInfo.m_isHave == true)
        {
            GameManager.Instance.Money += m_gachaMoney / 5;
            m_gachaInfoText.text = "Refund " + m_gachaMoney / 5;
        }
        else
        {
            _characterInfo.m_isHave = true;
            ResetInventoryPanel();
            m_gachaInfoText.text = "New Character: " + _characterInfo.m_data.m_name;
        }

        //��í �ݾ� ����
        GameManager.Instance.Money -= m_gachaMoney;

        if(m_sampleCharacterObj != null)
        {
            Destroy(m_sampleCharacterObj);
        }
        if(_characterInfo.m_data.m_object != null)
        {
            m_sampleCharacterObj = Instantiate(_characterInfo.m_data.m_object, m_rollReadyImage.transform);
            m_sampleCharacterObj.transform.localPosition = Vector3.zero;
            m_sampleCharacterObj.transform.localScale = m_sampleCharacterObj.transform.localScale * 100;
        }

        m_rollReadyImage.enabled = false;
    }

    /// <summary>
    /// �������� ĳ���� ����
    /// </summary>
    /// <param name="argKey">ĳ���� Ű</param>
    public bool SelectCharacter(int argKey)
    {
        if (GameManager.Instance.GetCharacterInfo(argKey).m_isHave == false || argKey <= -1)
        {
            GameManager.Instance.Alert("Don't have this character!");
            return false;
        }

        GameManager.Instance.CharacterCode = argKey;

        for(int i = 0; i < m_characterSelectBtnList.Count; i++)
        {
            if(GameManager.Instance.CharacterDicSortList[i] == argKey)
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

        Destroy(m_sampleCharacterObj);
        m_rollReadyImage.enabled = true;
        m_gachaInfoText.text = string.Empty;

        for (int i = 0; i < m_inventoryPanelContentTrans.childCount; i++)
        {
            Destroy(m_inventoryPanelContentTrans.GetChild(i).gameObject);
        }
        m_characterSelectBtnList.Clear();

        List<int> _characterKey = GameManager.Instance.CharacterDicSortList;
        for (int i = 0; i < _characterKey.Count; i++)
        {
            CharacterInfo _infoData = GameManager.Instance.GetCharacterInfo(_characterKey[i]);

            CharacterSelectBtn _btn = Instantiate(m_characterSelectBtnPrefeb, m_inventoryPanelContentTrans).GetComponent<CharacterSelectBtn>();
            m_characterSelectBtnList.Add(_btn);

            _btn.transform.GetChild(0).GetComponent<Text>().text = _infoData.m_data.m_name;

            if (_infoData.m_isHave == true)
            {
                if(_infoData.m_data.m_object != null)
                {
                    GameObject _sampleObj = Instantiate(_infoData.m_data.m_object, _btn.transform);
                    _sampleObj.transform.localPosition = Vector3.zero;
                    _sampleObj.transform.localScale = _sampleObj.transform.localScale * 100;
                }

                _btn.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                _btn.transform.GetChild(1).gameObject.SetActive(true);
            }

            _btn.ResetBtn(_characterKey[i], false, this);
            _btn.GetComponent<Image>().color = CharTypeToColor(_infoData.m_data.m_type);
        }

        SelectCharacter(GameManager.Instance.CharacterCode);
    }
}
