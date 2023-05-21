using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    Animator anim;
    public int maxHealth = 10;         // ������ �ִ� ü��
    private int currentHealth;          // ������ ���� ü��
    public Controller player;
    public SlimeMove move;

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
    }


    public void Update()
    {
        
    }

    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�����Ӱ� �浹");
            playerTransform.GetComponent<Controller>().Damaged(3f, slimeTransform.position); // �÷��̾�� �������� ������ �޼��� ȣ��
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (player.isAttack == true)
        {
            currentHealth -= damageAmount;   // ���� ü�¿��� ���ط��� ���ҽ�Ŵ
            anim.SetTrigger("Hurt");
        }
        if (currentHealth <= 0)
        {
            Die();    // ������ ü���� 0 ���ϸ� ���� ó��
        }
    }

    private void Die()
    {
        // ���Ͱ� ���� ���� ������ ����
        anim.SetBool("Death", true);
        Destroy(gameObject);   // ���� ������Ʈ �ı�
    }
}