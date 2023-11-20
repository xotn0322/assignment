using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SheetManagement : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycbxLjM6MRPwHkQbY5eEki6OYGguLeS0DGA-9Pr7-nR47k8BYEDcJnugiiEQ-bBE-FJbb/exec"; //TestVersion_1.83f
    public string responseText; //데이터 요청시 처리결과

    //주차장 진입시
    public string CarLicense; //인식된 차량의 번호판 문자열
    public string result; //차량의 통과가능유무
    
    //주차센서 작동시
    public string place; //주차했을 때 차량의 위치
    public string PCheckLicense; //체크할 차량의 번호판 문자열. ParkingCheckLicense

    //주차장 출차시
    public string OCheckLicense; //체크할 차량의 번호판 문자열. OutCheckLicense


    //스프레드시트에 보낼 박스생성(입차시)
    public void InPost()
    {
        //패키징
        WWWForm form = new WWWForm();
        form.AddField("send", "License"); //License를 사용하라는 의미를 send에(License는 시트코드에서 스위치의 case중 하나이다.)
        form.AddField("License", CarLicense); //차량의 번호판 문자열을 License에
        form.AddField("Intime", DateTime.Now.ToString()); //현재 시간을 Intime에

        StartCoroutine(Post(form));
    }

    //스프레드시트에 보낼 박스생성(주차센서작동시ON)
    public void PlacePost()
    {
        Debug.Log("주차센서 ON" + place);
        WWWForm form = new WWWForm();
        form.AddField("send", "Place");
        form.AddField("Place", place);
        form.AddField("License", PCheckLicense);

        StartCoroutine(Post(form));
    }

    //스프레드시트에 보낼 박스생성(주차센서작동시OFF)
    public void PlaceOutPost()
    {
        Debug.Log("주차센서 OFF" + place);
        WWWForm form = new WWWForm();
        form.AddField("send", "PlaceOut");
        form.AddField("Place", place);

        StartCoroutine(Post(form));
    }

    //스프레드시트에 보낼 박스생성(출차시)
    public void OutPost()
    {
        WWWForm form = new WWWForm();
        form.AddField("send", "Out");
        form.AddField("outcheck", OCheckLicense);
        form.AddField("OutTime", DateTime.Now.ToString()); //출차시간
    }

    //보내기
    private IEnumerator Post(WWWForm form)
    {
        //URL에 form을 담은 www가 만들어졌고, SendWebRequest를 통해 스프레드시트코드의 doPost()로 메시지를 전송한다.
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            //결과
            if (www.isDone)
            {
                print(www.downloadHandler.text);

                //json데이터값에서 데이터 추출
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

