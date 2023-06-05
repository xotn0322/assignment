using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rigid2D;
    public SpriteRenderer spriteRenderer;

    #region VARIABLES
    //ĳ���� ��ġ
    public float WalkSpeed = 2.0f; //�ȴ� �ӵ�
    public float JumpForce = 250.0f; //��������
    public float maxVelocityY; //�ִ� ��������
    public float charHp = 100f; //ü��
    public float respawnTime = 0.5f; //������ Ÿ��
    public Vector2 spawnPoint; //��������
    public int jumpCount = 2; //�������� Ƚ��
    public int charDmg = 10; //ĳ������ ������
    public Vector2 boxSize; //���ݹ��� (��ġ����x, ��ȭ�鿡�� ����)
    public Vector3 offset; //���ݹ��� �ڽ� ��ġ���� (��ġ����x, ��ȭ�鿡�� ����)
    private float attackTime; //���ݼӵ� (������ 191���ο���)

    //�������� ��ȣ�ۿ�
    public float sidePower = 0.1f; //�������� ������ �ۿ��ϴ� �� (����x)
    public float sideSpeed; //�������� ������ �����̴� �ӵ� (����x)
    public int sideFlag = 0; //�������� �������� �������� (����x)
    public Vector2 jumpBlockPW; //�������� ��

    //����Ȯ��
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
    
    //�����ð��� ������(���� ����)
    Color halfA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);

    //����
    public AudioClip walkClip;
    public AudioClip attack;
    public AudioClip damaged;
    public AudioClip jumpPF;
    public AudioClip nonAttack;
    AudioSource soundSource;


    public LayerMask isLayer; // isGround���� ���Ǵ� ���̾��ũ

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

            //idle and ����������� ����
            if (sideState && isIdle)
                SideMove();
            
            //OnDrawGizmos();
        }
    }

    //�⺻ ������
    private void Move()
    {
        // �� �� �̵�
        float axis = Input.GetAxisRaw("Horizontal"); //axis�� �� : -1, �� : 1�� ��ȯ
        if (!isAttack)
            rigid2D.velocity = new Vector2(WalkSpeed * axis, rigid2D.velocity.y);

        //Run
        if (axis != 0 && !isAttack)
        {
            //axis ���� ���� ĳ���� �¿� ����
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
        
        //���� �� ���� ��Ҵ��� Ȯ��
        if (isJump && isGround && rigid2D.velocity.y == 0)
        {
            isJump = false;
            jumpCount = 2;
        }
    }

    //����
    private void Jump()
    {
        //�ٴ� üũ radius = 0.35�� Circle�� ����
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.2f), 0.1f,
            isLayer);

        //����
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

    //�ִ� �������� ����
    void limitJumpSpeed()
    {
        if (rigid2D.velocity.y > maxVelocityY)
        {
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, maxVelocityY);
        }
    }

    //���� ���� ���� ���� �� ������
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

    //����
    private void Attack()
    {
        // ���� �� ��Ÿ��
        if (attackTime <= 0 && isGround && !isRun)
        {
            isAttack = false;

            //Ű���� zŬ���� ����
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + offset, boxSize, 0);

                isAttack = true;
                animator.SetTrigger("Attack 0");
                attackTime = 0.5f; //���ݼӵ� ����

                //Enemy�� TakeHit �� ���� ���� �޾��� ���� �޼��带 �ҷ��´�(Enemy Ȯ�� ��  �����ǰ�����)
                foreach (Collider2D hit in hits)
                {
                    if (hit.gameObject.tag == "Skeleton")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("���ݴ���");
                        hit.GetComponent<SkeletonController>().TakeDamage(charDmg,transform.position); //TakeHit() ȣ��, �μ��� ������
                    }
                    else if (hit.gameObject.tag == "Slime")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("���ݴ���");
                        hit.GetComponent<SlimeController>().TakeDamage(charDmg, transform.position); //TakeHit() ȣ��, �μ��� ������
                    }
                    else if (hit.gameObject.tag == "Mushroom")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("���ݴ���");
                        hit.GetComponent<MushroomController>().TakeDamage(charDmg, transform.position); //TakeHit() ȣ��, �μ��� ������
                    }
                    else if (hit.gameObject.tag == "Bat")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("���ݴ���");
                        hit.GetComponent<BatController>().TakeDamage(charDmg, transform.position); //TakeHit() ȣ��, �μ��� ������
                    }
                    else if (hit.gameObject.tag == "Boss")
                    {
                        Sound.instance.SFXPlay("Attack", attack);
                        Debug.Log("���ݴ���");
                        hit.GetComponent<BossMove>().TakeDamage(charDmg, transform.position); //TakeHit() ȣ��, �μ��� ������
                    }
                    else
                    {
                        Sound.instance.SFXPlay("nonAttack", nonAttack);
                    }
                }
            }
        }
        //���� ���� �� attackTime����
        else
        {
            //animator.SetBool("Attack", false);
            attackTime -= Time.deltaTime;
        }
    }

    //�ǰ�
    public void Damaged(float damage, Vector2 pos) //damage & pos ��� ������ ����
    {
        if (!isDamaged)
        {
            isDamaged = true;
            charHp -= damage;
            Sound.instance.SFXPlay("Damaged", damaged);

            //���
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
   
    //�浹 ���� ontrigger�� ������ is Trigger�� Ȱ��ȭ�ؾ� �۵�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���� ����
        if (collision.CompareTag("KillPlane")) 
        {
            charHp -= 1f; //ü�� ����
            if (charHp <= 0)
            {
                isDead = true;
            }
            isDamaged = true;
            StartCoroutine(damagedRoutine());
            StartCoroutine(alphaBlink());
            StartCoroutine(Respawn(respawnTime));
        }

        //üũ����Ʈ
        if (collision.CompareTag("CheckPoints"))
        {
            spawnPoint = transform.position;
        }

        //�ǰ� ����(����)
        if (collision.CompareTag("Enemy"))
        {
            Damaged(5f, collision.transform.position);
        }

        /*//�ǰ� ����(����)
        if (collision.CompareTag("Monster"))
        {
            Damaged(1f, collision.transform.position);
        }*/

        //��������
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
        //���� ���� ����� �� 
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

    //������
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

    

    //�˹�
    IEnumerator knokBack(float dir)
    {
        isKnokback = true;
        float ctime = 0;
        while (ctime < 0.2f) //�˹�ð�
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

    //�����ð�
    IEnumerator damagedRoutine()
    {
        yield return new WaitForSeconds(3f);
        isDamaged = false;
    }

    //�ǰ� ȿ��
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

    //�ȴ� �Ҹ�
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