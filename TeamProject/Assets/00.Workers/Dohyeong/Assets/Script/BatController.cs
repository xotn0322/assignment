using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    Animator anim;                          // 애니메이터 컴포넌트
    Rigidbody2D rigid2D;              // 리지드바디 컴포넌트
    public float maxHealth = 10f;         // 몬스터의 최대 체력
    private float currentHealth;          // 몬스터의 현재 체력
    public Controller player;              // 플레이어 스크립트
    public BatMove move;               // BatMove 스크립트
    public Transform pos;                // 공격범위 감지 위치
    public Vector2 boxSize;             // 공격범위 감지 박스 크기
    public BoxCollider2D box;         // 박스 콜라이더

    private Transform playerTransform;   // 플레이어의 위치를 저장하기 위한 변수
    private Transform batTransform;     // 몬스터의 위치를 저장하기 위한 변수



    public bool isAttack = false;  // 공격 중인지 여부를 나타내는 변수
    public bool isKnockback = false; // 넉백 중인지 여부를 나타내는 변수
    public bool isDamaged = false;  // 피격 상태인지 여부를 나타내는 변수
    public float attackCooldown = 0.001f; // 공격 쿨다운 시간
    private float currentCooldown = 0f; // 현재 쿨다운 시간

    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트

    Color halfA = new Color(1, 1, 1, 0.5f);  // 반투명한 색상
    Color fullA = new Color(1, 1, 1, 1);    // 완전한 색상



    public void Start()
    {
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        rigid2D = GetComponent<Rigidbody2D>(); // 리지드바디 컴포넌트 가져오기
        currentHealth = maxHealth;    // 몬스터의 체력을 최대 체력으로 초기화
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트 가져오기
        player = FindObjectOfType<Controller>(); // Controller 스크립트 가져오기
        move = FindObjectOfType<BatMove>();  // BatMove 스크립트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
        batTransform = transform; // 몬스터의 위치 저장
    }


    public void Update()
    {
        if (!isAttack&&!isKnockback)
        {
            if (currentCooldown <= 0)
            {
                AttackPlayer();   // 플레이어 공격 메서드 호출
                currentCooldown = attackCooldown; // 쿨다운 초기화
            }
            else if (currentCooldown > 0 && !anim.GetBool("Attack"))
            {
                currentCooldown -= Time.deltaTime;
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
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                anim.SetBool("Attack", true); // 애니메이션 상태 변경
                isAttack = true; // 공격 중 상태로 변경
                break;
            }
        }
    }

    public void FinishAttackAnimation()
    {
        anim.SetBool("Attack", false);  // 공격 애니메이션 종료
        isAttack = false; // 공격 종료 후 상태 변경
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize); // 공격 범위 콜라이더 씬에서 볼수 있도록  그리기
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
            Debug.Log("박쥐와 충돌");
            playerTransform.GetComponent<Controller>().Damaged(3f, batTransform.position); // 박쥐와 충돌하면 플레이어에게 데미지를 입히는 메서드 호출
        }
    }

    public void TakeDamage(float damageAmount, Vector2 playerpos)
    {
        if (!isDamaged)
        {
            isDamaged = true;
            currentHealth -= damageAmount;   // 몬스터 체력에서 피해량을 감소시킴

            if (currentHealth <= 0)
            {
                Die();    // 몬스터의 체력이 0 이하면 죽음 처리
            }
            else
            {
                anim.SetTrigger("Hurt"); // 피격 애니메이션 재생
                // 플레이어의 위치에 따라 넉백 방향 결정
                float knockbackDirection = transform.position.x - playerpos.x;
                if (knockbackDirection < 0)
                    knockbackDirection = -1f;
                else
                    knockbackDirection = 1f;


                StartCoroutine(KnockBack(knockbackDirection)); // 넉백 코루틴 실행
                StartCoroutine(damagedRoutine());  // 피격 상태 해제 코루틴 실행
                StartCoroutine(alphaBlink());   // 피격 효과를 위한 깜빡임 코루틴 실행
            }
        }
    }

    private void Die()
    {
        // 몬스터가 죽을 때의 동작을 구현
        anim.SetBool("Death", true);
    }

    public void removebat()
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
                rigid2D.velocity = new Vector2(2f * dir, 0);
            else
                rigid2D.velocity = new Vector2(2f * dir, 2f * dir);
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
            spriteRenderer.color = halfA; // 반투명 상태로 변경
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = fullA;  // 완전한 상태로 변경
        }
    }
}
