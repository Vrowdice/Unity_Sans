using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Slider m_hpSlider = null;
    [SerializeField]
    Slider m_lateHpSlider = null;
    [SerializeField]
    Text m_hpText = null;

    private bool isUpdatingSlider = false; // �����̴� ���� �ڵ忡�� ������Ʈ ������ Ȯ��

    public void SetSliders(int argVal)
    {
        m_hpSlider.maxValue = argVal;
        m_hpSlider.value = m_hpSlider.maxValue;

        m_lateHpSlider.maxValue = argVal;
        m_lateHpSlider.value = m_lateHpSlider.maxValue;

        HpSlider(argVal);
        LateHpSlider(argVal);
    }

    public void HpSlider(float argValue)
    {
        if (!isUpdatingSlider) // �̺�Ʈ ����
        {
            isUpdatingSlider = true;
            m_hpSlider.value = argValue;
            isUpdatingSlider = false;
            HpText();
        }
    }

    public void LateHpSlider(float argValue)
    {
        if (!isUpdatingSlider) // �̺�Ʈ ����
        {
            isUpdatingSlider = true;
            m_lateHpSlider.value = argValue;
            isUpdatingSlider = false;
        }
    }

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
