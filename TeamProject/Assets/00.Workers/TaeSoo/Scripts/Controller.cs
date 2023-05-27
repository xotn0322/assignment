using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rigid2D;
    public SpriteRenderer spriteRenderer;

    #region VARIABLES
    public float WalkSpeed = 2.0f;
    public float JumpForce = 275.0f;
    public float charHp = 10f;
    public int charDmg = 5;
    public float speed;
    public float respawnTime = 0.5f; //리스폰 타임
    public int jumpCount = 0;
    public bool isGround;
    public bool isJump = false;
    public bool isAttack;
    public bool isRun;
    public bool isDamaged = false;
    public bool isDead = false;
    public bool isKnokback = false;
    public Vector2 jumpBlockPW = new Vector2(0, 35);
    public Vector2 boxSize;
    public Vector2 spawnPoint;
    public Vector3 offset;
    public LayerMask isLayer;
    Color halfA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);

    private float attackTime;
    #endregion


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPoint = transform.position;

        animator.Play("Idle");
    }

    
    void Update()
    {
        if (!isDead)
        {
            Move();
            Jump();
            Attack();
        }

        //OnDrawGizmos();
    }

    //기본 움직임
    private void Move()
    {
        // ← → 이동
        float axis = Input.GetAxisRaw("Horizontal"); //axis에 좌 : -1, 우 : 1을 반환
        if (!isAttack)
            rigid2D.velocity = new Vector2(WalkSpeed * axis, rigid2D.velocity.y);

        //Run
        if (axis != 0 && !isAttack)
        {
            //axis 값에 따른 캐릭터 좌우 반전
            if (axis == -1)
            {
                spriteRenderer.flipX = true;
                offset.x = -0.4f;
            }
            else
            {
                spriteRenderer.flipX = false;
                offset.x = 0.4f;
            }

            animator.SetBool("Run", true);
            isRun = true;
        }

        //idle
        if (axis == 0 && !isJump && isGround && !isAttack)
        {
            animator.SetBool("Run", false);
            isRun = false;
            animator.Play("Idle");
        }
        
        if (isJump && isGround && rigid2D.velocity.y == 0)
        {
            isJump = false;
            jumpCount = 0;
        }
    }

    //점프
    private void Jump()
    {
        //바닥 체크 radius = 0.07인 Circle의 범위
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.2f), 0.35f,
            isLayer);

        //점프
        if (Input.GetKeyDown(KeyCode.Space) && isGround && !isAttack && jumpCount == 0)
        {
            isJump = true;
            jumpCount++;
            rigid2D.AddForce(transform.up * JumpForce);
            animator.SetBool("Jump", true);
        }

        //jump check
        animator.SetBool("Jump", !isGround);

        //fall check
        animator.SetFloat("yvelocity", rigid2D.velocity.y);
    }

    //공격
    private void Attack()
    {
        // 공격 후 쿨타임
        if (attackTime <= 0 && isGround && !isRun)
        {
            isAttack = false;

            //키보드 z클릭시 공격
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + offset, boxSize, 0);

                //Enemy의 TakeHit 즉 적의 공격 받았을 때의 메서드를 불러온다(Enemy 확인 및  몬스터피격판정)
                foreach(Collider2D hit in hits)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        Debug.Log("공격닿음");
                        hit.GetComponent<SkeletonController>().TakeDamage(charDmg); //TakeHit() 호출, 인수는 데미지
                    }
                }

                isAttack = true;
                animator.SetTrigger("Attack 0");
                attackTime = 0.5f;
            }
        }
        //공격 안할 때 attackTime감소
        else
        {
            //animator.SetBool("Attack", false);
            attackTime -= Time.deltaTime;
        }
    }

    //피격
    public void Damaged(float damage, Vector2 pos) //damage & pos 모두 몬스터의 변수
    {
        if (!isDamaged)
        {
            isDamaged = true;
            charHp -= damage;

            //사망
            if (charHp <= 0 && !isDead)
            {
                isDead = true;
                StartCoroutine(Respawn(respawnTime));
            }
            else
            {
                //animator.SetTrigger("");

                float x = transform.position.x - pos.x;
                if (x < 0)
                    x = -1;
                else
                    x = 1;

                StartCoroutine(knokBack(x));
                StartCoroutine(damagedRoutine());
                StartCoroutine(alphaBlink());
            }
        }
    }
   
    //충돌 판정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //낙사 판정
        if (collision.CompareTag("KillPlane")) 
        {
            charHp -= 1f; //체력 감소
            StartCoroutine(Respawn(respawnTime));
        }

        //체크포인트
        if (collision.CompareTag("CheckPoints"))
        {
            spawnPoint = transform.position;
        }

        //피격 판정(함정)
        if (collision.CompareTag("Enemy"))
        {
            Damaged(1f, collision.transform.position);
        }

        /*//피격 판정(몬스터)
        if (collision.CompareTag("Monster"))
        {
            Damaged(1f, collision.transform.position);
        }*/

        //점프발판
        if (collision.CompareTag("jumpingPlatform") && rigid2D.velocity.y < 0)
        {
            rigid2D.AddForce(jumpBlockPW, ForceMode2D.Impulse);
        }
    }

    //리스폰
    IEnumerator Respawn(float duration)
    {
        rigid2D.simulated = false;
        rigid2D.velocity = new Vector2(0, 0);
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        transform.position = spawnPoint;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        if (isDead)
        {
            isDead = false;
            charHp = 10f;//리스폰 시 체력
        }
        rigid2D.simulated = true;
    }

    //넉백
    IEnumerator knokBack(float dir)
    {
        isKnokback = true;
        float ctime = 0;
        while (ctime < 0.2f) //넉백시간
        {
            if (rigid2D.velocity.y == 0)
                rigid2D.velocity = new Vector2(3f * dir, 0);
            else
                rigid2D.velocity = new Vector2(3f * dir, 3f * dir);
            ctime += Time.deltaTime;
            yield return null;
        }
        isKnokback = false;
    }

    //무적시간
    IEnumerator damagedRoutine()
    {
        yield return new WaitForSeconds(3f);
        isDamaged = false;
    }

    //피격 효과
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }

}