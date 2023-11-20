using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkingSensor : MonoBehaviour
{
    public bool OnOff = false;
    public string Place;
    public Texture2D License;

    [SerializeField] private Texture2D imageToRecognize; //�νĽ�ų �̹����ؽ���
    private TesseractDriver _tesseractDriver;
    private string _ptext = ""; //�����. ������ �����̺�
    private Texture2D _texture; //�ν��۾��� �ʿ��� �ؽ���

    //������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        License = collision.GetComponent<CarMove>().selectedLicense; //�浹�� ������Ʈ(����)�� ��ȣ�ǰ�������
        Call();
        GetComponent<SheetManagement>().place = Place;
        GetComponent<SheetManagement>().PCheckLicense = _ptext; //������ _ptext
        GetComponent<SheetManagement>().PlacePost();
        OnOff = true;
    }

    //�����
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject.Find("Gate_In").GetComponent<SheetManagement>().place = Place;
        GameObject.Find("Gate_In").GetComponent<SheetManagement>().PlaceOutPost();
        OnOff = false;
    }

    //�������� �������� �� �ʱ�ȭ
    public void Call()
    {
        //�̹��� ��������
        imageToRecognize = License;

        //�̹����ؽ��� �ȼ�ȭ
        Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        texture.SetPixels32(imageToRecognize.GetPixels32());
        texture.Apply();

        _tesseractDriver = new TesseractDriver();
        Recoginze(texture);
    }

    //�ʱ⼳��
    private void Recoginze(Texture2D outputTexture)
    {
        _texture = outputTexture;
        ClearTextDisplay();
        _tesseractDriver.Setup(OnSetupCompleteRecognize); //���� �� OnSetupCompleteRecognize�Լ� ȣ��
    }

    //�̹������� �ؽ�Ʈ ����
    private void OnSetupCompleteRecognize()
    {
        _ptext = _tesseractDriver.Recognize(_texture); //���ڸ� ������
    }

    private void ClearTextDisplay()
    {
        _ptext = "";
    }
}
