using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// ���� (���� �ٸ� ���� ���嵵 ���� ����)
    /// </summary>
    [SerializeField]
    Sound m_roundSound = null;
    /// <summary>
    /// ��׶��� ����� �ҽ�
    /// </summary>
    private AudioSource m_backGroundSoundAS = null;
    /// <summary>
    /// ����Ʈ ������ҽ�
    /// </summary>
    private AudioSource m_effectSoundAS = null;

    // Start is called before the first frame update
    void Start()
    {
        m_backGroundSoundAS = transform.Find("BackGroundSound").GetComponent<AudioSource>();
        m_effectSoundAS = transform.Find("EffectSound").GetComponent<AudioSource>();
    }

    public void PlayEffectSound(AudioClip argAudioClip)
    {
        m_effectSoundAS.PlayOneShot(argAudioClip);
    }

    public Sound GetSound
    {
        get { return m_roundSound; }
    }
}
