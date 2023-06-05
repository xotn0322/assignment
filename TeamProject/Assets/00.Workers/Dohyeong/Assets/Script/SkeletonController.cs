using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    Animator anim;                            // 몬스터의 애니메이터 컴포넌트
    Rigidbody2D rigid2D;                  // 몬스터의 리지드바디2D 컴포넌트
    public float maxHealth = 30f;         // 몬스터의 최대 체력
    private float currentHealth;          // 몬스터의 현재 체력
    public Controller player;            // 플레이어의 컨트롤러 스크립트
    public SkeletonMove move;       // 몬스터의 이동 스크립트
    public Transform pos;                // 공격 범위를 위한 박스의 위치
    public Vector2 boxSize;               // 공격 범위를 위한 박스의 크기
    public BoxCollider2D box;            // 공격 범위를 위한 박스 콜라이더

    private Transform playerTransform;   // 플레이어의 위치를 저장하기 위한 변수
    private Transform skeletonTransform;  // 몬스터의 위치를 저장하기 위한 변수



    public bool isAttack = false;             // 현재 공격 중인지 여부를 나타내는 변수
    public bool isKnockback = false;     // 현재 넉백 상태인지 여부를 나타내는 변수
    public bool isDamaged = false;     // 현재 피해를 입고 있는지 여부를 나타내는 변수

    public float attackCooldown = 0.001f; // 공격 쿨다운 시간
    private float currentCooldown = 0f; // 현재 쿨다운 시간

    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트
    
    Color halfA = new Color(1, 1, 1, 0.5f);  // 스프라이트 알파 값을 반투명으로 설정하기 위한 컬러
    Color fullA = new Color(1, 1, 1, 1);      // 스프라이트 알파 값을 완전히 불투명으로 설정하기 위한 컬러

    public void Start()
    {
        anim = GetComponent<Animator>();        // 몬스터의 애니메이터 컴포넌트 가져오기
        rigid2D = GetComponent<Rigidbody2D>();   // 몬스터의 리지드바디2D 컴포넌트 가져오기
        currentHealth = maxHealth;    // 몬스터의 체력을 최대 체력으로 초기화
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트 가져오기
        player = FindObjectOfType<Controller>();            // Controller 타입의 스크립트 찾기
        move = FindObjectOfType<SkeletonMove>();   // SkeletonMove 타입의 스크립트 찾기
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
        skeletonTransform = transform;    // 몬스터의 Transform 컴포넌트 가져오기
    }


    public void Update()
    {
        if (!isAttack&& !isKnockback)
        {
            if (currentCooldown <= 0)
            {
                AttackPlayer();            // 플레이어를 공격하는 메서드 호출
                currentCooldown = attackCooldown;   // 공격 쿨다운 초기화
            }
            else if (currentCooldown > 0 && !anim.GetBool("Attack"))
            {
                currentCooldown -= Time.deltaTime;  // 공격 쿨다운 감소
            }
        }
        else
        {
            // 공격 중일 때 FlipX를 고정하지 않도록 설정
            spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);
        }
    }

    public void AttackPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);  // pos 위치를 기준으로 boxSize 크기의 박스 콜라이더와 겹치는 모든 콜라이더를 가져옴
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag =="Player")
            {
                move.moveDir = 0;  // 이동 방향을 멈춤
                anim.SetInteger("WalkSpeed", move.moveDir);  // WalkSpeed 애니메이션 파라미터 설정
                anim.SetBool("Attack",true);     // 공격 애니메이션 재생
                isAttack = true; // 공격 중 상태로 변경
                break;
            }
        }
    }

    public void FinishAttackAnimation()
    {
        anim.SetBool("Attack", false); // 공격 애니메이션 해제
        isAttack = false; // 공격 종료 후 상태 변경  
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);  // FlipX 설정
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);  // 공격 범위 상자를 기준으로 boxSize 크기의 노란색 와이어 큐브를 그림
    }

    // BoxCollider2D를 활성화하는 메서드
    public void EnableBoxCollider()
    {
        box.enabled = true;
    }

    // BoxCollider2D를 비활성화하는 메서드
    public void DisableBoxCollider()
    {
        box.enabled = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("해골과 충돌");
            playerTransform.GetComponent<Controller>().Damaged(3f, skeletonTransform.position); // 플레이어에게 데미지를 입히는 메서드 호출
        }
    }

    public void TakeDamage(float damageAmount,Vector2 playerpos)
    {
        if(!isDamaged)
        {
            isDamaged = true;
            currentHealth -= damageAmount;   // 몬스터 체력에서 피해량을 감소시킴
            
            if (currentHealth <= 0)
            {
                Die();    // 몬스터의 체력이 0 이하면 죽음 처리
            }
            else
            {
                anim.SetTrigger("Hurt");  // 피격 애니메이션 재생
                // 플레이어의 위치에 따라 넉백 방향 결정
                float knockbackDirection = transform.position.x - playerpos.x;
                if (knockbackDirection < 0)
                    knockbackDirection = -1f;
                else
                    knockbackDirection = 1f;


                    StartCoroutine(KnockBack(knockbackDirection));  // 넉백 코루틴 실행
                    StartCoroutine(damagedRoutine());  // 피격 상태 해제 코루틴 실행
                    StartCoroutine(alphaBlink());      // 피격 효과를 위한 깜빡임 코루틴 실행
            }
        }
    }

    private void Die()
    {
        // 몬스터가 죽을 때의 동작을 구현
        anim.SetBool("Death", true);
    }

    public void removeskeleton()
    {
        Destroy(gameObject);   // 몬스터 오브젝트 파괴
    }

    IEnumerator KnockBack(float dir)
    {
        isKnockback = true;
        float elapsedTime = 0;
        while (elapsedTime < 0.2f) //넉백시간
        {
            if (rigid2D.velocity.y == 0)
                rigid2D.velocity = new Vector2(3f * dir, 0);
            else
                rigid2D.velocity = new Vector2(3f * dir, 3f * dir);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
    }

    IEnumerator damagedRoutine()
    {
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }

    IEnumerator alphaBlink()
    {
        while (isDamaged)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = halfA;  // 반투명 상태로 변경
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = fullA;  // 완전한 상태로 변경
        }
    }
}
