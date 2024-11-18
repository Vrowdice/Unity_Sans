using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectBtn : MonoBehaviour
{
    /// <summary>
    /// üũ �̹���
    /// </summary>
    [SerializeField]
    GameObject m_checkImage = null;

    /// <summary>
    /// ���� ���� �Ŵ���
    /// </summary>
    private StoreManager m_storManager = null;

    /// <summary>
    /// �� ��ư�� �۵��ϴ� �ε���
    /// </summary>
    private int m_key = -1;

    /// <summary>
    /// ��ư ����
    /// </summary>
    /// <param name="argKey">ĳ���� �ε���</param>
    /// <param name="argCheckImageActive">üũ ǥ�� ����</param>
    /// <param name="argStorManager">����� �Ŵ��� �ν��Ͻ�</param>
    public void ResetBtn(int argKey, bool argCheckImageActive, StoreManager argStorManager)
    {
        m_key = argKey;
        m_checkImage.SetActive(argCheckImageActive);
        m_storManager = argStorManager;
    }
    /// <summary>
    /// üũ �̹��� Ȱ��ȭ
    /// </summary>
    /// <param name="argCheckImageActive">Ȱ��ȭ ����</param>
    public void CheckImageActive(bool argCheckImageActive)
    {
        m_checkImage.SetActive(argCheckImageActive);
    }

    /// <summary>
    /// Ŭ�� ��
    /// </summary>
    public void Click()
    {
        if(m_storManager == null)
        {
            return;
        }

        m_storManager.SelectCharacter(m_key);
    }
}
