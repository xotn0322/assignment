using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Variables
    [Header("Audio Source")]
    public AudioSource m_AudioSource;

    [Header("Audio Clip")]
    public AudioClip m_ClickSound;
    #endregion

    #region Play Sound
    public void PlayClickSound()
    {
        if (m_AudioSource != null && m_ClickSound != null)
            m_AudioSource.PlayOneShot(m_ClickSound);
    }
    #endregion
}
