using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAtk : MonoBehaviour
{
    /// <summary>
    /// ���� �����ϴ� ������Ʈ
    /// </summary>
    [SerializeField]
    private GameObject m_atkObj = null;
    /// <summary>
    /// ��� �ð�
    /// </summary>
    private float m_warnTime = 0.0f;
    /// <summary>
    /// ���� �ð�
    /// </summary>
    private float m_atkTime = 0.0f;

    /// <summary>
    /// ���Ÿ� ���� ����
    /// </summary>
    /// <param name="argWarnTime">��� �ð�</param>
    /// <param name="argAtkTime"></param>
    public void StartRangeAtk(float argWarnTime, float argAtkTime)
    {
        m_warnTime = argWarnTime;
        m_atkTime = argAtkTime;

        StartCoroutine(IEStartRangeAtk());
    }
    /// <summary>
    /// ���Ÿ� ���� ���� IE
    /// </summary>
    /// <returns>IE</returns>
    IEnumerator IEStartRangeAtk()
    {
        m_atkObj.SetActive(true);
        WarnAtk();
        yield return new WaitForSeconds(m_warnTime);
        StartAtk();
        yield return new WaitForSeconds(m_atkTime);
        ResetObj();
    }

    /// <summary>
    /// ���� ���
    /// </summary>
    void WarnAtk()
    {
        m_atkObj.GetComponent<MeshRenderer>().material = GameManager.Instance.GetWarnMat;
        m_atkObj.tag = "Untagged";
    }
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    void StartAtk()
    {
        m_atkObj.GetComponent<MeshRenderer>().material = GameManager.Instance.GetAtkObjMat;
        m_atkObj.tag = "Attack";
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void ResetObj()
    {
        GameManager.Instance.WaitRangeAtk(this);

        m_atkObj.tag = "Untagged";
        m_warnTime = 0.0f;
        m_atkTime = 0.0f;
        m_atkObj.SetActive(false);
    }
}
