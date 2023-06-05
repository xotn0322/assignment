using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [Header("Back Button")]
    public Button BackButton;
    public string TitleSceneName;

    private AudioController audioController;

    void Start()
    {
        audioController = GetComponent<AudioController>();
        HandleButtons();
    }

    private void HandleButtons()
    {
        BackButton.onClick.AddListener(delegate
        {
            audioController.PlayClickSound();
            LoadSceneManager.LoadScene(TitleSceneName);
        });
    }
}
