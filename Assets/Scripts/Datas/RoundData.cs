using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Round Data Asset", menuName = "New RoundData")]
public class RoundData : ScriptableObject
{
    /// <summary>
    /// ���� �ε���
    /// Resources ���� ���� �̸��� ���� �̸��� ����
    /// </summary>
    public int m_roundIndex = 0;
    /// <summary>
    /// ���� �̸�
    /// </summary>
    public string m_roundName = string.Empty;
    /// <summary>
    /// ���� ǥ�� ����
    /// </summary>
    public bool m_roundVisible = true;
    /// <summary>
    /// ���� �̹���
    /// </summary>
    public Sprite m_roundSprite = null;
    /// <summary>
    /// ����� ���۵Ǵ� �ð� ����
    /// </summary>
    public List<float> m_phaseStartTimeSet = new List<float>();
    /// <summary>
    /// ����� ������ �ð�
    /// </summary>
    public List<float> m_phaseOverTime = new List<float>();
    /// <summary>
    /// ���忡 ����� ���� ������
    /// </summary>
    public SoundData m_soundData = null;
}
