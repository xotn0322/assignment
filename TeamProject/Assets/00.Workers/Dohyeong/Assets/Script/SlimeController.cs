using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    Animator anim;
    public float maxHealth = 10f;         // 몬스터의 최대 체력
    private float currentHealth;          // 몬스터의 현재 체력
    public Controller player;
    public SlimeMove move;
    public LayerMask isLayer;
    public bool isGround;
    public bool isJump = false;
    private Rigidbody2D slimeRigidbody;  // Rigidbody2D 컴포넌트를 저장하는 변수
    public Transform pos;
    public Vector2 boxSize;
    public float JumpForce = 210.0f;

    private Transform playerTransform;   // 플레이어의 위치를 저장하기 위한 변수
    private Transform slimeTransform;

    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트


    public void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;    // 몬스터의 체력을 최대 체력으로 초기화
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트 가져오기
        player = FindObjectOfType<Controller>();
        move = FindObjectOfType<SlimeMove>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
        slimeTransform = transform;
        slimeRigidbody = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 가져오기
    }


    public void Update()
    {
        // 슬라임이 땅에 있는지 체크
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.2f), 0.35f, isLayer);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                // 만약 슬라임이 땅에 있고, 플레이어가 슬라임의 점프 범위 내에 있다면
                if (isGround == true)
                {
                    isJump = true;
                    // 플레이어의 방향을 향해 점프
                    JumpTowards();
                }
                anim.SetBool("Jump", !isGround);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void JumpTowards()
    {
        anim.SetBool("Jump", true);
        // 점프 힘을 적용
        slimeRigidbody.AddForce(transform.up * JumpForce);
        // 일정 시간 후에 점프 애니메이션 종료
        StartCoroutine(EndJumpAnimation());
    }

    IEnumerator EndJumpAnimation()
    {
        // 점프 시간 대기
        yield return new WaitForSeconds(0.5f);  // 점프 애니메이션 재생 시간을 알맞게 조정
        // 슬라임이 바닥에 착지한 후에 점프 애니메이션 종료
        anim.SetBool("Jump", false);
        isJump = false;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("슬라임과 충돌");
            playerTransform.GetComponent<Controller>().Damaged(3f, slimeTransform.position); // 플레이어에게 데미지를 입히는 메서드 호출
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

    public void removeslime()
    {
        Destroy(gameObject);   // 몬스터 오브젝트 파괴
    }
}