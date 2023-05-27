using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    Animator anim;
    public float maxHealth = 10f;         // ������ �ִ� ü��
    private float currentHealth;          // ������ ���� ü��
    public Controller player;
    public BatMove move;
    public Transform pos;
    public Vector2 boxSize;
    public BoxCollider2D box;

    private Transform playerTransform;   // �÷��̾��� ��ġ�� �����ϱ� ���� ����
    private Transform batTransform;



    public bool isAttack = false;
    public float attackCooldown = 0.001f; // ���� ��ٿ� �ð�
    private float currentCooldown = 0f; // ���� ��ٿ� �ð�

    public SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ������Ʈ


    public void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;    // ������ ü���� �ִ� ü������ �ʱ�ȭ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �÷��̾��� Transform ������Ʈ ��������
        player = FindObjectOfType<Controller>();
        move = FindObjectOfType<BatMove>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ������Ʈ ��������
        batTransform = transform;
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
            // ���� ���� �� FlipX�� �������� �ʵ��� ����
            spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);
        }
    }

    public void AttackPlayer()
    {
        // �÷��̾�� ���ظ� ������ ���� ����
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                anim.SetBool("Attack", true);
                isAttack = true; // ���� �� ���·� ����
                break;
            }
        }
    }

    public void FinishAttackAnimation()
    {
        anim.SetBool("Attack", false);
        isAttack = false; // ���� ���� �� ���� ����
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    // BoxCollider2D�� Ȱ��ȭ�ϴ� �޼���
    public void EnableBoxCollider()
    {
        box.enabled = true;
    }

    // BoxCollider2D�� ��Ȱ��ȭ�ϴ� �޼���
    public void DisableBoxCollider()
    {
        box.enabled = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("����� �浹");
            playerTransform.GetComponent<Controller>().Damaged(1f, batTransform.position); // �÷��̾�� �������� ������ �޼��� ȣ��
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;   // ���� ü�¿��� ���ط��� ���ҽ�Ŵ
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();    // ������ ü���� 0 ���ϸ� ���� ó��
        }
    }

    private void Die()
    {
        // ���Ͱ� ���� ���� ������ ����
        anim.SetBool("Death", true);
    }

    public void removebat()
    {
        Destroy(gameObject);   // ���� ������Ʈ �ı�
    }
}

