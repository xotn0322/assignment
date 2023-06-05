using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public GameObject bullet;
    public Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public Controller player;
    public float nextMove;
    public float groundDetectionDistance = 5f; // 절벽 감지 거리
    public bool isDamaged = false;
    public float maxHealth = 10f;         // 몬스터의 최대 체력
    public float currentHealth;
    public bool isDead = false;
    private Transform playerTransform;

    Color halfA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerTransform = GameObject.Find("Character").transform;
        player = FindObjectOfType<Controller>();
        Invoke("Think", 2);

    }

    void FixedUpdate()
    {
        // 절벽 감지
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, groundDetectionDistance);
        if (groundCheck.collider == null)
        {
            // 절벽 감지: 이동 방향을 반전
            nextMove *= -1;
            spriteRenderer.flipX = nextMove == 1;
        }

        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
    }

    void Think()
    {
        nextMove = UnityEngine.Random.Range(-1, 2);
        Invoke("Think", 2);

        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        if (nextMove == -1)
        {
            // 플레이어와의 상대 위치 계산
            Vector2 playerDirection = playerTransform.position - transform.position;

            // 플레이어가 반대편에 위치할 경우 몬스터의 시선을 반대로 설정
            if ((nextMove == -1 && playerDirection.x > 0) || (nextMove == 1 && playerDirection.x < 0))
                spriteRenderer.flipX = !spriteRenderer.flipX;

            // 공격 실행
            Vector3 bulletPosition = transform.position;
            GameObject bulletCopy = Instantiate(bullet, bulletPosition, transform.rotation);
            anim.SetBool("Attack", true);
            StartCoroutine(StopAttackAnimation());
        }
}

    IEnumerator StopAttackAnimation()
    {
        yield return new WaitForSeconds(3f); // 공격 애니메이션 재생 시간에 따라 조정
        anim.SetBool("Attack", false);
    }
    public void TakeDamage(float damageAmount, Vector2 playerpos)
    {
        if (!isDamaged)
        {
            isDamaged = true;
            StartCoroutine(damagedRoutine());
            currentHealth -= damageAmount;   // 몬스터 체력에서 피해량을 감소시킴

            if (currentHealth <= 0)
            {
                Die();    // 몬스터의 체력이 0 이하면 죽음 처리
            }
            else
            {
                StartCoroutine(alphaBlink());
            }
        }
    }

    private void Die()
    {
        // 몬스터가 죽을 때의 동작을 구현
        anim.SetBool("Dead", true);
        GameObject.Find("BossZone").GetComponent<bossZone>().bossKilled = true;
        GameObject.Find("BossZone").GetComponent<bossZone>().GateOpen();
        // 2초 후에 오브젝트 삭제
        Destroy(gameObject, 2f);
    }
    IEnumerator alphaBlink()
    {
        while (isDamaged)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = halfA;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = fullA;
        }
    }

    IEnumerator damagedRoutine()
    {
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }
}