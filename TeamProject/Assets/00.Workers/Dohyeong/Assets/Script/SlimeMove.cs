using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public Rigidbody2D rigid;  // Rigidbody2D 컴포넌트를 담을 변수
    public Animator anim;        // Animator 컴포넌트를 담을 변수
    SpriteRenderer spriteRenderer;   // SpriteRenderer 컴포넌트를 담을 변수
    public int moveDir;                    // 이동 방향을 나타내는 변수
    public float nextThinkTime;          // 다음 AI 판단 시간을 나타내는 변수
    public SlimeController controller;  // SlimeController 스크립트를 참조하는 변수


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    // Rigidbody2D 컴포넌트를 가져와서 변수에 저장
        anim = GetComponent<Animator>();        // Animator 컴포넌트를 가져와서 변수에 저장
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer 컴포넌트를 가져와서 변수에 저장
        controller = FindObjectOfType<SlimeController>();    // SlimeController 스크립트를 찾아 변수에 저장
        StartCoroutine("monsterAI");                             // monsterAI 코루틴을 시작
    }


    void FixedUpdate()
    {
        if(!controller.isJump||!controller.isKnockback)
        {
            // 이동
            rigid.velocity = new Vector2(moveDir, rigid.velocity.y);
            if (rigid.velocity.x > 0.1f)
            {
                spriteRenderer.flipX = false;  // 오른쪽으로 이동 중인 경우 스프라이트를 반전하지 않음
            }
            else
            {
                spriteRenderer.flipX = true;  // 왼쪽으로 이동 중인 경우 스프라이트를 반전시킴
            }

            //지형 체크
            //몬스터는 앞을 체크해야 
            Vector2 frontVec = new Vector2(rigid.position.x + moveDir*0.5f, rigid.position.y - 0.5f); // 이동 방향의 앞을 체크하기 위한 레이캐스트의 시작 위치를 설정
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));  // 레이캐스트를 시각적으로 표현하기 위해 그려줌
            // 시작,방향 색깔

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // 레이캐스트를 시각적으로 표현하기 위해 그려줌

            if (rayHit.collider == null)
            {
                Debug.Log("경고! 이 앞 낭떨어지다!");
                Turn();  // 방향을 반대로 전환
            }


            if (controller.isKnockback || controller.isDamaged) 
            {
                moveDir = 0; // 공격 중이거나 넉백 중이라면 moveDir 값을 변경하지 않음
            }
        }
    }

    IEnumerator monsterAI()
    { 
        moveDir = Random.Range(-1, 2);  // -1, 0, 1 중에서 랜덤하게 이동 방향을 설정
        nextThinkTime = Random.Range(2f, 5f);  // 다음 판단 시간을 2~5 사이의 랜덤 값으로 설정
        yield return new WaitForSeconds(nextThinkTime);  // 일정 시간 동안 대기
        StartCoroutine("monsterAI");    // 몬스터 AI 코루틴 재실행
    }

    void Turn()
    {
        moveDir = moveDir * (-1);  // 이동 방향을 반전시킴
        CancelInvoke();              // Invoke를 취소
    }

    public void startMove()
    {
        StartCoroutine("monsterAI");  // monsterAI 코루틴을 시작
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI");  // monsterAI 코루틴을 정지
    }
}
