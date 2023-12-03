using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Btn : MonoBehaviour
{
    GameManager GM;
    public Text bscoreText;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        bscoreText = GameObject.Find("BestScore").GetComponent<Text>();

        //�ְ����� ǥ��
        if (bscoreText != null)
        {
            bscoreText.text = "BestScore is " + GM.maxScore.ToString();
        }
    }

    //���� ��ư Ŭ��
    public void PlayBtn()
    {
        GM.isPlay = true;
        Debug.Log("����");

        GM.score = 0;
        SceneManager.LoadScene("SampleScene");
        StartCoroutine(GM.AddScore());
        GM.isCor = true;
    }
}
