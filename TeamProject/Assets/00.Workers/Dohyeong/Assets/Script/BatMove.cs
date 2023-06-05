using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMove : MonoBehaviour
{
    Rigidbody2D rigid; // Rigidbody2D ������Ʈ�� �����ϱ� ���� ����
    Transform target;  // ������ ����� Transform ������Ʈ�� �����ϱ� ���� ����
    SpriteRenderer spriteRenderer; // SpriteRenderer ������Ʈ�� �����ϱ� ���� ����

    [SerializeField] [Range(0f, 4f)] float moveSpeed = 1f; // �̵� �ӵ��� �����ϱ� ���� ����
    [SerializeField] [Range(0f, 3f)] float contactDistance = 0.5f; // ������ ���� �Ÿ��� �����ϱ� ���� ����
    public bool follow = false;  // ����� �������� ���θ� �����ϴ� ����

    public GameObject batAttack; // ���� ������Ʈ�� �����ϱ� ���� ����
    public BatController controller; // BatController ��ũ��Ʈ�� �����ϱ� ���� ����

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // ���� ���� ������Ʈ�� ����� Rigidbody2D ������Ʈ�� ������
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // "Player" �±׸� ���� ���� ������Ʈ�� Transform ������Ʈ�� ������
        spriteRenderer = GetComponent<SpriteRenderer>(); // ���� ���� ������Ʈ�� ����� SpriteRenderer ������Ʈ�� ������
        batAttack = GameObject.Find("batAttack");   // "batAttack" �̸��� ���� ������Ʈ�� ã�Ƽ� ������
        controller = FindObjectOfType<BatController>(); // Scene���� BatController ��ũ��Ʈ�� ã�� ������
    }

    void Update()
    {
        if (!controller.isKnockback)  // BatController�� isKnockback ������ false�� ���� ����
        {
            FollowTarget();   // ����� �����ϴ� �Լ� ȣ��
        }
    }

    void FollowTarget()
    {
        if (Vector2.Distance(transform.position, target.position) > contactDistance && follow)
        {
            // ���� ��ġ�� ��� ��ġ ������ �Ÿ��� ���� �Ÿ����� ũ��, follow ������ true�� �� ����

            // ��� ��ġ�� ���� �ӵ��� �̵�
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime*0.45f);

            // �÷��̾ �����ʿ� ���� �� Sprite�� ������ ������ �ʰ� �����ϰ�, batAttack�� ���� ��ġ�� ����
            if (target.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
                batAttack.transform.localPosition = new Vector2(Mathf.Abs(batAttack.transform.localPosition.x), batAttack.transform.localPosition.y);
            }

            // �÷��̾ ���ʿ� ���� �� Sprite�� ������ ������, batAttack�� ���� ��ġ�� ����
            else if (target.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                batAttack.transform.localPosition = new Vector2(-Mathf.Abs(batAttack.transform.localPosition.x), batAttack.transform.localPosition.y);
            }
        }
        else // ���� �Ÿ� �̳��� �ְų�, follow ������ false�� �� ����
        {
            rigid.velocity = Vector2.zero;   // Rigidbody2D�� �ӵ��� 0���� �����Ͽ� ����
        }
    }
}
