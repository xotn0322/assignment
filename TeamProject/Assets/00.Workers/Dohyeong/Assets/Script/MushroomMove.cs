using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMove : MonoBehaviour
{
    public Rigidbody2D rigid;  // Rigidbody2D ������Ʈ�� �����ϱ� ���� ����
    public Animator anim;     // Animator ������Ʈ�� �����ϱ� ���� ����
    SpriteRenderer spriteRenderer;  // SpriteRenderer ������Ʈ�� �����ϱ� ���� ����
    public int moveDir;                     // �̵� ������ �����ϱ� ���� ����
    public float nextThinkTime;        // ���� �Ǵ� �ð��� �����ϱ� ���� ����
    public MushroomController controller; // MushroomController ��ũ��Ʈ�� �����ϱ� ���� ����

    public GameObject mushroomAttack;  // mushroomAttack ���� ������Ʈ�� �����ϱ� ���� ����
    public GameObject ShootPoison;  // ShootPoison ���� ������Ʈ�� �����ϱ� ���� ����


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ�� �����ͼ� ������ ����
        anim = GetComponent<Animator>();      // Animator ������Ʈ�� �����ͼ� ������ ����
        spriteRenderer = GetComponent<SpriteRenderer>();    //SpriteRenderer ������Ʈ�� �����ͼ� ������ ����
        mushroomAttack = GameObject.Find("mushroomAttack");  // "mushroomAttack" ������Ʈ�� ã�Ƽ� ������ ����
        ShootPoison = GameObject.Find("ShootPoison");       // "ShootPoison" ������Ʈ�� ã�Ƽ� ������ ����
        controller = FindObjectOfType<MushroomController>(); // MushroomController ��ũ��Ʈ�� ã�Ƽ� ������ ����
        StartCoroutine("monsterAI");                                       // monsterAI �ڷ�ƾ�� ����
    }


    void FixedUpdate()
    {
        if(!controller.isKnockback)   // �˹� ���� �ƴ� ��쿡�� ����
        {
            //Move
            rigid.velocity = new Vector2(moveDir, rigid.velocity.y);    // Rigidbody2D�� �ӵ��� �����Ͽ� �̵�
            if (rigid.velocity.x > 0.1f)
            {
                spriteRenderer.flipX = false;    // �ӵ��� ���(������ �̵�)�� �� ��������Ʈ�� �¿� �������� ����
            }
            else
            {
                spriteRenderer.flipX = true;    // �ӵ��� ����(���� �̵�)�� �� ��������Ʈ�� �¿� ����
            }

            //���� üũ
            //���ʹ� ���� üũ�ؾ� 
            Vector2 frontVec = new Vector2(rigid.position.x + moveDir*0.5f, rigid.position.y - 0.5f);   // �̵� ������ ���� üũ�ϱ� ���� ����ĳ��Ʈ�� ���� ��ġ�� ����
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));           // ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� �׷���

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground")); // ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� �׷���

            if (rayHit.collider == null)
            {
                Debug.Log("���! �� �� ����������!");   
                Turn();                                          // ������ �ݴ�� ��ȯ
            }

            if (moveDir > 0)
            {
                mushroomAttack.transform.localPosition = new Vector2(Mathf.Abs(mushroomAttack.transform.localPosition.x), mushroomAttack.transform.localPosition.y);  // mushroomAttack�� ���� �������� �����Ͽ� ���� ��ȯ
                ShootPoison.transform.localPosition = new Vector2(Mathf.Abs(ShootPoison.transform.localPosition.x), ShootPoison.transform.localPosition.y);  // ShootPoison�� ���� �������� �����Ͽ� ���� ��ȯ
            }
            else if (moveDir < 0)
            {
                mushroomAttack.transform.localPosition = new Vector2(-Mathf.Abs(mushroomAttack.transform.localPosition.x), mushroomAttack.transform.localPosition.y);  // mushroomAttack�� ���� �������� �����Ͽ� ���� ��ȯ
                ShootPoison.transform.localPosition = new Vector2(-Mathf.Abs(ShootPoison.transform.localPosition.x), ShootPoison.transform.localPosition.y);                     // ShootPoison�� ���� �������� �����Ͽ� ���� ��ȯ
            }

            if (controller.isAttack || controller.isKnockback || controller.isDamaged) // ���� ���̰ų� �˹� ���̰ų� ���ظ� �ް� �ִ� ���
            {
                moveDir = 0;                                     // �̵� ������ 0���� �����Ͽ� ����
                anim.SetInteger("WalkSpeed", moveDir);   // Animator ������Ʈ�� WalkSpeed �Ķ���͸� �����Ͽ� �ִϸ��̼� ����� ������Ŵ
            }
        }
    }

    IEnumerator monsterAI()
    {
        if (!controller.isKnockback) // �˹� ���� �ƴ� ��쿡�� ����
        {
            moveDir = Random.Range(-1, 2);   // -1, 0, 1 �߿��� �����ϰ� �̵� ���� ����
            anim.SetInteger("WalkSpeed", moveDir);   // Animator ������Ʈ�� WalkSpeed �Ķ���͸� �����Ͽ� �̵� �ִϸ��̼� ���
        }
        nextThinkTime = Random.Range(2f, 5f);     // ���� �Ǵ� �ð��� �����ϰ� ����
        yield return new WaitForSeconds(nextThinkTime);  // ���� �ð� ���� ���
        StartCoroutine("monsterAI");     // monsterAI �ڷ�ƾ �����
    }

    void Turn()
    {
        moveDir = moveDir * (-1);   // �̵� ������ �ݴ�� ����
        CancelInvoke();                // ������ ����� Invoke�� ���
    }

    public void startMove()
    {
        StartCoroutine("monsterAI");   // monsterAI �ڷ�ƾ�� ����
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI");   // monsterAI �ڷ�ƾ�� ����
    }
}
