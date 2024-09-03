using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ���� ��
    /// </summary>
    [SerializeField]
    float m_jumpForce = 50.0f;

    /// <summary>
    /// ���� ������ �ӵ�
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 50.0f;

    /// <summary>
    /// �밢�� ������ �ӵ�
    /// </summary>
    [SerializeField]
    float m_diagonalMoveSpeed = 0.5f;

    /// <summary>
    /// ���콺 ����
    /// </summary>
    [SerializeField]
    float m_xSensitivity = 1.0f;
    [SerializeField]
    float m_ySensitivity = 1.0f;

    /// <summary>
    /// ī�޶�
    /// </summary>
    [SerializeField]
    public Transform m_cameraBase;

    /// <summary>
    /// ������ٵ�
    /// </summary>
    Rigidbody m_rb;

    /// <summary>
    /// Ű �Է� Ȯ���� ���� �Ҹ�
    /// </summary>
    bool InputKey_W = false;
    bool InputKey_S = false;
    bool InputKey_A = false;
    bool InputKey_D = false;

    /// <summary>
    /// ���� �÷���
    /// </summary>
    bool m_jumpFlag = true;

    /// <summary>
    /// ���� ����� ��
    /// </summary>
    /// <param name="col">�ݸ���</param>
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            m_jumpFlag = true;
        }
    }

    /// <summary>
    /// ������ �������� ����
    /// </summary>
    /// <param name="col">�ݸ���</param>
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            m_jumpFlag = false;
        }
    }

    /// <summary>
    /// start
    /// </summary>
    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// update
    /// </summary>
    private void Update()
    {
        PlayerControll();
        CameraControll();
    }

    /// <summary>
    /// �̵��� ���� Ű �Է� �� ����
    /// </summary>
    void PlayerControll()
    {

    }

    /// <summary>
    /// �÷��̾� ī�޶� ��Ʈ��
    /// </summary>
    void CameraControll()
    {
        float _yRota = Input.GetAxis("Mouse X") * m_xSensitivity;
        float _xRota = Input.GetAxis("Mouse Y") * m_ySensitivity;

        transform.localRotation *= Quaternion.Euler(0, _yRota, 0);
        m_cameraBase.localRotation *= Quaternion.Euler(-_xRota, 0, 0);
    }
}
