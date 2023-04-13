using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    Animator anime;
    GameObject Score;
    GameObject m_Score;
    public int slideNumber;
    public Slider slider; //슬라이더 호출
    public float speed = 300; //슬라이더 속도
    public float r_speed; //슬라이더의 랜덤 속도값
    public float Maxpos;
    public float Minpos;
    public RectTransform pass; //판정범위인 초록 상자의 위치값가져오기 위한 호출 (anchor 값 가져오기위함)
    public int combo;
    public bool isSlide = false;
    Vector2 newPass; //pass.anchor 위치
    Vector2 p_size; //pass.sizeDelta 값
    public int m_score; //최고점

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        Score = GameObject.Find("Score");
        m_Score = GameObject.Find("m_Score");
        newPass = pass.anchoredPosition;
        p_size = pass.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSlide) //update에서 coroutine을 작동시키는 setslide의 작동을 위한 트리거
        {
            isSlide = true;
            SetSlide();
        }

        //점수출력
        m_Score.GetComponent<Text>().text = "Max Score is : " + m_score;
        Score.GetComponent<Text>().text = "Score is : " + combo;
    }

    public void PlayAnimation(int slideNumber) //애니메이션 작동
    {
        anime.SetFloat("Blend", slideNumber); //Blend에 slideNumber값 입력
        anime.SetTrigger("Atk"); //트리거작동
    }

    public void SetSlide() //슬라이더 초기값 설정
    {
        slider.value = 0;
        Minpos = pass.anchoredPosition.x;
        Maxpos = pass.sizeDelta.x + Minpos;
        StartCoroutine(SlideBar()); //Coroutine : update는 무조건적으로 프레임마다 실행되지만 coroutine은 원할때만 반복할 수 있다.
    }

    IEnumerator SlideBar() //Coroutine할때 실행될 코드 / 독자적인 update라고 생각하면 편함
    { 
        yield return null; //필수 다음 프레임까지 대기
        while (!(Input.GetKeyDown(KeyCode.Space) || slider.value == slider.maxValue)) //update에서 setslide를 작동시키는 순간 slidebar가 작동되며 while의 조건이 끝날 때까지 coroutine이 작동함
        {
            slider.value += Time.deltaTime * speed;
            yield return null; //필수
        }

        if (slider.value >= Minpos && slider.value <= Maxpos) //판정박스에 위치할 때
        {
            //초기화
            speed = 300;
            anime.speed = 1;
            //조건없이 작동
            PlayAnimation(slideNumber++);
            SetSlide(); //루틴반복
            r_speed = Random.Range(1.0f, 1.7f);
            speed *= r_speed;
            anime.speed *= (r_speed + 0.5f);
            combo++;
            
            if (slideNumber > 2) //애니메이션 초기화
            {
                slideNumber = 0;
            }

            if (combo % 10 == 0) //판정구간인 초록박스의 크기를 줄읾으로써 난이도 상승 및 게임성 추가
            {
                newPass.x *= 1.04f;
                pass.anchoredPosition = newPass; //초록박스의 가로위치조절
                p_size.x -= 8;
                pass.sizeDelta = p_size; //초록박스의 가로크기조절
            }
        }
        else
        {
            if (m_score < combo) //최고점수 갱신
                m_score = combo;
            //초기화
            combo = 0;
            isSlide = false;
            slideNumber = 0;
            newPass.x = 110;
            pass.anchoredPosition = newPass;
            p_size.x = 80;
            pass.sizeDelta = p_size;
        }
        slider.value = 0;
    }
}
