using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    Animator anim;                           // 애니메이터 컴포넌트
    Rigidbody2D rigid2D;                   // Rigidbody2D 컴포넌트
    public float maxHealth = 40f;         // 몬스터의 최대 체력
    private float currentHealth;          // 몬스터의 현재 체력
    public Controller player;               // 플레이어 컨트롤러 스크립트
    public SlimeMove move;              // 슬라임 이동 스크립트
    public LayerMask isLayer;            // 충돌 체크 레이어

    public bool isGround;                  // 땅에 닿은 상태인지 여부
    public bool isJump = false;         // 점프 중인지 여부
    public bool isKnockback = false; // 넉백 중인지 여부
    public bool isDamaged = false;   // 피격 상태인지 여부

    public Transform pos;             // 점프 범위 상자 위치
    public Vector2 boxSize;         // 점프 범위 박스 크기
    public float JumpForce = 500f;  // 점프 힘
    private bool canJump = true;     // 점프 가능 여부

    private Transform playerTransform;   // 플레이어의 위치를 저장하기 위한 변수
    private Transform slimeTransform;       // 슬라임의 위치를 저장하기 위한 변수

    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트

    Color halfA = new Color(1, 1, 1, 0.5f);  // 투명도 반값인 색
    Color fullA = new Color(1, 1, 1, 1);    // 투명도 전체인 색


    public void Start()
    {
        anim = GetComponent<Animator>();   // 애니메이터 컴포넌트 가져오기
        rigid2D = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 가져오기
        currentHealth = maxHealth;    // 몬스터의 체력을 최대 체력으로 초기화
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트 가져오기
        player = FindObjectOfType<Controller>();  // 플레이어 컨트롤러 스크립트 가져오기
        move = FindObjectOfType<SlimeMove>();  // 슬라임 이동 스크립트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
        slimeTransform = transform; // 슬라임의 위치 저장
    }


    public void FixedUpdate()
    {
        // 지형 체크
        Vector2 groundCheckPos = transform.position - new Vector3(0, 0.5f, 0);  // 슬라임의 아래쪽 지점을 체크
        RaycastHit2D groundRaycast = Physics2D.Raycast(groundCheckPos, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        isGround = groundRaycast.collider != null;  // 지면과 충돌했는지 여부를 isGround 변수에 할당
         

        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);  // 충돌 박스 내의 콜라이더 가져오기
        bool playerInRange = false;  // 플레이어가 슬라임의 범위 내에 있는지 여부

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                playerInRange = true;  // 플레이어가 슬라임의 콜라이더 내에 있다면 true로 설정
            }
        }
        // 만약 슬라임이 땅에 있고, 플레이어가 슬라임의 점프 범위 내에 있다면
        if (isGround && playerInRange && !isJump && !isKnockback && !isDamaged && canJump)
        {
            Debug.Log("점프한다!");
            
            // 플레이어의 방향을 향해 점프
            JumpTowards();

            canJump = false;  // 점프 후 점프를 막기 위해 canJump 변수를 false로 설정
            StartCoroutine(EnableJump());  // 1초 후에 다시 점프를 가능하게 해주는 코루틴 실행
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize); // Gizmos를 통해 점프 범위 충돌 박스를 그려주는 함수
    }

    void JumpTowards()
    {
        isJump = true;
        anim.SetBool("Jump", true); // 점프 애니메이션 재생을 위해 애니메이션 변수를 true로 설정
        StartCoroutine(Jump());  // 점프 코루틴 실행
    }

    void EndJumpAnimation()   // 점프 애니메이션 종료 처리 함수
    {
        isJump = false;
        anim.SetBool("Jump", false);  // 애니메이션 변수를 false로 설정
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("슬라임과 충돌");
            playerTransform.GetComponent<Controller>().Damaged(4f, slimeTransform.position); // 슬라임과 충돌하면 플레이어에게 데미지를 입히는 메서드 호출
            isJump = false;
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
                StartCoroutine(damagedRoutine()); // 피해 처리를 위한 코루틴 실행
                StartCoroutine(alphaBlink());       // 깜빡임 효과를 위한 코루틴 실행
            }
        }
    }

    private void Die()
    {
        anim.SetBool("Death", true); // 몬스터가 죽을 때의 동작을 구현
    }

    public void removeslime()
    {
        Destroy(gameObject);   // 몬스터 오브젝트 파괴
    }

    IEnumerator Jump()
    {
        float jumpTime = JumpForce / 100f; // 점프 애니메이션 재생 시간 계산
        float elapsedTime = 0f;

        // 플레이어와 슬라임의 위치 차이 계산
        float playerDirection = playerTransform.position.x - slimeTransform.position.x;

        // 플레이어의 위치에 따라 점프 방향 결정
        float jumpDirection = (playerDirection < 0) ? -1f : 1f;



        while (elapsedTime < jumpTime)
        {
            float jumpProgress = elapsedTime / jumpTime;
            float jumpForce = Mathf.Lerp(JumpForce, 0f, jumpProgress); // 초기 점프 힘에서 0까지 선형 보간

            move.moveDir = (playerTransform.position.x > slimeTransform.position.x) ? 3 : -3;

            // 플레이어 위치에 따라 점프 방향 결정
            rigid2D.velocity = new Vector2(jumpDirection * rigid2D.velocity.x, jumpForce);

            elapsedTime += Time.deltaTime * 0.5f;
            yield return null;
        }
    }

    IEnumerator EnableJump()
    {
        yield return new WaitForSeconds(1f);  // 1초 대기
        canJump = true;  // 점프 가능하도록 canJump 변수를 true로 설정
    }

    IEnumerator KnockBack(float dir)
    {
        isKnockback = true;  // 넉백 중임을 나타내는 변수를 true로 설정
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
        yield return new WaitForSeconds(2f);
        isDamaged = false;
    }

    IEnumerator alphaBlink()
    {
        while (isDamaged)
        {
            yield return new WaitForSeconds(0.1f); // 투명도 반값인 색으로 설정
            spriteRenderer.color = halfA;
            yield return new WaitForSeconds(0.1f);  // 투명도 전체인 색으로 설정
            spriteRenderer.color = fullA;
        }
    }
}
