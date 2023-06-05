using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    public float maxHealth = 10f;         // 몬스터의 최대 체력
    private float currentHealth;          // 몬스터의 현재 체력
    public Controller player;              // 플레이어 컨트롤러 참조
    public float distance;                 // 공격 가능 거리
    public float atkDistance;             // 독 공격 가능 거리
    public LayerMask isLayer;         // 공격 대상 레이어 마스크
    Animator anim;                            // 애니메이터 컴포넌트 참조
    Rigidbody2D rigid2D;                  // 리지드바디2D 컴포넌트 참조
    public Transform pos;                 // 공격 범위를 나타내는 상자 위치
    public Vector2 boxSize;               // 공격 범위 상자 크기
    public BoxCollider2D box;             // 공격 범위 상자 컴포넌트

    public float attackCooldown = 0.0001f; // 공격 쿨다운 시간
    private float currentCooldown; // 현재 쿨다운 시간

    public GameObject bullet;         // 독 발사 오브젝트
    public Transform bulletpos;         // 독 발사 위치
    public MushroomMove move;   // 버섯 이동 스크립트 참조

    private Transform playerTransform;   // 플레이어의 트랜스폼
    private Transform mushroomTransform;  // 몬스터의 트랜스폼

    public bool isAttack = false;                   // 현재 공격 중인지 여부
    public bool isPoisonAttack = false;         // 현재 독 공격 중인지 여부
    public bool isKnockback = false;           // 현재 넉백 상태인지 여부
    public bool isDamaged = false;            // 현재 피해를 받은 상태인지 여부
    public bool isMovingRight = false;        // 오른쪽으로 이동 중인지 여부

    public SpriteRenderer spriteRenderer;   // 스프라이트 렌더러 컴포넌트
    Color halfA = new Color(1, 1, 1, 0.5f);  // 투명도 반값인 색
    Color fullA = new Color(1, 1, 1, 1);    // 투명도 1인 색

    void Start()
    {
        anim = GetComponent<Animator>();                     // 애니메이터 컴포넌트 참조
        rigid2D = GetComponent<Rigidbody2D>();           // 리지드바디2D 컴포넌트 참조
        move = FindObjectOfType<MushroomMove>();      // MushroomMove 스크립트를 찾아서 참조
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // "Player" 태그를 가진 오브젝트의 트랜스폼 참조
        spriteRenderer = GetComponent<SpriteRenderer>();        // 스프라이트 렌더러 컴포넌트 참조
        player = FindObjectOfType<Controller>();                       // Controller 스크립트를 찾아서 참조
        mushroomTransform = transform;                                 // 몬스터의 트랜스폼 참조
    }

    public float cooltime;                           // 공격 쿨타임
    private float currenttime;                   // 현재 시간

    void FixedUpdate()
    {
        SetMovingRight(); // 독 발사 방향 설정

        RaycastHit2D raycast;
        if (isMovingRight)
        {
            raycast = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);    // 오른쪽 방향으로 거리(distance)만큼 레이캐스트를 발사하고 충돌 정보를 raycast 변수에 저장
        }
        else
        {
            raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);    // 왼쪽 방향으로 거리(distance)만큼 레이캐스트를 발사하고 충돌 정보를 raycast 변수에 저장
        }
        if (raycast.collider!=null)
        {
            if (Vector2.Distance(transform.position, raycast.collider.transform.position) < atkDistance)
            {
                if (currenttime <= 0)
                {
                    GameObject bulletcopy = Instantiate(bullet, bulletpos.position, Quaternion.Euler(0f, 0f, isMovingRight ? 0f : 180f));   // 독 발사 오브젝트를 bulletpos 위치에 생성하고, 좌우 방향을 설정하기 위해 쿼터니언으로 회전 정보를 설정
                    move.moveDir = 0;
                    anim.SetInteger("WalkSpeed", move.moveDir);
                    anim.SetBool("Poison", true);
                    isPoisonAttack = true;
                    currenttime = cooltime;         // 독 공격 쿨타임 설정
                }
            }

            currenttime -= Time.deltaTime;
        }

        if (!isAttack&& !isKnockback)
        {
            if (currentCooldown <= 0)
            {
                AttackPlayer();                          // 플레이어를 공격하는 메서드 호출
                currentCooldown = attackCooldown;
            }
            else if (currentCooldown > 0 && !anim.GetBool("Attack"))
            {
                currentCooldown -= Time.deltaTime;
            }
        }
        else
        {
            spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);  // 플레이어의 위치에 따라 스프라이트 좌우 반전
        }
    }

    public void FinishPoisonAttackAnimation()
    {
        anim.SetBool("Poison", false);
        isPoisonAttack = false; // 독 공격 종료 후 상태 변경
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);  // 플레이어의 위치에 따라 스프라이트 좌우 반전
    }

    void SetMovingRight()
    {
        Vector3 playerPos = playerTransform.position;
        if (playerPos.x > transform.position.x)
        {
            isMovingRight = true;
            bulletpos.localScale = new Vector3(1, 1, 1); // 독 발사 위치의 스케일을 원래대로 설정 (오른쪽 방향)
        }
        else
        {
            isMovingRight = false;
            bulletpos.localScale = new Vector3(-1, 1, 1); // 독 발사 위치의 스케일을 뒤집어서 설정 (왼쪽 방향)
        }
    }

    public void AttackPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0); // pos 위치를 기준으로 boxSize 크기의 박스 콜라이더와 겹치는 모든 콜라이더를 가져옴
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                move.moveDir = 0;
                anim.SetInteger("WalkSpeed", move.moveDir);
                anim.SetBool("Attack", true);  // 애니메이션 상태 변경
                isAttack = true;                   // 공격 중 상태로 변경
                break;
            }
        }
    }
    
    public void FinishAttackAnimation()
    {
        anim.SetBool("Attack", false);  // 공격 애니메이션 종료
        isAttack = false; // 공격 종료 후 상태 변경
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);   // 플레이어의 위치에 따라 스프라이트 좌우 반전
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);     // 공격 범위 상자를 기준으로 boxSize 크기의 노란색 와이어 큐브를 그림
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
            Debug.Log("버섯과 충돌");
            playerTransform.GetComponent<Controller>().Damaged(3f, mushroomTransform.position); // 버섯과 충돌하면 플레이어에게 데미지를 입히는 메서드 호출
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
                Die();                                          // 몬스터의 체력이 0 이하면 죽음 처리
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
        anim.SetBool("Death", true);  // 몬스터가 죽을 때의 동작을 구현
    }

    public void removemushroom()
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
            spriteRenderer.color = fullA;   // 완전한 상태로 변경
    }
    }
}
