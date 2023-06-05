using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    Animator anim;                          // �ִϸ����� ������Ʈ
    Rigidbody2D rigid2D;              // ������ٵ� ������Ʈ
    public float maxHealth = 10f;         // ������ �ִ� ü��
    private float currentHealth;          // ������ ���� ü��
    public Controller player;              // �÷��̾� ��ũ��Ʈ
    public BatMove move;               // BatMove ��ũ��Ʈ
    public Transform pos;                // ���ݹ��� ���� ��ġ
    public Vector2 boxSize;             // ���ݹ��� ���� �ڽ� ũ��
    public BoxCollider2D box;         // �ڽ� �ݶ��̴�

    private Transform playerTransform;   // �÷��̾��� ��ġ�� �����ϱ� ���� ����
    private Transform batTransform;     // ������ ��ġ�� �����ϱ� ���� ����



    public bool isAttack = false;  // ���� ������ ���θ� ��Ÿ���� ����
    public bool isKnockback = false; // �˹� ������ ���θ� ��Ÿ���� ����
    public bool isDamaged = false;  // �ǰ� �������� ���θ� ��Ÿ���� ����
    public float attackCooldown = 0.001f; // ���� ��ٿ� �ð�
    private float currentCooldown = 0f; // ���� ��ٿ� �ð�

    public SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ������Ʈ

    Color halfA = new Color(1, 1, 1, 0.5f);  // �������� ����
    Color fullA = new Color(1, 1, 1, 1);    // ������ ����



    public void Start()
    {
        anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
        rigid2D = GetComponent<Rigidbody2D>(); // ������ٵ� ������Ʈ ��������
        currentHealth = maxHealth;    // ������ ü���� �ִ� ü������ �ʱ�ȭ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �÷��̾��� Transform ������Ʈ ��������
        player = FindObjectOfType<Controller>(); // Controller ��ũ��Ʈ ��������
        move = FindObjectOfType<BatMove>();  // BatMove ��ũ��Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ������Ʈ ��������
        batTransform = transform; // ������ ��ġ ����
    }


    public void Update()
    {
        if (!isAttack&&!isKnockback)
        {
            if (currentCooldown <= 0)
            {
                AttackPlayer();   // �÷��̾� ���� �޼��� ȣ��
                currentCooldown = attackCooldown; // ��ٿ� �ʱ�ȭ
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
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                anim.SetBool("Attack", true); // �ִϸ��̼� ���� ����
                isAttack = true; // ���� �� ���·� ����
                break;
            }
        }
    }

    public void FinishAttackAnimation()
    {
        anim.SetBool("Attack", false);  // ���� �ִϸ��̼� ����
        isAttack = false; // ���� ���� �� ���� ����
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize); // ���� ���� �ݶ��̴� ������ ���� �ֵ���  �׸���
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
            playerTransform.GetComponent<Controller>().Damaged(3f, batTransform.position); // ����� �浹�ϸ� �÷��̾�� �������� ������ �޼��� ȣ��
        }
    }

    public void TakeDamage(float damageAmount, Vector2 playerpos)
    {
        if (!isDamaged)
        {
            isDamaged = true;
            currentHealth -= damageAmount;   // ���� ü�¿��� ���ط��� ���ҽ�Ŵ

            if (currentHealth <= 0)
            {
                Die();    // ������ ü���� 0 ���ϸ� ���� ó��
            }
            else
            {
                anim.SetTrigger("Hurt"); // �ǰ� �ִϸ��̼� ���
                // �÷��̾��� ��ġ�� ���� �˹� ���� ����
                float knockbackDirection = transform.position.x - playerpos.x;
                if (knockbackDirection < 0)
                    knockbackDirection = -1f;
                else
                    knockbackDirection = 1f;


                StartCoroutine(KnockBack(knockbackDirection)); // �˹� �ڷ�ƾ ����
                StartCoroutine(damagedRoutine());  // �ǰ� ���� ���� �ڷ�ƾ ����
                StartCoroutine(alphaBlink());   // �ǰ� ȿ���� ���� ������ �ڷ�ƾ ����
            }
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

    IEnumerator KnockBack(float dir)
    {
        isKnockback = true;
        float elapsedTime = 0;
        while (elapsedTime < 0.2f) //�˹�ð�
        {
            if (rigid2D.velocity.y == 0)
                rigid2D.velocity = new Vector2(2f * dir, 0);
            else
                rigid2D.velocity = new Vector2(2f * dir, 2f * dir);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
    }

    IEnumerator damagedRoutine()
    {
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }

    IEnumerator alphaBlink()
    {
        while (isDamaged)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = halfA; // ������ ���·� ����
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = fullA;  // ������ ���·� ����
        }
    }
}
