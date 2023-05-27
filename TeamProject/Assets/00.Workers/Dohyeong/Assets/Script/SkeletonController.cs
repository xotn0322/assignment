using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    Animator anim;
    public float maxHealth = 10f;         // 몬스터의 최대 체력
    private float currentHealth;          // 몬스터의 현재 체력
    public Controller player;
    public SkeletonMove move;
    public Transform pos;
    public Vector2 boxSize;
    public BoxCollider2D box;

    private Transform playerTransform;   // 플레이어의 위치를 저장하기 위한 변수
    private Transform skeletonTransform;



    public bool isAttack = false;
    public float attackCooldown = 0.001f; // 공격 쿨다운 시간
    private float currentCooldown = 0f; // 현재 쿨다운 시간

    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트


    public void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;    // 몬스터의 체력을 최대 체력으로 초기화
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트 가져오기
        player = FindObjectOfType<Controller>();
        move = FindObjectOfType<SkeletonMove>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
        skeletonTransform = transform;
    }


    public void Update()
    {
        if (!isAttack)
        {
            if (currentCooldown <= 0)
            {
                AttackPlayer();
                currentCooldown = attackCooldown;
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
        // 플레이어에게 피해를 입히는 로직 구현
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag =="Player")
            {
                move.moveDir = 0;
                anim.SetInteger("WalkSpeed", move.moveDir);
                anim.SetBool("Attack",true);
                isAttack = true; // 공격 중 상태로 변경
                break;
            }
        }
    }

    public void FinishAttackAnimation()
    {
        anim.SetBool("Attack", false);
        isAttack = false; // 공격 종료 후 상태 변경
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);
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
            playerTransform.GetComponent<Controller>().Damaged(2f, skeletonTransform.position); // 플레이어에게 데미지를 입히는 메서드 호출
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;   // 몬스터 체력에서 피해량을 감소시킴
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();    // 몬스터의 체력이 0 이하면 죽음 처리
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
}
