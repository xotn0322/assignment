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
    public float respawnTime = 0.5f; //������ Ÿ��
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

    //����
    private void Jump()
    {
        //�ٴ� üũ radius = 0.07�� Circle�� ����
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.2f), 0.35f,
            isLayer);

        //����
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

                //Enemy�� TakeHit �� ���� ���� �޾��� ���� �޼��带 �ҷ��´�(Enemy Ȯ�� ��  �����ǰ�����)
                foreach(Collider2D hit in hits)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        Debug.Log("���ݴ���");
                        hit.GetComponent<SkeletonController>().TakeDamage(charDmg); //TakeHit() ȣ��, �μ��� ������
                    }
                }

                isAttack = true;
                animator.SetTrigger("Attack 0");
                attackTime = 0.5f;
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

            //���
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
   
    //�浹 ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���� ����
        if (collision.CompareTag("KillPlane")) 
        {
            charHp -= 1f; //ü�� ����
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
            Damaged(1f, collision.transform.position);
        }

        /*//�ǰ� ����(����)
        if (collision.CompareTag("Monster"))
        {
            Damaged(1f, collision.transform.position);
        }*/

        //��������
        if (collision.CompareTag("jumpingPlatform") && rigid2D.velocity.y < 0)
        {
            rigid2D.AddForce(jumpBlockPW, ForceMode2D.Impulse);
        }
    }

    //������
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
            charHp = 10f;//������ �� ü��
        }
        rigid2D.simulated = true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }

}