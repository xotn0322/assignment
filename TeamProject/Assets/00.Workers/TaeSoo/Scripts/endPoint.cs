using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class endPoint : MonoBehaviour
{
    public GameObject checkPointIcon;
    public GameObject checkPointEffect;
    public Controller pController;
    public string collideTag = "Player";
    public int stage;

    // Start is called before the first frame update
    void Start()
    {
        pController = GameObject.FindWithTag("Player").GetComponent<Controller>();

        //�� ��ȯ�� ������ġ�� �������� ���ϱ�
        pController.spawnPoint = new Vector2(0, 1);
        pController.transform.position = new Vector2(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //�ǽð����� �������� Ȯ��
        stage = GameObject.Find("gameManeger").GetComponent<gameManeger>().stageNumber;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == collideTag)
        {
            Debug.Log("����");
            
            //����Ʈ
            checkPointEffect.SetActive(true);
            checkPointIcon.SetActive(true);

            Invoke("goStage", 3f);
        }
    }

    //�������� �̵�
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
                LoadSceneManager.LoadScene("stage 6");
                break;
        }
    }
}
