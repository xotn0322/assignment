using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region Variables
    [Header("Main Panel")]
    public GameObject MainPanel;

    [Header("Start Room Button")]
    public Button StartRoomButton;
    public string nextSceneName;

    [Header("Setting Room Button")]
    public Button SettingRoomButton;
    public GameObject m_SettingPanel;
    public Button s_BackButton;

    [Header("Creator Room Button")]
    public Button CreatorRoomButton;
    public GameObject m_CreatorPanel;
    public Button c_BackButton;


    [Header("Exit Room Button")]
    public Button ExitRoomButton;

    private AudioController audioController;

    public AudioMixer audioSource;
    public Slider BgmSlider;
    public Slider VfxSlider;

    #endregion

    void Start()
    {
        audioController = GetComponent<AudioController>();
        HandleButtons();
    }

    // 볼륨을 조절하는 메서드
    public void SetBgmVolume()
    {
        audioSource.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }

    // 볼륨을 조절하는 메서드
    public void SetVfxVolume()
    {
        audioSource.SetFloat("VFX", Mathf.Log10(BgmSlider.value) * 20);
    }

    #region Handle UI
    private void HandleButtons()
    {
        StartRoomButton.onClick.AddListener(delegate
        {
            audioController.PlayClickSound();
            LoadSceneManager.LoadScene(nextSceneName);
        });

        SettingRoomButton.onClick.AddListener(delegate
        {
            audioController.PlayClickSound();
            MainPanel.SetActive(false);
            m_SettingPanel.SetActive(true);
        });

        CreatorRoomButton.onClick.AddListener(delegate
        {
            audioController.PlayClickSound();
            MainPanel.SetActive(false);
            m_CreatorPanel.SetActive(true);
        });

        ExitRoomButton.onClick.AddListener(delegate
        {
            audioController.PlayClickSound();
            Application.Quit();
        });

        s_BackButton.onClick.AddListener(delegate
        {
            m_SettingPanel.SetActive(false);
            MainPanel.SetActive(true);
        });

        c_BackButton.onClick.AddListener(delegate
        {
            m_CreatorPanel.SetActive(false);
            MainPanel.SetActive(true);
        });

    }
    #endregion


}
