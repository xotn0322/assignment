using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InSensor : MonoBehaviour
{
    GameObject Car;
    public Texture2D License;

    private void Start()
    {
        //License = GetComponent<Texture2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Car"))
        {
            License = collision.GetComponent<CarMove>().selectedLicense; //�浹�� ������Ʈ(����)�� ��ȣ�ǰ�������
            GetComponent<TestingTesseract>().Call();
            Debug.Log("������!");
        }
    }
}
