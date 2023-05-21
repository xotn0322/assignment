using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    SpriteRenderer spriteRenderer;

    #region VARIABLES
    public float jumpForce = 680.0f;
    public float WalkSpeed = 2.0f;
    public bool isGround;
    public bool isJump;
    public bool isAttack;
    public Vector2 boxSize;
    public Vector3 offset;
    private float attackTime;
    public float curHp;
    private bool isDead;
    #endregion

    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

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

    }

    private void Move()
    {
        // �� �� �̵�
        float axis = Input.GetAxisRaw("Horizontal");
        rigid2D.velocity = new Vector2(WalkSpeed * axis, rigid2D.velocity.y);

        // Walk
        if (axis != 0)
        {
            // ĳ���� ����, ���׿����� ���
            spriteRenderer.flipX = axis == -1;

            // �ǰ� offset ����, ���׿����� ��� ? true : false;
            offset.x = axis == -1 ? -0.4f : 0.4f;

            if (isGround)
                animator.Play("Walk");
        }
        // Idle
        if (axis == 0 && !isJump && isGround && !isAttack)
            animator.Play("Idle");
        // Fall
        if (!isGround)
            animator.Play("Fall");
        // Jump ������ �ٴ� Ȯ��
        if (isJump && isGround)
            isJump = false;
    }

    private void Jump()
    {
        //�ٴ� üũ, 0.07 ������ �� ����� ���̾� üũ << �� ��Ʈ������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f),
            0.07f,
            1 << LayerMask.NameToLayer("Ground"));

        // ����
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJump = true;
            this.rigid2D.AddForce(transform.up * this.jumpForce);
            animator.Play("Jump");
        }
    }

    private void Attack()
    {
        // ���� �� attacTime 0.5�� ������ ���� ����
        if (attackTime <= 0 && isGround)
        {
            isAttack = false;

            // ���콺 �� Ŭ��
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + offset, boxSize, 0);

                foreach(Collider2D hit in hits)
                {
                    if (hit.CompareTag("Enemy"))
                        hit.GetComponent<Enemy>().TakeHit(10f);
                }

                isAttack = true;
                animator.SetTrigger("Attack1");
                attackTime = 0.5f;
            }
        }
        // ��� �� attackTime ����
        else
            attackTime -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ӽ� �ǰ�
        if (collision.CompareTag("Enemy"))
        {
            bool mobisDead = collision.GetComponent<Enemy>().isDead;

            if (!mobisDead)
            {
                curHp -= 10f;

                // �÷��̾� ��� ó��
                if (curHp <= 0 && !isDead)
                {
                    animator.Play("Death");
                    isDead = true;
                    Invoke("DieScene", 2f);
                }
            }
        }
    }

    void DieScene()
    {
        SceneManager.LoadScene("DieScene");
    }
}
