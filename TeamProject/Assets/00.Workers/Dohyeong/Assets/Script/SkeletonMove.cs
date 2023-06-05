using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMove : MonoBehaviour
{
    public Rigidbody2D rigid; // 몬스터의 Rigidbody2D 컴포넌트에 대한 참조
    public Animator anim;    // 몬스터의 Animator 컴포넌트에 대한 참조
    SpriteRenderer spriteRenderer;  // 몬스터의 SpriteRenderer 컴포넌트에 대한 참조
    public int moveDir;               // 몬스터의 이동 방향 (-1: 왼쪽, 0: 정지, 1: 오른쪽)
    public float nextThinkTime;   // 다음 생각을 하는 시간 간격
    public SkeletonController controller;  // SkeletonController 스크립트에 대한 참조

    public GameObject skeletonAttack; // 스켈레톤 몬스터의 skeletonAttack 자식 오브젝트에 대한 참조


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 가져오기
        anim = GetComponent<Animator>();     // Animator 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();   // SpriteRenderer 컴포넌트 가져오기
        skeletonAttack = GameObject.Find("skeletonAttack");  // skeletonAttack 오브젝트 찾아 가져오기
        controller = FindObjectOfType<SkeletonController>();  // SkeletonController 스크립트 가져오기
        StartCoroutine("monsterAI");  // 몬스터 AI 코루틴 시작
    }


    void FixedUpdate()
    {
        if(!controller.isKnockback)  // 넉백 중이 아닌 경우에만 동작
        {
            // 이동
            rigid.velocity = new Vector2(moveDir, rigid.velocity.y);
            if (rigid.velocity.x > 0.1f)
            {
                spriteRenderer.flipX = false; // 오른쪽으로 이동하고 있을 때 스프라이트 반전하지 않음
            }
            else
            {
                spriteRenderer.flipX = true; // 왼쪽으로 이동하고 있을 때 스프라이트 반전
            }

            //지형 체크
            //몬스터는 앞을 체크해야
            Vector2 frontVec = new Vector2(rigid.position.x + moveDir*0.5f, rigid.position.y - 0.5f);  // 이동 방향의 앞을 체크하기 위한 레이캐스트의 시작 위치를 설정
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));            // 레이캐스트를 시각적으로 표현하기 위해 그려줌


            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // 레이캐스트를 시각적으로 표현하기 위해 그려줌

            if (rayHit.collider == null)
            {
                Debug.Log("경고! 이 앞 낭떨어지다!");
                Turn();   // 방향 전환
            }


            // skeletonAttack 자식 오브젝트 방향 설정
            if (moveDir > 0)
            {
                skeletonAttack.transform.localPosition = new Vector2(Mathf.Abs(skeletonAttack.transform.localPosition.x), skeletonAttack.transform.localPosition.y); // skeletonAttack의 로컬 포지션을 설정하여 방향 전환
            }
            else if (moveDir < 0)
            {
                skeletonAttack.transform.localPosition = new Vector2(-Mathf.Abs(skeletonAttack.transform.localPosition.x), skeletonAttack.transform.localPosition.y); // skeletonAttack의 로컬 포지션을 설정하여 방향 전환
            }

            if (controller.isAttack || controller.isKnockback||controller.isDamaged) // 공격 중이거나 넉백 중이거나 피격 상태인 경우 moveDir 값을 변경하지 않음
            {
                moveDir = 0;  // 정지
                anim.SetInteger("WalkSpeed", moveDir); // Animator에 이동 속도 값 전달
            }
        }
    }

    IEnumerator monsterAI()
    {
        if (!controller.isKnockback) // 넉백 중이 아닌 경우에만 moveDir 값을 변경
        {
            moveDir = Random.Range(-1, 2);  // -1, 0, 1 중에서 랜덤하게 이동 방향 설정
            anim.SetInteger("WalkSpeed", moveDir);  // Animator에 이동 속도 값 전달
        }
        nextThinkTime = Random.Range(2f, 5f);   // 다음 생각을 하는 시간 간격을 2초에서 5초 사이로 랜덤하게 설정
        yield return new WaitForSeconds(nextThinkTime);
        StartCoroutine("monsterAI");  // 몬스터 AI 코루틴을 재귀적으로 호출
    }

    void Turn()
    {
        moveDir = moveDir * (-1); // 이동 방향 반전
        CancelInvoke();  // 이전 Invoke 취소
    }

    public void startMove()
    {
        StartCoroutine("monsterAI"); // 몬스터 AI 코루틴 시작
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI"); // 몬스터 AI 코루틴 정지
    }
}
