using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform target;
    SpriteRenderer spriteRenderer; // SpriteRenderer ������Ʈ�� �����ϱ� ���� ����

    [SerializeField] [Range(1f, 4f)] float moveSpeed = 1f;
    [SerializeField] [Range(0f, 3f)] float contactDistance = 0.5f;
    public bool follow = false;

    public GameObject batAttack;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        batAttack = GameObject.Find("batAttack");
    }

    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (Vector2.Distance(transform.position, target.position) > contactDistance && follow)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // �÷��̾ �����ʿ� ���� �� FlipX ���� false�� ����
            if (target.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
                batAttack.transform.localPosition = new Vector2(Mathf.Abs(batAttack.transform.localPosition.x), batAttack.transform.localPosition.y);
            }
            
            // �÷��̾ ���ʿ� ���� �� FlipX ���� true�� ����
            else if (target.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                batAttack.transform.localPosition = new Vector2(-Mathf.Abs(batAttack.transform.localPosition.x), batAttack.transform.localPosition.y);
            }
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }
}