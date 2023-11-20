using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SheetManagement : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycbxLjM6MRPwHkQbY5eEki6OYGguLeS0DGA-9Pr7-nR47k8BYEDcJnugiiEQ-bBE-FJbb/exec"; //TestVersion_1.83f
    public string responseText; //������ ��û�� ó�����

    //������ ���Խ�
    public string CarLicense; //�νĵ� ������ ��ȣ�� ���ڿ�
    public string result; //������ �����������
    
    //�������� �۵���
    public string place; //�������� �� ������ ��ġ
    public string PCheckLicense; //üũ�� ������ ��ȣ�� ���ڿ�. ParkingCheckLicense

    //������ ������
    public string OCheckLicense; //üũ�� ������ ��ȣ�� ���ڿ�. OutCheckLicense


    //���������Ʈ�� ���� �ڽ�����(������)
    public void InPost()
    {
        //��Ű¡
        WWWForm form = new WWWForm();
        form.AddField("send", "License"); //License�� ����϶�� �ǹ̸� send��(License�� ��Ʈ�ڵ忡�� ����ġ�� case�� �ϳ��̴�.)
        form.AddField("License", CarLicense); //������ ��ȣ�� ���ڿ��� License��
        form.AddField("Intime", DateTime.Now.ToString()); //���� �ð��� Intime��

        StartCoroutine(Post(form));
    }

    //���������Ʈ�� ���� �ڽ�����(���������۵���ON)
    public void PlacePost()
    {
        Debug.Log("�������� ON" + place);
        WWWForm form = new WWWForm();
        form.AddField("send", "Place");
        form.AddField("Place", place);
        form.AddField("License", PCheckLicense);

        StartCoroutine(Post(form));
    }

    //���������Ʈ�� ���� �ڽ�����(���������۵���OFF)
    public void PlaceOutPost()
    {
        Debug.Log("�������� OFF" + place);
        WWWForm form = new WWWForm();
        form.AddField("send", "PlaceOut");
        form.AddField("Place", place);

        StartCoroutine(Post(form));
    }

    //���������Ʈ�� ���� �ڽ�����(������)
    public void OutPost()
    {
        WWWForm form = new WWWForm();
        form.AddField("send", "Out");
        form.AddField("outcheck", OCheckLicense);
        form.AddField("OutTime", DateTime.Now.ToString()); //�����ð�
    }

    //������
    private IEnumerator Post(WWWForm form)
    {
        //URL�� form�� ���� www�� ���������, SendWebRequest�� ���� ���������Ʈ�ڵ��� doPost()�� �޽����� �����Ѵ�.
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            //���
            if (www.isDone)
            {
                print(www.downloadHandler.text);

                //json�����Ͱ����� ������ ����
                responseText = www.downloadHandler.text;
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseText);
                result = responseData.result;
                Debug.Log(result);
            }
            else
                print("Error");
        }
    }
}

public class ResponseData
{
    public string license;
    public string result;
    public string msg;
}

