using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������
/// </summary>
public class AtkData
{
    //���� ����
    public int m_order = -1;
    //���� Ÿ��
    public AtkType m_type = new AtkType();
    //�����̴��� �ȿ����̴���
    //�����Ϳ����� 0�� 1�� ǥ��
    //0 = false
    //1 = true
    public bool m_isMove = false;
    //�����̴� �ӵ�
    public float m_speed = 0.0f;
    //���� �ð�
    public float m_genTime = 0.0f;

    //���� ������Ʈ ������
    //���� ��ġ, ���� ����
    //�����̴� ������Ʈ�� �������� �̵���
    //���� ���⵵ �����̼ǿ� ���� �޶���
    public Vector3 m_size = new Vector3();
    public Vector3 m_position = new Vector3();
    public Vector3 m_rotation = new Vector3();
}
