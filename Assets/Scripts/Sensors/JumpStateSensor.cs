using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateSensor : MonoBehaviour
{
    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ� �ν��Ͻ�
    /// </summary>
    private PlayerController m_playerController = null;

    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            m_playerController.IsGroundFlag = true;
        }
    }
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terrain")  
        {
            m_playerController.GetIsCanJumpFlag = true;
            m_playerController.GetIsGroundFlag = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            m_playerController.GetIsGroundFlag = false;
        }
    }
}
