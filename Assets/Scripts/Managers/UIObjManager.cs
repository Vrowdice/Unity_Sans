using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIObjManager : MonoBehaviour
{
    /// <summary>
    /// ���� ���� �ؽ�Ʈ ������Ʈ
    /// </summary>
    GameObject m_gameOverTextObj = null;
    /// <summary>
    /// �¸� �� ǥ���ϴ� �ؽ�Ʈ ������Ʈ
    /// </summary>
    GameObject m_youWinTextObj = null;
    /// <summary>
    /// ȸ�� ������ Ƚ���� ǥ���ϴ� �ؽ�Ʈ
    /// </summary>
    TextMeshPro m_recoverCountTextMeshPro = null;

    private void Awake()
    {
        m_gameOverTextObj = transform.Find("GameOverTextObj").gameObject;
        m_youWinTextObj = transform.Find("YouWinTextObj").gameObject;
        m_recoverCountTextMeshPro = transform.Find("RecoverBtn").Find("CountTextObj").GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// ������ ������ ��
    /// </summary>
    public void GameWinState(bool argIsWin)
    {
        if (argIsWin)
        {
            gameObject.SetActive(true);
            m_gameOverTextObj.SetActive(false);
            m_youWinTextObj.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            m_gameOverTextObj.SetActive(true);
            m_youWinTextObj.SetActive(false);
        }
    }
    /// <summary>
    /// �� ������Ʈ Ȱ��ȭ ����
    /// </summary>
    public void ActiveUIObj(bool argState)
    {
        gameObject.SetActive(argState);
        m_gameOverTextObj.SetActive(false);
        m_youWinTextObj.SetActive(false);
    }
    /// <summary>
    /// ȸ�� ���� Ƚ�� �ؽ�Ʈ ����
    /// </summary>
    /// <param name="argCount">��</param>
    public void SetRecoverCountTextObj(int argCount)
    {
        m_recoverCountTextMeshPro.text = "RECOVER : " + argCount;
    }
}
