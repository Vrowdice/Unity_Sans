using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// ��׶��� ����� �ҽ�
    /// </summary>
    private AudioSource m_backgroundSoundAS = null;
    /// <summary>
    /// ����Ʈ ������ҽ�
    /// </summary>
    private AudioSource m_effectSoundAS = null;

    // Start is called before the first frame update
    void Start()
    {
        m_backgroundSoundAS = transform.Find("BackGroundSound").GetComponent<AudioSource>();
        m_effectSoundAS = transform.Find("EffectSound").GetComponent<AudioSource>();
    }

    /// <summary>
    /// ����Ʈ ���
    /// </summary>
    /// <param name="argAudioClip">����� Ŭ��</param>
    public void PlayEffectSound(AudioClip argAudioClip)
    {
        m_effectSoundAS.PlayOneShot(argAudioClip);
    }
    /// <summary>
    /// ������� ���
    /// </summary>
    /// <param name="argAudioClip">����� Ŭ��</param>
    public void PlayBackgroundSound(AudioClip argAudioClip)
    {
        m_backgroundSoundAS.clip = argAudioClip;
        m_backgroundSoundAS.loop = true;
        m_backgroundSoundAS.Play();
    }

    /// <summary>
    /// �� ���� ����
    /// </summary>
    /// <param name="argVal">���� ��</param>
    public void BackgroundSoundVolume(float argVal)
    {
        m_backgroundSoundAS.volume = argVal;
    }
    public void EffectSoundVolume(float argVal)
    {
        m_effectSoundAS.volume = argVal;
    }

    /// <summary>
    /// ����� Ŭ�� ����
    /// </summary>
    public void ResetAudioClip()
    {
        m_backgroundSoundAS.clip = null;
        m_effectSoundAS.clip = null;

        m_backgroundSoundAS.Stop();
        m_effectSoundAS.Stop();
    }
}
