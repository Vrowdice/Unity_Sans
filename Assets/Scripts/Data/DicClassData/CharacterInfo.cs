
//��ųʸ��� �� ĳ���� ���� ������
public class CharacterInfo
{
    public CharacterInfo(CharacterData argData, bool argIsHave)
    {
        m_data = argData;
        m_isHave = argIsHave;
    }

    /// <summary>
    /// ĳ���� ������
    /// </summary>
    public CharacterData m_data = null;

    /// <summary>
    /// �� ĳ���͸� ������ ���� ��� true
    /// </summary>
    public bool m_isHave = false;
}
