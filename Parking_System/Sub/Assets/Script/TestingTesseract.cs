using UnityEngine;
using UnityEngine.UI;

public class TestingTesseract : MonoBehaviour
{
    [SerializeField] private Texture2D imageToRecognize; //인식시킬 이미지텍스쳐
    [SerializeField] private Text displayText; //결과를 출력할 UI
    [SerializeField] private RawImage outputImage;
    private TesseractDriver _tesseractDriver;
    public string _text = ""; //결과값. 원래는 프라이빗
    private Texture2D _texture; //인식작업에 필요한 텍스쳐

    //초기화
    private void Start()
    {
        

    }

    //센서에서 사진찍을 때 초기화
    public void Call()
    {
        //이미지 가져오기
        imageToRecognize = GetComponent<InSensor>().License;

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
        //AddToTextDisplay(_tesseractDriver.CheckTessVersion()); //tesseract 버전확인
        _tesseractDriver.Setup(OnSetupCompleteRecognize); //설정 후 OnSetupCompleteRecognize함수 호출
    }

    //이미지에서 텍스트 추출
    private void OnSetupCompleteRecognize()
    {
        _text = _tesseractDriver.Recognize(_texture); //문자만 빼내기
        GetComponent<SheetManagement>().CarLicense = _text;
        GetComponent<SheetManagement>().InPost();
        //AddToTextDisplay(_tesseractDriver.Recognize(_texture)); //본체 (여기서 나오는 string값이 추출된 텍스트)
        //AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
        //SetImageDisplay();
        //GameObject.Find("OutPut").GetComponent<Text>().text = _text;
    }

    private void ClearTextDisplay()
    {
        _text = "";
    }

    //출력될 이미지(텍스트) 설정 (사실상 출력)
    private void AddToTextDisplay(string text, bool isError = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        _text += (string.IsNullOrWhiteSpace(displayText.text) ? "" : "\n") + text; //displayText가 출력될 UI오브젝트

        if (isError)
            Debug.LogError(text);
        else
            Debug.Log(text);
    }

    //업데이트
    /*private void LateUpdate()
    {
        displayText.text = _text;
    }*/

    //출력할 화면 세팅
    private void SetImageDisplay()
    {
        RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            rectTransform.rect.width * _tesseractDriver.GetHighlightedTexture().height / _tesseractDriver.GetHighlightedTexture().width);
        outputImage.texture = _tesseractDriver.GetHighlightedTexture();
    }
}