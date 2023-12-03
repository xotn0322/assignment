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

    //���ӿ���
    public void GameOver()
    {
        isPlay = false;
        Debug.Log("����");

        maxScore = score;
        SceneManager.LoadScene("Start");
        StopCoroutine(AddScore());

        //���̵� �ʱ�ȭ
        gameSpeed = 5;
        GameObject.Find("Player").GetComponent<Player>().jumpSpeed = 6;
        GameObject.Find("MobGenerator").GetComponent<MobSpawn>().minSpawn = 1;
        GameObject.Find("MobGenerator").GetComponent<MobSpawn>().maxSpawn = 3;
    }

    private void Update()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();

        //AddScore �ڷ�ƾ�� �ȵ��ư� ���� ����
        if (isCor == true)
        {
            StartCoroutine(AddScore());
        }
    }

    //���� �߰�
    public IEnumerator AddScore()
    {
        isCor = false;
        //0.1�ʴ� 1���� �߰�
        while (isPlay)
        {
            score++;
            scoreText.text = "Score : " + score.ToString();
            yield return new WaitForSeconds(0.1f);

            //���̵� ����
            gameSpeed += 0.005f;
            GameObject.Find("Player").GetComponent<Player>().jumpSpeed += 0.004f;
            GameObject.Find("MobGenerator").GetComponent<MobSpawn>().minSpawn -= 0.0007f;
            GameObject.Find("MobGenerator").GetComponent<MobSpawn>().maxSpawn -= 0.0007f;
        }
    }
}
