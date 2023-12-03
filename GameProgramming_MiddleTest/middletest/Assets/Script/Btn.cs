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

        //최고점수 표기
        if (bscoreText != null)
        {
            bscoreText.text = "BestScore is " + GM.maxScore.ToString();
        }
    }

    //시작 버튼 클릭
    public void PlayBtn()
    {
        GM.isPlay = true;
        Debug.Log("시작");

        GM.score = 0;
        SceneManager.LoadScene("SampleScene");
        StartCoroutine(GM.AddScore());
        GM.isCor = true;
    }
}
