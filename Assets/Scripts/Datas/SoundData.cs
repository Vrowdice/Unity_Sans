using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Asset", menuName = "New SoundData")]
public class SoundData : ScriptableObject
{
    /// <summary>
    /// ���� �׸� ��
    /// </summary>
    public AudioClip m_backGround = null;
    /// <summary>
    /// ���� ���� ��
    /// </summary>
    public AudioClip m_hit = null;
    /// <summary>
    /// ü�� ȸ�� ��
    /// </summary>
    public AudioClip m_heal = null;
    /// <summary>
    /// ���� �ε����� ��
    /// </summary>
    public AudioClip m_hitGround = null;
    /// <summary>
    /// �߷� ���� ȿ����
    /// </summary>
    public AudioClip m_gravityAtk = null;
    /// <summary>
    /// Ƣ����� ���� ȿ����
    /// </summary>
    public AudioClip m_popAtk = null;
    /// <summary>
    /// ���Ÿ� ���� ȿ����
    /// </summary>
    public AudioClip m_rangeAtk = null;
}
