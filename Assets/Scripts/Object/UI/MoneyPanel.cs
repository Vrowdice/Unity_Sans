using UnityEngine;
using UnityEngine.UI;

public class MoneyPanel : MonoBehaviour
{
    /// <summary>
    /// �� �ؽ�Ʈ
    /// </summary>
    [SerializeField]
    Text m_moneyText = null;

    public string SetMoneyText
    {
        get { return m_moneyText.text; }
        set
        {
            m_moneyText.text = value;
        }
    }
}
