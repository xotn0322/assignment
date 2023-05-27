using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    Animator anim;
    public float maxHealth = 10f;         // ������ �ִ� ü��
    private float currentHealth;          // ������ ���� ü��
    public Controller player;
    public SlimeMove move;
    public LayerMask isLayer;
    public bool isGround;
    public bool isJump = false;
    private Rigidbody2D slimeRigidbody;  // Rigidbody2D ������Ʈ�� �����ϴ� ����
    public Transform pos;
    public Vector2 boxSize;
    public float JumpForce = 210.0f;

    private Transform playerTransform;   // �÷��̾��� ��ġ�� �����ϱ� ���� ����
    private Transform slimeTransform;

    public SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ������Ʈ


    public void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;    // ������ ü���� �ִ� ü������ �ʱ�ȭ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �÷��̾��� Transform ������Ʈ ��������
        player = FindObjectOfType<Controller>();
        move = FindObjectOfType<SlimeMove>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ������Ʈ ��������
        slimeTransform = transform;
        slimeRigidbody = GetComponent<Rigidbody2D>();  // Rigidbody2D ������Ʈ ��������
    }


    public void Update()
    {
        // �������� ���� �ִ��� üũ
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.2f), 0.35f, isLayer);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                // ���� �������� ���� �ְ�, �÷��̾ �������� ���� ���� ���� �ִٸ�
                if (isGround == true)
                {
                    isJump = true;
                    // �÷��̾��� ������ ���� ����
                    JumpTowards();
                }
                anim.SetBool("Jump", !isGround);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void JumpTowards()
    {
        anim.SetBool("Jump", true);
        // ���� ���� ����
        slimeRigidbody.AddForce(transform.up * JumpForce);
        // ���� �ð� �Ŀ� ���� �ִϸ��̼� ����
        StartCoroutine(EndJumpAnimation());
    }

    IEnumerator EndJumpAnimation()
    {
        // ���� �ð� ���
        yield return new WaitForSeconds(0.5f);  // ���� �ִϸ��̼� ��� �ð��� �˸°� ����
        // �������� �ٴڿ� ������ �Ŀ� ���� �ִϸ��̼� ����
        anim.SetBool("Jump", false);
        isJump = false;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�����Ӱ� �浹");
            playerTransform.GetComponent<Controller>().Damaged(3f, slimeTransform.position); // �÷��̾�� �������� ������ �޼��� ȣ��
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

    public void removeslime()
    {
        Destroy(gameObject);   // ���� ������Ʈ �ı�
    }
}