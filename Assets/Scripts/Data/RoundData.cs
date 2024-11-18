using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ���带 �����ϴ� �������Դϴ�
//����� ���۵Ǵ� �ð��� ������ �ð��� �������� ������ ���缭 ���� ���� �����Ͽ����մϴ�
[CreateAssetMenu(fileName = "Round Data Asset", menuName = "New RoundData")]
public class RoundData : ScriptableObject
{
    /// <summary>
    /// ���� �̸�
    /// </summary>
    public string m_name = string.Empty;
    /// <summary>
    /// ���� ǥ�� ����
    /// </summary>
    public bool m_isVisible = true;
    /// <summary>
    /// ���� �̹���
    /// </summary>
    public Sprite m_sampleSprite = null;
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
