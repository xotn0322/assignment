using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossZone : MonoBehaviour
{
    public GameObject LWall;
    public GameObject RWall;
    public GameObject Boss;
    public GameObject Gene;
    public bool bossKilled = false;

    // Start is called before the first frame update
    void Start()
    {
        LWall = GameObject.Find("LWall");
        RWall = GameObject.Find("RWall");
        Boss = GameObject.Find("Boss");
        Gene = GameObject.Find("Generator");
        GateOpen();
    }

    //������ ���� Ȱ��ȭ
    public void GateClose()
    {
        LWall.SetActive(true);
        RWall.SetActive(true);
        Boss.SetActive(true);
        Gene.SetActive(true);
    }

    //������ ���� ��Ȱ��ȭ
    public void GateOpen()
    {
        LWall.SetActive(false);
        RWall.SetActive(false);
        Boss.SetActive(false);
        Gene.SetActive(false);
    }

    //������ ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !bossKilled)
        {
            GateClose();
        }
    }
}
