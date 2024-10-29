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
    /// �ɼ��� Ȱ��ȭ �Ǿ�����
    /// </summary>
    private bool m_optionState = false;

    // Start is called before the first frame update
    void Start()
    {
        if(GameDataManager.Instance == null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �� �����̴� �� ��ȭ ����
    /// </summary>
    public void BackGroundSlider()
    {
        GameDataManager.Instance.GetSoundManager.BackgroundSoundVolume(m_backgroundSoundSlider.value / 100);
    }
    public void EffectSoundSlider()
    {
        GameDataManager.Instance.GetSoundManager.EffectSoundVolume(m_effectSoundSlider.value / 100);
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
        GameDataManager.Instance.GoTitleScene();
    }

    public void OptionState(bool argState)
    {
        m_optionState = argState;

        if (m_optionState == true)
        {
            m_optionPanel.SetActive(true);
            GameDataManager.Instance.CursorState(true);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GetPlayerController.SetPlayerControllFlag = false;
            }
            Time.timeScale = 0;
        }
        else
        {
            m_optionPanel.SetActive(false);
            GameDataManager.Instance.CursorState(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GetPlayerController.SetPlayerControllFlag = true;
            }
            Time.timeScale = 1;
        }
    }

    public bool GetOptionState
    {
        get { return m_optionState; }
    }
}
