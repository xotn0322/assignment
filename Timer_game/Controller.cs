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
    public Slider slider; //�����̴� ȣ��
    public float speed = 300; //�����̴� �ӵ�
    public float r_speed; //�����̴��� ���� �ӵ���
    public float Maxpos;
    public float Minpos;
    public RectTransform pass; //���������� �ʷ� ������ ��ġ���������� ���� ȣ�� (anchor �� ������������)
    public int combo;
    public bool isSlide = false;
    Vector2 newPass; //pass.anchor ��ġ
    Vector2 p_size; //pass.sizeDelta ��
    public int m_score; //�ְ���

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
        if (Input.GetKeyDown(KeyCode.Space) && !isSlide) //update���� coroutine�� �۵���Ű�� setslide�� �۵��� ���� Ʈ����
        {
            isSlide = true;
            SetSlide();
        }

        //�������
        m_Score.GetComponent<Text>().text = "Max Score is : " + m_score;
        Score.GetComponent<Text>().text = "Score is : " + combo;
    }

    public void PlayAnimation(int slideNumber) //�ִϸ��̼� �۵�
    {
        anime.SetFloat("Blend", slideNumber); //Blend�� slideNumber�� �Է�
        anime.SetTrigger("Atk"); //Ʈ�����۵�
    }

    public void SetSlide() //�����̴� �ʱⰪ ����
    {
        slider.value = 0;
        Minpos = pass.anchoredPosition.x;
        Maxpos = pass.sizeDelta.x + Minpos;
        StartCoroutine(SlideBar()); //Coroutine : update�� ������������ �����Ӹ��� ��������� coroutine�� ���Ҷ��� �ݺ��� �� �ִ�.
    }

    IEnumerator SlideBar() //Coroutine�Ҷ� ����� �ڵ� / �������� update��� �����ϸ� ����
    { 
        yield return null; //�ʼ� ���� �����ӱ��� ���
        while (!(Input.GetKeyDown(KeyCode.Space) || slider.value == slider.maxValue)) //update���� setslide�� �۵���Ű�� ���� slidebar�� �۵��Ǹ� while�� ������ ���� ������ coroutine�� �۵���
        {
            slider.value += Time.deltaTime * speed;
            yield return null; //�ʼ�
        }

        if (slider.value >= Minpos && slider.value <= Maxpos) //�����ڽ��� ��ġ�� ��
        {
            //�ʱ�ȭ
            speed = 300;
            anime.speed = 1;
            //���Ǿ��� �۵�
            PlayAnimation(slideNumber++);
            SetSlide(); //��ƾ�ݺ�
            r_speed = Random.Range(1.0f, 1.7f);
            speed *= r_speed;
            anime.speed *= (r_speed + 0.5f);
            combo++;
            
            if (slideNumber > 2) //�ִϸ��̼� �ʱ�ȭ
            {
                slideNumber = 0;
            }

            if (combo % 10 == 0) //���������� �ʷϹڽ��� ũ�⸦ �������ν� ���̵� ��� �� ���Ӽ� �߰�
            {
                newPass.x *= 1.04f;
                pass.anchoredPosition = newPass; //�ʷϹڽ��� ������ġ����
                p_size.x -= 8;
                pass.sizeDelta = p_size; //�ʷϹڽ��� ����ũ������
            }
        }
        else
        {
            if (m_score < combo) //�ְ����� ����
                m_score = combo;
            //�ʱ�ȭ
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
