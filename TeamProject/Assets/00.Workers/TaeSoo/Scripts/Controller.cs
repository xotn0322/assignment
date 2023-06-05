using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rigid2D;
    public SpriteRenderer spriteRenderer;

    #region VARIABLES
    //캐릭터 수치
    public float WalkSpeed = 2.0f; //걷는 속도
    public float JumpForce = 250.0f; //점프높이
    public float maxVelocityY; //최대 점프높이
    public float charHp = 100f; //체력
    public float respawnTime = 0.5f; //리스폰 타임
    public Vector2 spawnPoint; //스폰지점
    public int jumpCount = 2; //점프가능 횟수
    public int charDmg = 10; //캐릭터의 데미지
    public Vector2 boxSize; //공격범위 (수치조절x, 씬화면에서 조정)
    public Vector3 offset; //공격범위 박스 위치조정 (수치조절x, 씬화면에서 조정)
    private float attackTime; //공격속도 (조정은 191라인에서)

    //무빙스톤 상호작용
    public float sidePower = 0.1f; //무빙스톤 위에서 작용하는 힘 (변경x)
    public float sideSpeed; //무빙스톤 위에서 움직이는 속도 (변경x)
    public int sideFlag = 0; //무빙스톤 위에서의 방향지정 (변경x)
    public Vector2 jumpBlockPW; //점프발판 힘

    //상태확인
    public bool isGround;
    public bool isJump = false;
    public bool isFall = false;
    public bool isAttack;
    public bool isRun;
    public bool isDamaged = false;
    public bool isDead = false;
    public bool isKnokback = false;
    public bool isIdle;
    public bool isJumpPF = false;
    public bool sideState = false;
    
    //무적시간의 깜빡임(투명도 조절)
    Color halfA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);

    //사운드
    public AudioClip walkClip;
    public AudioClip attack;
    public AudioClip damaged;
    public AudioClip jumpPF;
    public AudioClip nonAttack;
    AudioSource soundSource;


    public LayerMask isLayer; // isGround에서 사용되는 레이어마스크

    #endregion


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundSource = GetComponent<AudioSource>();
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

            //idle and 무브발판위에 존재
            if (sideState && isIdle)
                SideMove();
            
            //OnDrawGizmos();
        }
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

            StartCoroutine(WalkSound());
            animator.SetBool("Run", true);
            isRun = true;
        }

        //idle
        if (axis == 0 && !isJump && isGround && !isAttack)
        {
            isIdle = true;
            animator.SetBool("Run", false);
            isRun = false;
            animator.Play("Idle");
        }
        else
            isIdle = false;
        
        //점프 후 땅에 닿았는지 확인
        if (isJump && isGround && rigid2D.velocity.y == 0)
        {
            isJump = false;
            jumpCount = 2;
        }
    }

    //점프
    private void Jump()
    {
        //바닥 체크 radius = 0.35인 Circle의 범위
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.2f), 0.1f,
            isLayer);

        //점프
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack && jumpCount > 0)
        {
            isJump = true;
            jumpCount--;
            rigid2D.AddForce(transform.up * JumpForce);
            animator.SetBool("Jump", true);
            limitJumpSpeed();
        }

        //jump check
        animator.SetBool("Jump", !isGround);


        //fall check
        animator.SetFloat("yvelocity", rigid2D.velocity.y);
        if (rigid2D.velocity.y < 0)
            isFall = true;
        else
            isFall = false;
    }

    //최대 점프높이 제한
    void limitJumpSpeed()
    {
        if (rigid2D.velocity.y > maxVelocityY)
        {
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, maxVelocityY);
        }
    }

    //무브 발판 위에 존재 시 움직임
    private void SideMove()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (this.sideFlag == 1)
        {
            moveVelocity = new Vector3(sidePower, 0, 0);
        }
        else
        {
            moveVelocity = new Vector3(-sidePower, 0, 0);
        }
        transform.position += moveVelocity * sideSpeed * Time.deltaTime;
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

                isAttack = true;
                animator.SetTrigger("Attack 0");
                attackTime = 0.5f; //공격속도 조절

                //Enemy의 TakeHit 즉 적의 공격 받았을 때의 메서드를 불러온다(Enemy 확인 및  몬스터피격판정)
                foreach (Collider2D hit in hits)
                {
                    if (hit.gameObject.tag == "Skeleton")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("공격닿음");
                        hit.GetComponent<SkeletonController>().TakeDamage(charDmg,transform.position); //TakeHit() 호출, 인수는 데미지
                    }
                    else if (hit.gameObject.tag == "Slime")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("공격닿음");
                        hit.GetComponent<SlimeController>().TakeDamage(charDmg, transform.position); //TakeHit() 호출, 인수는 데미지
                    }
                    else if (hit.gameObject.tag == "Mushroom")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("공격닿음");
                        hit.GetComponent<MushroomController>().TakeDamage(charDmg, transform.position); //TakeHit() 호출, 인수는 데미지
                    }
                    else if (hit.gameObject.tag == "Bat")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("공격닿음");
                        hit.GetComponent<BatController>().TakeDamage(charDmg, transform.position); //TakeHit() 호출, 인수는 데미지
                    }
                    else if (hit.gameObject.tag == "Boss")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("공격닿음");
                        hit.GetComponent<BossMove>().TakeDamage(charDmg, transform.position); //TakeHit() 호출, 인수는 데미지
                    }
                    else
                    {
                        Sound.instance.SFXPlay("nonAttack", nonAttack);
                    }
                }
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
            Sound.instance.SFXPlay("Damaged", damaged);

            //사망
            if (charHp <= 0 && !isDead)
            {
                isDead = true;
                StartCoroutine(Respawn(4f));
                GameObject.Find("BossZone").GetComponent<bossZone>().GateOpen();
            }
            else
            { 
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
   
    //충돌 판정 ontrigger는 한쪽이 is Trigger를 활성화해야 작동
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //낙사 판정
        if (collision.CompareTag("KillPlane")) 
        {
            charHp -= 1f; //체력 감소
            if (charHp <= 0)
            {
                isDead = true;
            }
            isDamaged = true;
            StartCoroutine(damagedRoutine());
            StartCoroutine(alphaBlink());
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
            Damaged(5f, collision.transform.position);
        }

        /*//피격 판정(몬스터)
        if (collision.CompareTag("Monster"))
        {
            Damaged(1f, collision.transform.position);
        }*/

        //점프발판
        if (collision.CompareTag("jumpingPlatform") && rigid2D.velocity.y < 0)
        {
            Sound.instance.SFXPlay("JumpPF", jumpPF);
            jumpBlockPW = new Vector2(0, ((rigid2D.velocity.y) * -1.2f) + 6);
            isJumpPF = true;
            rigid2D.AddForce(jumpBlockPW, ForceMode2D.Impulse);
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //무빙 발판 밟았을 때 
        if (collision.gameObject.tag == "MovePlatform" && (transform.position.y - collision.transform.position.y) > 0.5)
        {
            sideState = true;
            if (collision.gameObject.GetComponent<MovingPlatform>().pointSelection == 0)
                sideFlag = 2;
            else
                sideFlag = 1;
            sideSpeed = (collision.gameObject.GetComponent<MovingPlatform>().moveSpeed) * 10;
            //sideSpeed = 3.0f;
        }
        else if (collision.gameObject.tag == "MovePlatform2" && (transform.position.y - collision.transform.position.y) > 0.5)
        {
            sideState = true;
            if (collision.gameObject.GetComponent<MovingPlatform>().pointSelection == 0)
                sideFlag = 1;
            else
                sideFlag = 2;
            sideSpeed = (collision.gameObject.GetComponent<MovingPlatform>().moveSpeed) * 10;
            //sideSpeed = 3.0f;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovePlatform" && (transform.position.y - collision.transform.position.y) > 0.5)
        {
            if (collision.gameObject.GetComponent<MovingPlatform>().pointSelection == 0)
                sideFlag = 2;
            else
                sideFlag = 1;
            sideSpeed = (collision.gameObject.GetComponent<MovingPlatform>().moveSpeed) * 10;
            //sideSpeed = 3.0f;
        }
        else if (collision.gameObject.tag == "MovePlatform2" && (transform.position.y - collision.transform.position.y) > 0.5)
        {
            sideState = true;
            if (collision.gameObject.GetComponent<MovingPlatform>().pointSelection == 0)
                sideFlag = 1;
            else
                sideFlag = 2;
            sideSpeed = (collision.gameObject.GetComponent<MovingPlatform>().moveSpeed) * 10;
            //sideSpeed = 3.0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovePlatform")
        {
            sideState = false;
        }
    }

    //리스폰
    IEnumerator Respawn(float duration)
    {
        if (isDead)
        {
            //animator.SetBool("Dead", true);
            animator.SetTrigger("Death");
            yield return new WaitForSeconds(duration);
            transform.position = spawnPoint;
            isDead = false;
            charHp = 100f;
            //animator.SetBool("Dead", false);
        }
        else
        {
            rigid2D.simulated = false;
            rigid2D.velocity = new Vector2(0, 0);
            transform.localScale = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(duration);
            transform.position = spawnPoint;
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            rigid2D.simulated = true;
        }
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

    //걷는 소리
    IEnumerator WalkSound()
    {
        if (!soundSource.isPlaying && !isJump && !isFall)
        {
            soundSource.clip = walkClip;
            soundSource.Play();
            if (!isJump && !isFall)
                yield return new WaitForSeconds(0.5f);
            else
                soundSource.Stop();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }

}