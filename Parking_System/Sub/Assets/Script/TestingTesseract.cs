using UnityEngine;
using UnityEngine.UI;

public class TestingTesseract : MonoBehaviour
{
    [SerializeField] private Texture2D imageToRecognize; //�νĽ�ų �̹����ؽ���
    [SerializeField] private Text displayText; //����� ����� UI
    [SerializeField] private RawImage outputImage;
    private TesseractDriver _tesseractDriver;
    public string _text = ""; //�����. ������ �����̺�
    private Texture2D _texture; //�ν��۾��� �ʿ��� �ؽ���

    //�ʱ�ȭ
    private void Start()
    {
        

    }

    //�������� �������� �� �ʱ�ȭ
    public void Call()
    {
        //�̹��� ��������
        imageToRecognize = GetComponent<InSensor>().License;

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
        //AddToTextDisplay(_tesseractDriver.CheckTessVersion()); //tesseract ����Ȯ��
        _tesseractDriver.Setup(OnSetupCompleteRecognize); //���� �� OnSetupCompleteRecognize�Լ� ȣ��
    }

    //�̹������� �ؽ�Ʈ ����
    private void OnSetupCompleteRecognize()
    {
        _text = _tesseractDriver.Recognize(_texture); //���ڸ� ������
        GetComponent<SheetManagement>().CarLicense = _text;
        GetComponent<SheetManagement>().InPost();
        //AddToTextDisplay(_tesseractDriver.Recognize(_texture)); //��ü (���⼭ ������ string���� ����� �ؽ�Ʈ)
        //AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
        //SetImageDisplay();
        //GameObject.Find("OutPut").GetComponent<Text>().text = _text;
    }

    private void ClearTextDisplay()
    {
        _text = "";
    }

    //��µ� �̹���(�ؽ�Ʈ) ���� (��ǻ� ���)
    private void AddToTextDisplay(string text, bool isError = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        _text += (string.IsNullOrWhiteSpace(displayText.text) ? "" : "\n") + text; //displayText�� ��µ� UI������Ʈ

        if (isError)
            Debug.LogError(text);
        else
            Debug.Log(text);
    }

    //������Ʈ
    /*private void LateUpdate()
    {
        displayText.text = _text;
    }*/

    //����� ȭ�� ����
    private void SetImageDisplay()
    {
        RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            rectTransform.rect.width * _tesseractDriver.GetHighlightedTexture().height / _tesseractDriver.GetHighlightedTexture().width);
        outputImage.texture = _tesseractDriver.GetHighlightedTexture();
    }
}