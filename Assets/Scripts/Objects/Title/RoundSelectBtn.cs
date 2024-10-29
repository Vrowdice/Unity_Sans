using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSelectBtn : MonoBehaviour
{
    /// <summary>
    /// ���� ��������Ʈ ������
    /// </summary>
    [SerializeField]
    SpriteRenderer m_roundSpriteRenderer = null;

    /// <summary>
    /// Ÿ��Ʋ �Ŵ���
    /// </summary>
    private TitleManager m_titleManager = null;
    /// <summary>
    /// �ڽ��� ���� �ε���
    /// </summary>
    private int m_roundIndex = 0;

    public void SetRoundSelectBtn(TitleManager argTitleManager, Sprite argRoundSprite, int argRoundIndex)
    {
        m_titleManager = argTitleManager;
        m_roundSpriteRenderer.sprite = argRoundSprite;
        m_roundIndex = argRoundIndex;
    }

    public int GetRoundIndex
    {
        get { return m_roundIndex; }
    }
}
