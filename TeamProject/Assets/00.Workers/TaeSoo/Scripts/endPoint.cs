using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class endPoint : MonoBehaviour
{
    //체크포인트 이펙트 및 아이콘 애니메이션
    public GameObject checkPointIcon;
    public GameObject checkPointEffect;

    public Controller pController;
    public AudioClip turnOn; //활성화 사운드
    public string collideTag = "Player";
    public int stage;

    // Start is called before the first frame update
    void Start()
    {
        pController = GameObject.FindWithTag("Player").GetComponent<Controller>();

        //씬 전환시 스폰위치와 시작지점 정하기
        pController.spawnPoint = new Vector2(0, 1);
        pController.transform.position = new Vector2(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //실시간으로 스테이지 확인
        stage = GameObject.Find("gameManeger").GetComponent<gameManeger>().stageNumber;
    }

    //플레이어가 목적지에 도착했을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == collideTag)
        {
            Debug.Log("도착");
            
            //이펙트
            checkPointEffect.SetActive(true);
            checkPointIcon.SetActive(true);

            //사운드
            Sound.instance.SFXPlay("End", turnOn);

            Invoke("goStage", 3f);
        }
    }

    //스테이지 이동
    void goStage()
    {
        switch (stage)
        {
            case 1:
                LoadSceneManager.LoadScene("stage 2");
                break;

            case 2:
                LoadSceneManager.LoadScene("stage 3");
                break;

            case 3:
                LoadSceneManager.LoadScene("stage 4");
                break;

            case 4:
                LoadSceneManager.LoadScene("stage 5");
                break;

            case 5:
                LoadSceneManager.LoadScene("Ending");
                break;
        }
    }
}
