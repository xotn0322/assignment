using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;

    public float MaxHp;
    public float CurHp;
    public bool isDead;
    public float nextPos;
    private bool getDamage;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        CurHp = MaxHp;
        Invoke("Move", 5);
        animator.Play("Idle");
    }

    private void Update()
    {
        if (!isDead && !getDamage)
        {
            rigid2D.velocity = new Vector2(nextPos, rigid2D.velocity.y);

            // Walk, ���� �̵� x �� 0���� ������ ���� ����
            spriteRenderer.flipX = nextPos < 0 ? true : false;

            if (rigid2D.velocity.x != 0)
                animator.Play("Walk");

            if (rigid2D.velocity.x == 0)
                animator.Play("Idle");
        }
    }

    // �ǰ� �Լ�, �÷��̾� ��ũ��Ʈ���� ȣ��
    public void TakeHit(float damage)
    {
        if (!isDead)
        {
            // Hit
            CurHp -= damage;
            animator.Play("Take Hit");
            Invoke("HitAnim", 0.8f);
            getDamage = true;

            // Dead
            if (CurHp <= 0)
            {
                animator.Play("Death");
                isDead = true;
                transform.parent.GetComponent<EnemyManager>().AddCount();
            }
        }
    }

    private void Move()
    {
        // ����Լ�, �̵� �ݺ�
        nextPos = Random.Range(-1, 2);

        Invoke("Move", 5);
    }

    // �´� ���� �ٸ� �ִϸ��̼� ��� X
    void HitAnim()
    {
        getDamage = false;
    }
}
