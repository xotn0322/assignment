using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public int currentKillCount;
    public int countToReachKill;
    public Text m_text;
    void Start()
    {
        countToReachKill = transform.childCount;
        m_text.text = "Count : " + currentKillCount.ToString() +
            " / Goal : " + countToReachKill.ToString();
    }

    // ���� ��� �� ȣ��
    public void AddCount()
    {
        currentKillCount++;
        m_text.text = "Count " + currentKillCount.ToString() +
            " / Goal : " + countToReachKill.ToString();

        // ���� ������Ʈ ���� ó�� �� ����� ȣ��
        if (currentKillCount >= countToReachKill)
        {
            currentKillCount = 0;
            Invoke("EnableEndScene", 2f);
        }
    }

    private void EnableEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
