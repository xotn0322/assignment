using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMove : MonoBehaviour
{
    public Rigidbody2D rigid;  // Rigidbody2D 컴포넌트를 저장하기 위한 변수
    public Animator anim;     // Animator 컴포넌트를 저장하기 위한 변수
    SpriteRenderer spriteRenderer;  // SpriteRenderer 컴포넌트를 저장하기 위한 변수
    public int moveDir;                     // 이동 방향을 결정하기 위한 변수
    public float nextThinkTime;        // 다음 판단 시간을 저장하기 위한 변수
    public MushroomController controller; // MushroomController 스크립트를 저장하기 위한 변수

    public GameObject mushroomAttack;  // mushroomAttack 게임 오브젝트를 저장하기 위한 변수
    public GameObject ShootPoison;  // ShootPoison 게임 오브젝트를 저장하기 위한 변수


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트를 가져와서 변수에 저장
        anim = GetComponent<Animator>();      // Animator 컴포넌트를 가져와서 변수에 저장
        spriteRenderer = GetComponent<SpriteRenderer>();    //SpriteRenderer 컴포넌트를 가져와서 변수에 저장
        mushroomAttack = GameObject.Find("mushroomAttack");  // "mushroomAttack" 오브젝트를 찾아서 변수에 저장
        ShootPoison = GameObject.Find("ShootPoison");       // "ShootPoison" 오브젝트를 찾아서 변수에 저장
        controller = FindObjectOfType<MushroomController>(); // MushroomController 스크립트를 찾아서 변수에 저장
        StartCoroutine("monsterAI");                                       // monsterAI 코루틴을 시작
    }


    void FixedUpdate()
    {
        if(!controller.isKnockback)   // 넉백 중이 아닌 경우에만 실행
        {
            //Move
            rigid.velocity = new Vector2(moveDir, rigid.velocity.y);    // Rigidbody2D의 속도를 설정하여 이동
            if (rigid.velocity.x > 0.1f)
            {
                spriteRenderer.flipX = false;    // 속도가 양수(오른쪽 이동)일 때 스프라이트를 좌우 반전하지 않음
            }
            else
            {
                spriteRenderer.flipX = true;    // 속도가 음수(왼쪽 이동)일 때 스프라이트를 좌우 반전
            }

            //지형 체크
            //몬스터는 앞을 체크해야 
            Vector2 frontVec = new Vector2(rigid.position.x + moveDir*0.5f, rigid.position.y - 0.5f);   // 이동 방향의 앞을 체크하기 위한 레이캐스트의 시작 위치를 설정
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));           // 레이캐스트를 시각적으로 표현하기 위해 그려줌

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground")); // 레이캐스트를 시각적으로 표현하기 위해 그려줌

            if (rayHit.collider == null)
            {
                Debug.Log("경고! 이 앞 낭떨어지다!");   
                Turn();                                          // 방향을 반대로 전환
            }

            if (moveDir > 0)
            {
                mushroomAttack.transform.localPosition = new Vector2(Mathf.Abs(mushroomAttack.transform.localPosition.x), mushroomAttack.transform.localPosition.y);  // mushroomAttack의 로컬 포지션을 설정하여 방향 전환
                ShootPoison.transform.localPosition = new Vector2(Mathf.Abs(ShootPoison.transform.localPosition.x), ShootPoison.transform.localPosition.y);  // ShootPoison의 로컬 포지션을 설정하여 방향 전환
            }
            else if (moveDir < 0)
            {
                mushroomAttack.transform.localPosition = new Vector2(-Mathf.Abs(mushroomAttack.transform.localPosition.x), mushroomAttack.transform.localPosition.y);  // mushroomAttack의 로컬 포지션을 설정하여 방향 전환
                ShootPoison.transform.localPosition = new Vector2(-Mathf.Abs(ShootPoison.transform.localPosition.x), ShootPoison.transform.localPosition.y);                     // ShootPoison의 로컬 포지션을 설정하여 방향 전환
            }

            if (controller.isAttack || controller.isKnockback || controller.isDamaged) // 공격 중이거나 넉백 중이거나 피해를 받고 있는 경우
            {
                moveDir = 0;                                     // 이동 방향을 0으로 설정하여 멈춤
                anim.SetInteger("WalkSpeed", moveDir);   // Animator 컴포넌트의 WalkSpeed 파라미터를 변경하여 애니메이션 재생을 정지시킴
            }
        }
    }

    IEnumerator monsterAI()
    {
        if (!controller.isKnockback) // 넉백 중이 아닌 경우에만 실행
        {
            moveDir = Random.Range(-1, 2);   // -1, 0, 1 중에서 랜덤하게 이동 방향 설정
            anim.SetInteger("WalkSpeed", moveDir);   // Animator 컴포넌트의 WalkSpeed 파라미터를 변경하여 이동 애니메이션 재생
        }
        nextThinkTime = Random.Range(2f, 5f);     // 다음 판단 시간을 랜덤하게 설정
        yield return new WaitForSeconds(nextThinkTime);  // 일정 시간 동안 대기
        StartCoroutine("monsterAI");     // monsterAI 코루틴 재실행
    }

    void Turn()
    {
        moveDir = moveDir * (-1);   // 이동 방향을 반대로 변경
        CancelInvoke();                // 이전에 예약된 Invoke를 취소
    }

    public void startMove()
    {
        StartCoroutine("monsterAI");   // monsterAI 코루틴을 시작
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI");   // monsterAI 코루틴을 정지
    }
}
