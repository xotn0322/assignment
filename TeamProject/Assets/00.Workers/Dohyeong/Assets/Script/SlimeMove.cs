using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    public Rigidbody2D rigid;  // Rigidbody2D ������Ʈ�� ���� ����
    public Animator anim;        // Animator ������Ʈ�� ���� ����
    SpriteRenderer spriteRenderer;   // SpriteRenderer ������Ʈ�� ���� ����
    public int moveDir;                    // �̵� ������ ��Ÿ���� ����
    public float nextThinkTime;          // ���� AI �Ǵ� �ð��� ��Ÿ���� ����
    public SlimeController controller;  // SlimeController ��ũ��Ʈ�� �����ϴ� ����


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    // Rigidbody2D ������Ʈ�� �����ͼ� ������ ����
        anim = GetComponent<Animator>();        // Animator ������Ʈ�� �����ͼ� ������ ����
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer ������Ʈ�� �����ͼ� ������ ����
        controller = FindObjectOfType<SlimeController>();    // SlimeController ��ũ��Ʈ�� ã�� ������ ����
        StartCoroutine("monsterAI");                             // monsterAI �ڷ�ƾ�� ����
    }


    void FixedUpdate()
    {
        if(!controller.isJump||!controller.isKnockback)
        {
            // �̵�
            rigid.velocity = new Vector2(moveDir, rigid.velocity.y);
            if (rigid.velocity.x > 0.1f)
            {
                spriteRenderer.flipX = false;  // ���������� �̵� ���� ��� ��������Ʈ�� �������� ����
            }
            else
            {
                spriteRenderer.flipX = true;  // �������� �̵� ���� ��� ��������Ʈ�� ������Ŵ
            }

            //���� üũ
            //���ʹ� ���� üũ�ؾ� 
            Vector2 frontVec = new Vector2(rigid.position.x + moveDir*0.5f, rigid.position.y - 0.5f); // �̵� ������ ���� üũ�ϱ� ���� ����ĳ��Ʈ�� ���� ��ġ�� ����
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));  // ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� �׷���
            // ����,���� ����

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� �׷���

            if (rayHit.collider == null)
            {
                Debug.Log("���! �� �� ����������!");
                Turn();  // ������ �ݴ�� ��ȯ
            }


            if (controller.isKnockback || controller.isDamaged) 
            {
                moveDir = 0; // ���� ���̰ų� �˹� ���̶�� moveDir ���� �������� ����
            }
        }
    }

    IEnumerator monsterAI()
    { 
        moveDir = Random.Range(-1, 2);  // -1, 0, 1 �߿��� �����ϰ� �̵� ������ ����
        nextThinkTime = Random.Range(2f, 5f);  // ���� �Ǵ� �ð��� 2~5 ������ ���� ������ ����
        yield return new WaitForSeconds(nextThinkTime);  // ���� �ð� ���� ���
        StartCoroutine("monsterAI");    // ���� AI �ڷ�ƾ �����
    }

    void Turn()
    {
        moveDir = moveDir * (-1);  // �̵� ������ ������Ŵ
        CancelInvoke();              // Invoke�� ���
    }

    public void startMove()
    {
        StartCoroutine("monsterAI");  // monsterAI �ڷ�ƾ�� ����
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI");  // monsterAI �ڷ�ƾ�� ����
    }
}
