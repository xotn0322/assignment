using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMove : MonoBehaviour
{
    public Rigidbody2D rigid; // ������ Rigidbody2D ������Ʈ�� ���� ����
    public Animator anim;    // ������ Animator ������Ʈ�� ���� ����
    SpriteRenderer spriteRenderer;  // ������ SpriteRenderer ������Ʈ�� ���� ����
    public int moveDir;               // ������ �̵� ���� (-1: ����, 0: ����, 1: ������)
    public float nextThinkTime;   // ���� ������ �ϴ� �ð� ����
    public SkeletonController controller;  // SkeletonController ��ũ��Ʈ�� ���� ����

    public GameObject skeletonAttack; // ���̷��� ������ skeletonAttack �ڽ� ������Ʈ�� ���� ����


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();  // Rigidbody2D ������Ʈ ��������
        anim = GetComponent<Animator>();     // Animator ������Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>();   // SpriteRenderer ������Ʈ ��������
        skeletonAttack = GameObject.Find("skeletonAttack");  // skeletonAttack ������Ʈ ã�� ��������
        controller = FindObjectOfType<SkeletonController>();  // SkeletonController ��ũ��Ʈ ��������
        StartCoroutine("monsterAI");  // ���� AI �ڷ�ƾ ����
    }


    void FixedUpdate()
    {
        if(!controller.isKnockback)  // �˹� ���� �ƴ� ��쿡�� ����
        {
            // �̵�
            rigid.velocity = new Vector2(moveDir, rigid.velocity.y);
            if (rigid.velocity.x > 0.1f)
            {
                spriteRenderer.flipX = false; // ���������� �̵��ϰ� ���� �� ��������Ʈ �������� ����
            }
            else
            {
                spriteRenderer.flipX = true; // �������� �̵��ϰ� ���� �� ��������Ʈ ����
            }

            //���� üũ
            //���ʹ� ���� üũ�ؾ�
            Vector2 frontVec = new Vector2(rigid.position.x + moveDir*0.5f, rigid.position.y - 0.5f);  // �̵� ������ ���� üũ�ϱ� ���� ����ĳ��Ʈ�� ���� ��ġ�� ����
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));            // ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� �׷���


            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� �׷���

            if (rayHit.collider == null)
            {
                Debug.Log("���! �� �� ����������!");
                Turn();   // ���� ��ȯ
            }


            // skeletonAttack �ڽ� ������Ʈ ���� ����
            if (moveDir > 0)
            {
                skeletonAttack.transform.localPosition = new Vector2(Mathf.Abs(skeletonAttack.transform.localPosition.x), skeletonAttack.transform.localPosition.y); // skeletonAttack�� ���� �������� �����Ͽ� ���� ��ȯ
            }
            else if (moveDir < 0)
            {
                skeletonAttack.transform.localPosition = new Vector2(-Mathf.Abs(skeletonAttack.transform.localPosition.x), skeletonAttack.transform.localPosition.y); // skeletonAttack�� ���� �������� �����Ͽ� ���� ��ȯ
            }

            if (controller.isAttack || controller.isKnockback||controller.isDamaged) // ���� ���̰ų� �˹� ���̰ų� �ǰ� ������ ��� moveDir ���� �������� ����
            {
                moveDir = 0;  // ����
                anim.SetInteger("WalkSpeed", moveDir); // Animator�� �̵� �ӵ� �� ����
            }
        }
    }

    IEnumerator monsterAI()
    {
        if (!controller.isKnockback) // �˹� ���� �ƴ� ��쿡�� moveDir ���� ����
        {
            moveDir = Random.Range(-1, 2);  // -1, 0, 1 �߿��� �����ϰ� �̵� ���� ����
            anim.SetInteger("WalkSpeed", moveDir);  // Animator�� �̵� �ӵ� �� ����
        }
        nextThinkTime = Random.Range(2f, 5f);   // ���� ������ �ϴ� �ð� ������ 2�ʿ��� 5�� ���̷� �����ϰ� ����
        yield return new WaitForSeconds(nextThinkTime);
        StartCoroutine("monsterAI");  // ���� AI �ڷ�ƾ�� ��������� ȣ��
    }

    void Turn()
    {
        moveDir = moveDir * (-1); // �̵� ���� ����
        CancelInvoke();  // ���� Invoke ���
    }

    public void startMove()
    {
        StartCoroutine("monsterAI"); // ���� AI �ڷ�ƾ ����
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI"); // ���� AI �ڷ�ƾ ����
    }
}
