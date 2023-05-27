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
        stage = GameObject.Find("gameManeger").GetComponent<gameManeger>().stageNumber;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == collideTag)
        {
            Debug.Log("����");
            //������ ������ġ�� �������� ���ϱ�
            pController.spawnPoint = new Vector2(0, 1);
            pController.transform.position = new Vector2(0, 1);

            //����Ʈ
            checkPointEffect.SetActive(true);
            checkPointIcon.SetActive(true);

            //�������� �̵�
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
            //GameObject.Find("gameManeger").GetComponent<gameManeger>().stageNumber += 1;
        }
    }
}
