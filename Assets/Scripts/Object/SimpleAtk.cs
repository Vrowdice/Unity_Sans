using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAtk : MonoBehaviour
{
    /// <summary>
    /// �����̰� ���� ���
    /// </summary>
    public bool m_isMove = false;
    /// <summary>
    /// �����̴� �÷��̾� ���� ��ȯ
    /// </summary>
    public bool m_isMovePlayerAtk = false;
    /// <summary>
    /// �ӵ�
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// ���� ��� ��ȯ
    /// </summary>
    /// <param name="argIsMovePlayerAtk">true �����̴� �÷��̾� ��������</param>
    public void ChangeMovePlayerAtk(bool argIsMovePlayerAtk)
    {
        if (argIsMovePlayerAtk)
        {
            GetComponent<MeshRenderer>().material = GameManager.Instance.IsMovePlayerAtkObjMat;
            m_isMovePlayerAtk = true;
            gameObject.tag = "MovePlayerAttack";
        }
        else
        {
            GetComponent<MeshRenderer>().material = GameManager.Instance.IsAtkObjMat;
            m_isMovePlayerAtk = false;
            gameObject.tag = "Attack";
        }
    }

    /// <summary>
    /// Ƣ������� ���� ��ü ó��
    /// </summary>
    /// <param name="argWaitTime">���� �� ��ٸ��� �ð�</param>
    /// <param name="argSpeed">���� �ӵ�</param>
    /// <param name="argMaxHeight">�ִ� ���� ����</param>
    public void PopAtk(float argWaitTime, float argSpeed, float argMaxHeight)
    {
        StartCoroutine(IEPopAtk(argWaitTime, argSpeed, argMaxHeight));
    }
    /// <summary>
    /// Ƣ������� ���� IE ��ü ó��
    /// </summary>
    /// <param name="argWaitTime">���� �� ��ٸ��� �ð�</param>
    /// <param name="argSpeed">���� �ӵ�</param>
    /// <param name="argMaxHeight">�ִ� ���� ����</param>
    /// <returns>IE</returns>
    IEnumerator IEPopAtk(float argWaitTime, float argSpeed, float argMaxHeight)
    {
        // �ö󰡱�
        while (true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, argMaxHeight, transform.position.z),
                argSpeed * Time.deltaTime);
            if (Mathf.Abs(argMaxHeight - transform.position.y) < 0.01f)
            {
                break;
            }

            yield return null;
        }
        yield return new WaitForSeconds(argWaitTime - 1.0f);

        // ��������
        while (true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, -10.0f, transform.position.z),
                argSpeed * Time.deltaTime);
            if (Mathf.Abs(-10.0f - transform.position.y) < 0.01f)
            {
                GameManager.Instance.WaitSimpleAtk(this);
                break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void ResetObj()
    {
        ChangeMovePlayerAtk(false);
        m_isMove = false;
        m_speed = 0.0f;
    }
}
