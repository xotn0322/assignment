using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class gameManeger : MonoBehaviour
{
    public int stageNumber;

    // Start is called before the first frame update
    void Start()
    {
        stageNumber = SceneManager.GetActiveScene().buildIndex;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Scene index체크해서 씬전환에 사용
        stageNumber = SceneManager.GetActiveScene().buildIndex;
    }
}
