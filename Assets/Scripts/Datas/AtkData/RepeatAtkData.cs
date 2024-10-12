using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatAtkData
{
    /// <summary>
    /// ���� ������
    /// </summary>
    public AtkData m_atkData = new AtkData();
    /// <summary>
    /// �ݺ� ���� �ð�
    /// </summary>
    public float m_repeatStartTime = 0.0f;
    /// <summary>
    /// �ݺ� �ð�
    /// </summary>
    public float m_repeatTime = 0.0f;
    /// <summary>
    /// �ݺ� ������ �ð�
    /// </summary>
    public float m_repeatOverTime = 0.0f;
    /// <summary>
    /// ������ �ݺ��� �ð�
    /// </summary>
    public float m_toRepeatTime = 0.0f;
}
