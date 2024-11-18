using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ĳ���� ��Ų�� �����ϴ� �������Դϴ�
[CreateAssetMenu(fileName = "Character Data Asset", menuName = "New CharacterData")]
public class CharacterData : ScriptableObject
{
    /// <summary>
    /// ĳ���� �̸�
    /// </summary>
    public string m_name = string.Empty;

    /// <summary>
    /// ĳ���� ��Ų ������Ʈ
    /// </summary>
    public GameObject m_object = null;

    /// <summary>
    /// ĳ���� Ÿ��
    /// </summary>
    public CharacterType m_type = new CharacterType();
}
