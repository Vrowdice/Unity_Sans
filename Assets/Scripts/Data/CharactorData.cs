using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ĳ���� ��Ų�� �����ϴ� �������Դϴ�
[CreateAssetMenu(fileName = "Charactor Data Asset", menuName = "New CharactorData")]
public class CharactorData : ScriptableObject
{
    /// <summary>
    /// ĳ���� �ε���
    /// </summary>
    public int m_index = 0;

    /// <summary>
    /// ĳ���� ��Ų ������Ʈ
    /// </summary>
    public GameObject m_object = null;

    /// <summary>
    /// ĳ���� ��Ų ����
    /// </summary>
    public long m_price = 0;
}
