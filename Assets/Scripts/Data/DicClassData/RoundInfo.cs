
//��ųʸ��� �� ���� ���� ������
public class RoundInfo
{
    public RoundInfo(RoundData argData, bool argIsClear, bool argIsHardcoreClear)
    {
        m_data = argData;
        m_isClear = argIsClear;
        m_isHardcoreClear = argIsHardcoreClear;
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    public RoundData m_data = null;

    /// <summary>
    /// �� ���带 Ŭ���� �� ���
    /// </summary>
    public bool m_isClear = false;

    /// <summary>
    /// �� ������ �ϵ��ھ� ������ Ŭ���� �� ���
    /// </summary>
    public bool m_isHardcoreClear = false;
}
