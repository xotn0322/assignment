using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkingSensor : MonoBehaviour
{
    public bool OnOff = false;
    public string Place;
    public Texture2D License;

    [SerializeField] private Texture2D imageToRecognize; //인식시킬 이미지텍스쳐
    private TesseractDriver _tesseractDriver;
    private string _ptext = ""; //결과값. 원래는 프라이빗
    private Texture2D _texture; //인식작업에 필요한 텍스쳐

    //주차시
    private void OnTriggerEnter2D(Collider2D collision)
    {
        License = collision.GetComponent<CarMove>().selectedLicense; //충돌한 오브젝트(차량)의 번호판가져오기
        Call();
        GetComponent<SheetManagement>().place = Place;
        GetComponent<SheetManagement>().PCheckLicense = _ptext; //원래는 _ptext
        GetComponent<SheetManagement>().PlacePost();
        OnOff = true;
    }

    //퇴장시
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject.Find("Gate_In").GetComponent<SheetManagement>().place = Place;
        GameObject.Find("Gate_In").GetComponent<SheetManagement>().PlaceOutPost();
        OnOff = false;
    }

    //센서에서 사진찍을 때 초기화
    public void Call()
    {
        //이미지 가져오기
        imageToRecognize = License;

        //이미지텍스쳐 픽셀화
        Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        texture.SetPixels32(imageToRecognize.GetPixels32());
        texture.Apply();

        _tesseractDriver = new TesseractDriver();
        Recoginze(texture);
    }

    //초기설정
    private void Recoginze(Texture2D outputTexture)
    {
        _texture = outputTexture;
        ClearTextDisplay();
        _tesseractDriver.Setup(OnSetupCompleteRecognize); //설정 후 OnSetupCompleteRecognize함수 호출
    }

    //이미지에서 텍스트 추출
    private void OnSetupCompleteRecognize()
    {
        _ptext = _tesseractDriver.Recognize(_texture); //문자만 빼내기
    }

    private void ClearTextDisplay()
    {
        _ptext = "";
    }
}
