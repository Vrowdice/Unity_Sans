using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Hp �����̴�
    /// </summary>
    [SerializeField]
    Slider m_hpSlider = null;
    /// <summary>
    /// �ð��� ������ ���� ��� Hp �����̴�
    /// </summary>
    [SerializeField]
    Slider m_lateHpSlider = null;
    /// <summary>
    /// Hp �� �ؽ�Ʈ
    /// </summary>
    [SerializeField]
    Text m_hpText = null;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// �����̴� �ʱ� ����
    /// </summary>
    /// <param name="argHpSliderVal">hp �����̴� �ִ밪</param>
    /// <param name="argLateHpSliderVal">late hp �����̴� �ִ밪</param>
    public void SetSliders(int argVal)
    {
        m_hpSlider.maxValue = argVal;
        m_hpSlider.value = m_hpSlider.maxValue;

        m_lateHpSlider.maxValue = argVal;
        m_lateHpSlider.value = m_lateHpSlider.maxValue;

        HpText();
    }

    /// <summary>
    /// hp �����̴� ���ϱ�
    /// </summary>
    /// <param name="argValue">���� ��</param>
    public void HpSlider(float argValue)
    {
        m_hpSlider.value = argValue;

        HpText();
    }
    /// <summary>
    /// ���߿� �������� hp �����̴� ���ϱ�
    /// </summary>
    /// <param name="argValue">���� ��</param>
    public void LateHpSlider(float argValue)
    {
        m_lateHpSlider.value = argValue;
    }

    /// <summary>
    /// hp �ؽ�Ʈ �����̴� ������ ����
    /// </summary>
    void HpText()
    {
        string _str = string.Empty;
        
        if (m_hpSlider.value / 10.0f < 1.0f)
        {
            _str = "0";
        }
        _str += m_hpSlider.value;

        m_hpText.text = _str + " / " + m_hpSlider.maxValue;
    }
}
