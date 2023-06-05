using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    Animator anim;                           // �ִϸ����� ������Ʈ
    Rigidbody2D rigid2D;                   // Rigidbody2D ������Ʈ
    public float maxHealth = 40f;         // ������ �ִ� ü��
    private float currentHealth;          // ������ ���� ü��
    public Controller player;               // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    public SlimeMove move;              // ������ �̵� ��ũ��Ʈ
    public LayerMask isLayer;            // �浹 üũ ���̾�

    public bool isGround;                  // ���� ���� �������� ����
    public bool isJump = false;         // ���� ������ ����
    public bool isKnockback = false; // �˹� ������ ����
    public bool isDamaged = false;   // �ǰ� �������� ����

    public Transform pos;             // ���� ���� ���� ��ġ
    public Vector2 boxSize;         // ���� ���� �ڽ� ũ��
    public float JumpForce = 500f;  // ���� ��
    private bool canJump = true;     // ���� ���� ����

    private Transform playerTransform;   // �÷��̾��� ��ġ�� �����ϱ� ���� ����
    private Transform slimeTransform;       // �������� ��ġ�� �����ϱ� ���� ����

    public SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ������Ʈ

    Color halfA = new Color(1, 1, 1, 0.5f);  // ���� �ݰ��� ��
    Color fullA = new Color(1, 1, 1, 1);    // ���� ��ü�� ��


    public void Start()
    {
        anim = GetComponent<Animator>();   // �ִϸ����� ������Ʈ ��������
        rigid2D = GetComponent<Rigidbody2D>();  // Rigidbody2D ������Ʈ ��������
        currentHealth = maxHealth;    // ������ ü���� �ִ� ü������ �ʱ�ȭ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �÷��̾��� Transform ������Ʈ ��������
        player = FindObjectOfType<Controller>();  // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ ��������
        move = FindObjectOfType<SlimeMove>();  // ������ �̵� ��ũ��Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ������Ʈ ��������
        slimeTransform = transform; // �������� ��ġ ����
    }


    public void FixedUpdate()
    {
        // ���� üũ
        Vector2 groundCheckPos = transform.position - new Vector3(0, 0.5f, 0);  // �������� �Ʒ��� ������ üũ
        RaycastHit2D groundRaycast = Physics2D.Raycast(groundCheckPos, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        isGround = groundRaycast.collider != null;  // ����� �浹�ߴ��� ���θ� isGround ������ �Ҵ�
         

        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);  // �浹 �ڽ� ���� �ݶ��̴� ��������
        bool playerInRange = false;  // �÷��̾ �������� ���� ���� �ִ��� ����

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                playerInRange = true;  // �÷��̾ �������� �ݶ��̴� ���� �ִٸ� true�� ����
            }
        }
        // ���� �������� ���� �ְ�, �÷��̾ �������� ���� ���� ���� �ִٸ�
        if (isGround && playerInRange && !isJump && !isKnockback && !isDamaged && canJump)
        {
            Debug.Log("�����Ѵ�!");
            
            // �÷��̾��� ������ ���� ����
            JumpTowards();

            canJump = false;  // ���� �� ������ ���� ���� canJump ������ false�� ����
            StartCoroutine(EnableJump());  // 1�� �Ŀ� �ٽ� ������ �����ϰ� ���ִ� �ڷ�ƾ ����
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize); // Gizmos�� ���� ���� ���� �浹 �ڽ��� �׷��ִ� �Լ�
    }

    void JumpTowards()
    {
        isJump = true;
        anim.SetBool("Jump", true); // ���� �ִϸ��̼� ����� ���� �ִϸ��̼� ������ true�� ����
        StartCoroutine(Jump());  // ���� �ڷ�ƾ ����
    }

    void EndJumpAnimation()   // ���� �ִϸ��̼� ���� ó�� �Լ�
    {
        isJump = false;
        anim.SetBool("Jump", false);  // �ִϸ��̼� ������ false�� ����
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�����Ӱ� �浹");
            playerTransform.GetComponent<Controller>().Damaged(4f, slimeTransform.position); // �����Ӱ� �浹�ϸ� �÷��̾�� �������� ������ �޼��� ȣ��
            isJump = false;
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
                StartCoroutine(damagedRoutine()); // ���� ó���� ���� �ڷ�ƾ ����
                StartCoroutine(alphaBlink());       // ������ ȿ���� ���� �ڷ�ƾ ����
            }
        }
    }

    private void Die()
    {
        anim.SetBool("Death", true); // ���Ͱ� ���� ���� ������ ����
    }

    public void removeslime()
    {
        Destroy(gameObject);   // ���� ������Ʈ �ı�
    }

    IEnumerator Jump()
    {
        float jumpTime = JumpForce / 100f; // ���� �ִϸ��̼� ��� �ð� ���
        float elapsedTime = 0f;

        // �÷��̾�� �������� ��ġ ���� ���
        float playerDirection = playerTransform.position.x - slimeTransform.position.x;

        // �÷��̾��� ��ġ�� ���� ���� ���� ����
        float jumpDirection = (playerDirection < 0) ? -1f : 1f;



        while (elapsedTime < jumpTime)
        {
            float jumpProgress = elapsedTime / jumpTime;
            float jumpForce = Mathf.Lerp(JumpForce, 0f, jumpProgress); // �ʱ� ���� ������ 0���� ���� ����

            move.moveDir = (playerTransform.position.x > slimeTransform.position.x) ? 3 : -3;

            // �÷��̾� ��ġ�� ���� ���� ���� ����
            rigid2D.velocity = new Vector2(jumpDirection * rigid2D.velocity.x, jumpForce);

            elapsedTime += Time.deltaTime * 0.5f;
            yield return null;
        }
    }

    IEnumerator EnableJump()
    {
        yield return new WaitForSeconds(1f);  // 1�� ���
        canJump = true;  // ���� �����ϵ��� canJump ������ true�� ����
    }

    IEnumerator KnockBack(float dir)
    {
        isKnockback = true;  // �˹� ������ ��Ÿ���� ������ true�� ����
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
        yield return new WaitForSeconds(2f);
        isDamaged = false;
    }

    IEnumerator alphaBlink()
    {
        while (isDamaged)
        {
            yield return new WaitForSeconds(0.1f); // ���� �ݰ��� ������ ����
            spriteRenderer.color = halfA;
            yield return new WaitForSeconds(0.1f);  // ���� ��ü�� ������ ����
            spriteRenderer.color = fullA;
        }
    }
}
