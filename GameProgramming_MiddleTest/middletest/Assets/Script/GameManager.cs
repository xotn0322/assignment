using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region instance
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public float gameSpeed = 5;
    public bool isPlay = false;
    public bool isCor = false;
    public GameObject playBtn;

    public Text scoreText;
    public int maxScore = 0;
    public int score = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        maxScore = 0;
    }

    //게임오버
    public void GameOver()
    {
        isPlay = false;
        Debug.Log("닿음");

        maxScore = score;
        SceneManager.LoadScene("Start");
        StopCoroutine(AddScore());

        //난이도 초기화
        gameSpeed = 5;
        GameObject.Find("Player").GetComponent<Player>().jumpSpeed = 6;
        GameObject.Find("MobGenerator").GetComponent<MobSpawn>().minSpawn = 1;
        GameObject.Find("MobGenerator").GetComponent<MobSpawn>().maxSpawn = 3;
    }

    private void Update()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();

        //AddScore 코루틴이 안돌아갈 때를 위함
        if (isCor == true)
        {
            StartCoroutine(AddScore());
        }
    }

    //점수 추가
    public IEnumerator AddScore()
    {
        isCor = false;
        //0.1초당 1점씩 추가
        while (isPlay)
        {
            score++;
            scoreText.text = "Score : " + score.ToString();
            yield return new WaitForSeconds(0.1f);

            //난이도 조절
            gameSpeed += 0.005f;
            GameObject.Find("Player").GetComponent<Player>().jumpSpeed += 0.004f;
            GameObject.Find("MobGenerator").GetComponent<MobSpawn>().minSpawn -= 0.0007f;
            GameObject.Find("MobGenerator").GetComponent<MobSpawn>().maxSpawn -= 0.0007f;
        }
    }
}
