using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region Variables
    [Header("Start Room Button")]
    public Button StartRoomButton;
    public string nextSceneName;

    [Header("Setting Room Button")]
    public Button SettingRoomButton;

    [Header("Creator Room Button")]
    public Button CreatorRoomButton;

    [Header("Exit Room Button")]
    public Button ExitRoomButton;

    private AudioController audioController;
    #endregion

    void Start()
    {
        audioController = GetComponent<AudioController>();
        HandleButtons();
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
        });

        CreatorRoomButton.onClick.AddListener(delegate
        {
            audioController.PlayClickSound();
        });

        ExitRoomButton.onClick.AddListener(delegate
        {
            audioController.PlayClickSound();
        });
    }
    #endregion
}
