using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : MonoBehaviour
{
    /// <summary>
    /// �����̰� ���� ���
    /// </summary>
    public bool m_isMove = false;
    /// <summary>
    /// �ӵ�
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void ResetObj()
    {
        m_isMove = false;
        m_speed = 0.0f;
    }
}
