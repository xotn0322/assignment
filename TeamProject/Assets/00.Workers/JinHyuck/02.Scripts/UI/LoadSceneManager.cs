using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    // 화면에 표시되는 LoadingBar 리소스 연결
    public Slider sliderUI;
    public static string NextSceneName;

    // 더미 시간의 설정 Min Max 값
    float dummyTimeRange_Min;
    float dummyTimeRange_Max;

    // 최초 게이지가 증가되는 더미 시간 -> 이후 본 로딩 시작
    float dummyTime;

    void Awake()
    {
        dummyTimeRange_Min = 0.8f;
        dummyTimeRange_Max = 1.5f;
        dummyTime = 0;
    }

    void Start()
    {
        StartCoroutine(LoadAsynchronously());
    }

    public static void LoadScene(string sceneName)
    {
        NextSceneName = sceneName;
        SceneManager.LoadScene("LoadScene");
    }

    IEnumerator LoadAsynchronously()
    {
        // 더미 타이머로 진행할 값을 설정
        dummyTime = Random.Range(dummyTimeRange_Min, dummyTimeRange_Max);

        float loadingTime = 0.0f;   // 시간 계산
        float progress = 0.0f;      // 게이지


        // 타이머 게이지 처리 
        while (loadingTime <= dummyTime)
        {
            loadingTime += Time.deltaTime;

            // AsyncOperation 를 통한 추가 로딩 처리를 위해 0.9 값 을 백분율화 
            // 이후 opertaion.progress 는 0.9 까치 처리 되고 완료 된다.
            progress = Mathf.Clamp01(loadingTime / (0.9f + dummyTime));

            // 슬라이더바의 값 증가 처리
            sliderUI.value = progress;

            yield return null;
        }

        // "AsyncOperation"라는 "비동기적인 연산을 위한 코루틴을 제공"
        AsyncOperation operation = SceneManager.LoadSceneAsync(NextSceneName);
        operation.allowSceneActivation = false;

        // 로딩이 종료되기 전까지의 로딩창 게이지 처리
        while (!operation.isDone)
        {
            // 비동기 로딩 진행에 따른 게이지 처리
            progress = Mathf.Clamp01((operation.progress + loadingTime) / (0.9f + dummyTime));

            // 슬라이더 증가 처리
            sliderUI.value = progress;

            if (progress == 1.0f)
                operation.allowSceneActivation = true;

            yield return null;

        }

    }
}
