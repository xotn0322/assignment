using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    public float maxHealth = 10f;         // ������ �ִ� ü��
    private float currentHealth;          // ������ ���� ü��
    public Controller player;              // �÷��̾� ��Ʈ�ѷ� ����
    public float distance;                 // ���� ���� �Ÿ�
    public float atkDistance;             // �� ���� ���� �Ÿ�
    public LayerMask isLayer;         // ���� ��� ���̾� ����ũ
    Animator anim;                            // �ִϸ����� ������Ʈ ����
    Rigidbody2D rigid2D;                  // ������ٵ�2D ������Ʈ ����
    public Transform pos;                 // ���� ������ ��Ÿ���� ���� ��ġ
    public Vector2 boxSize;               // ���� ���� ���� ũ��
    public BoxCollider2D box;             // ���� ���� ���� ������Ʈ

    public float attackCooldown = 0.0001f; // ���� ��ٿ� �ð�
    private float currentCooldown; // ���� ��ٿ� �ð�

    public GameObject bullet;         // �� �߻� ������Ʈ
    public Transform bulletpos;         // �� �߻� ��ġ
    public MushroomMove move;   // ���� �̵� ��ũ��Ʈ ����

    private Transform playerTransform;   // �÷��̾��� Ʈ������
    private Transform mushroomTransform;  // ������ Ʈ������

    public bool isAttack = false;                   // ���� ���� ������ ����
    public bool isPoisonAttack = false;         // ���� �� ���� ������ ����
    public bool isKnockback = false;           // ���� �˹� �������� ����
    public bool isDamaged = false;            // ���� ���ظ� ���� �������� ����
    public bool isMovingRight = false;        // ���������� �̵� ������ ����

    public SpriteRenderer spriteRenderer;   // ��������Ʈ ������ ������Ʈ
    Color halfA = new Color(1, 1, 1, 0.5f);  // ���� �ݰ��� ��
    Color fullA = new Color(1, 1, 1, 1);    // ���� 1�� ��

    void Start()
    {
        anim = GetComponent<Animator>();                     // �ִϸ����� ������Ʈ ����
        rigid2D = GetComponent<Rigidbody2D>();           // ������ٵ�2D ������Ʈ ����
        move = FindObjectOfType<MushroomMove>();      // MushroomMove ��ũ��Ʈ�� ã�Ƽ� ����
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // "Player" �±׸� ���� ������Ʈ�� Ʈ������ ����
        spriteRenderer = GetComponent<SpriteRenderer>();        // ��������Ʈ ������ ������Ʈ ����
        player = FindObjectOfType<Controller>();                       // Controller ��ũ��Ʈ�� ã�Ƽ� ����
        mushroomTransform = transform;                                 // ������ Ʈ������ ����
    }

    public float cooltime;                           // ���� ��Ÿ��
    private float currenttime;                   // ���� �ð�

    void FixedUpdate()
    {
        SetMovingRight(); // �� �߻� ���� ����

        RaycastHit2D raycast;
        if (isMovingRight)
        {
            raycast = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);    // ������ �������� �Ÿ�(distance)��ŭ ����ĳ��Ʈ�� �߻��ϰ� �浹 ������ raycast ������ ����
        }
        else
        {
            raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);    // ���� �������� �Ÿ�(distance)��ŭ ����ĳ��Ʈ�� �߻��ϰ� �浹 ������ raycast ������ ����
        }
        if (raycast.collider!=null)
        {
            if (Vector2.Distance(transform.position, raycast.collider.transform.position) < atkDistance)
            {
                if (currenttime <= 0)
                {
                    GameObject bulletcopy = Instantiate(bullet, bulletpos.position, Quaternion.Euler(0f, 0f, isMovingRight ? 0f : 180f));   // �� �߻� ������Ʈ�� bulletpos ��ġ�� �����ϰ�, �¿� ������ �����ϱ� ���� ���ʹϾ����� ȸ�� ������ ����
                    move.moveDir = 0;
                    anim.SetInteger("WalkSpeed", move.moveDir);
                    anim.SetBool("Poison", true);
                    isPoisonAttack = true;
                    currenttime = cooltime;         // �� ���� ��Ÿ�� ����
                }
            }

            currenttime -= Time.deltaTime;
        }

        if (!isAttack&& !isKnockback)
        {
            if (currentCooldown <= 0)
            {
                AttackPlayer();                          // �÷��̾ �����ϴ� �޼��� ȣ��
                currentCooldown = attackCooldown;
            }
            else if (currentCooldown > 0 && !anim.GetBool("Attack"))
            {
                currentCooldown -= Time.deltaTime;
            }
        }
        else
        {
            spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);  // �÷��̾��� ��ġ�� ���� ��������Ʈ �¿� ����
        }
    }

    public void FinishPoisonAttackAnimation()
    {
        anim.SetBool("Poison", false);
        isPoisonAttack = false; // �� ���� ���� �� ���� ����
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);  // �÷��̾��� ��ġ�� ���� ��������Ʈ �¿� ����
    }

    void SetMovingRight()
    {
        Vector3 playerPos = playerTransform.position;
        if (playerPos.x > transform.position.x)
        {
            isMovingRight = true;
            bulletpos.localScale = new Vector3(1, 1, 1); // �� �߻� ��ġ�� �������� ������� ���� (������ ����)
        }
        else
        {
            isMovingRight = false;
            bulletpos.localScale = new Vector3(-1, 1, 1); // �� �߻� ��ġ�� �������� ����� ���� (���� ����)
        }
    }

    public void AttackPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0); // pos ��ġ�� �������� boxSize ũ���� �ڽ� �ݶ��̴��� ��ġ�� ��� �ݶ��̴��� ������
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                move.moveDir = 0;
                anim.SetInteger("WalkSpeed", move.moveDir);
                anim.SetBool("Attack", true);  // �ִϸ��̼� ���� ����
                isAttack = true;                   // ���� �� ���·� ����
                break;
            }
        }
    }
    
    public void FinishAttackAnimation()
    {
        anim.SetBool("Attack", false);  // ���� �ִϸ��̼� ����
        isAttack = false; // ���� ���� �� ���� ����
        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);   // �÷��̾��� ��ġ�� ���� ��������Ʈ �¿� ����
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);     // ���� ���� ���ڸ� �������� boxSize ũ���� ����� ���̾� ť�긦 �׸�
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
            Debug.Log("������ �浹");
            playerTransform.GetComponent<Controller>().Damaged(3f, mushroomTransform.position); // ������ �浹�ϸ� �÷��̾�� �������� ������ �޼��� ȣ��
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
                Die();                                          // ������ ü���� 0 ���ϸ� ���� ó��
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
        anim.SetBool("Death", true);  // ���Ͱ� ���� ���� ������ ����
    }

    public void removemushroom()
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
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }

    IEnumerator alphaBlink()
    {
        while (isDamaged)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = halfA;  // ������ ���·� ����
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = fullA;   // ������ ���·� ����
    }
    }
}
