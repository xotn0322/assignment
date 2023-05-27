using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    // ȭ�鿡 ǥ�õǴ� LoadingBar ���ҽ� ����
    public Slider sliderUI;
    public static string NextSceneName;

    // ���� �ð��� ���� Min Max ��
    float dummyTimeRange_Min;
    float dummyTimeRange_Max;

    // ���� �������� �����Ǵ� ���� �ð� -> ���� �� �ε� ����
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
        // ���� Ÿ�̸ӷ� ������ ���� ����
        dummyTime = Random.Range(dummyTimeRange_Min, dummyTimeRange_Max);

        float loadingTime = 0.0f;   // �ð� ���
        float progress = 0.0f;      // ������


        // Ÿ�̸� ������ ó�� 
        while (loadingTime <= dummyTime)
        {
            loadingTime += Time.deltaTime;

            // AsyncOperation �� ���� �߰� �ε� ó���� ���� 0.9 �� �� �����ȭ 
            // ���� opertaion.progress �� 0.9 ��ġ ó�� �ǰ� �Ϸ� �ȴ�.
            progress = Mathf.Clamp01(loadingTime / (0.9f + dummyTime));

            // �����̴����� �� ���� ó��
            sliderUI.value = progress;

            yield return null;
        }

        // "AsyncOperation"��� "�񵿱����� ������ ���� �ڷ�ƾ�� ����"
        AsyncOperation operation = SceneManager.LoadSceneAsync(NextSceneName);
        operation.allowSceneActivation = false;

        // �ε��� ����Ǳ� �������� �ε�â ������ ó��
        while (!operation.isDone)
        {
            // �񵿱� �ε� ���࿡ ���� ������ ó��
            progress = Mathf.Clamp01((operation.progress + loadingTime) / (0.9f + dummyTime));

            // �����̴� ���� ó��
            sliderUI.value = progress;

            if (progress == 1.0f)
                operation.allowSceneActivation = true;

            yield return null;

        }

    }
}
