using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    /// <summary>
    /// �ɼ� �г�
    /// </summary>
    [SerializeField]
    GameObject m_optionPanel = null;

    /// <summary>
    /// ��׶��� ���� �����̴�
    /// </summary>
    [SerializeField]
    Slider m_backgroundSoundSlider = null;
    /// <summary>
    /// ����Ʈ ���� �����̴�
    /// </summary>
    [SerializeField]
    Slider m_effectSoundSlider = null;
    /// <summary>
    /// ���� ǥ���� �ý�Ʈ
    /// </summary>
    Text m_backgroundValueText = null;
    Text m_effectSoundText = null;

    /// <summary>
    /// �ɼ��� Ȱ��ȭ �Ǿ�����
    /// </summary>
    private bool m_optionState = false;

    // Start is called before the first frame update
    void Start()
    {
        //���� �Ŵ����� ���� ��� ����
        if(GameManager.Instance == null)
        {
            Destroy(gameObject);
        }

        //�ý�Ʈ ��������
        m_backgroundValueText = m_backgroundSoundSlider.transform.GetChild(0).GetComponent<Text>();
        m_effectSoundText = m_effectSoundSlider.transform.GetChild(0).GetComponent<Text>();

        //�ʱ�ȭ
        BackGroundSlider();
        EffectSoundSlider();
    }

    /// <summary>
    /// �� �����̴� �� ��ȭ ����
    /// </summary>
    public void BackGroundSlider()
    {
        GameManager.Instance.GetSoundManager.BackgroundSoundVolume(m_backgroundSoundSlider.value / 100);
        m_backgroundValueText.text = m_backgroundSoundSlider.value.ToString();
    }
    public void EffectSoundSlider()
    {
        GameManager.Instance.GetSoundManager.EffectSoundVolume(m_effectSoundSlider.value / 100);
        m_effectSoundText.text = m_effectSoundSlider.value.ToString();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    /// <summary>
    /// Ÿ��Ʋ�� �̵�
    /// </summary>
    public void BackTitle()
    {
        GameManager.Instance.GoTitleScene();
    }

    public void OptionState(bool argState)
    {
        m_optionState = argState;

        if (m_optionState == true)
        {
            m_optionPanel.SetActive(true);
            GameManager.Instance.ChangeCursorState(true);

            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.GetPlayerController.SetPlayerControllFlag = false;
            }
            Time.timeScale = 0;
        }
        else
        {
            m_optionPanel.SetActive(false);
            GameManager.Instance.ChangeCursorState(false);

            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.GetPlayerController.SetPlayerControllFlag = true;
            }
            Time.timeScale = 1;
        }
    }

    public bool GetOptionState
    {
        get { return m_optionState; }
    }
}
