using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChase : MonoBehaviour
{
    private bool isChasing = false; // �Ѿƿ��� ������ ���θ� ��Ÿ���� ����
    public float chaserRadius;  // �Ѿư� ���� ������

    public SlimeController controller; // SlimeController ��ũ��Ʈ�� �����ϱ� ���� ����

    public void Start()
    {
        controller = FindObjectOfType<SlimeController>(); // SlimeController ��ũ��Ʈ�� ã�� ������ �Ҵ�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ ã���� ��
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("�������� �Ѿƿ´�");
            transform.parent.GetComponent<SlimeMove>().stopMove(); // SlimeMove ��ũ��Ʈ�� stopMove() �Լ� ȣ���Ͽ� ������ �̵� ����
            Vector3 playerPos = collision.transform.position;  // �÷��̾� ��ġ ����
            int moveDir = 0; // �̵� ������ �����ϴ� ����
            Animator anim = transform.parent.GetComponent<Animator>(); // Animator ������Ʈ�� ������


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;    // �÷��̾ ������ �����ʿ� ������ moveDir�� 1�� ����
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;   // �÷��̾ ������ ���ʿ� ������ moveDir�� -1�� ����
            }

            transform.parent.GetComponent<SlimeMove>().moveDir = moveDir; // ����� moveDir�� SlimeMove ��ũ��Ʈ�� moveDir�� ����

            isChasing = true; // �Ѿƿ��� ������ ����
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SlimeMove>().startMove();  // SlimeMove ��ũ��Ʈ�� startMove() �Լ� ȣ���Ͽ� ������ �̵� ����
            isChasing = false; // �Ѿƿ��� ���� �ƴϰ� ����
        }
    }

    private void Update()
    {
        if (isChasing&& (!controller.isJump || !controller.isKnockback))
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;   // �÷��̾� Transform ��������
            SlimeMove slimeMove = transform.parent.GetComponent<SlimeMove>();                      // SlimeMove ��ũ��Ʈ�� ������
            if (playerTransform.position.x > transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == -1)
            {
                Flip(); // �÷��̾ �������� �ݴ������� �̵��� ��� ������ �ٲ�
            }
            else if (playerTransform.position.x < transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == 1)
            {
                Flip(); // �÷��̾ �������� �ݴ������� �̵��� ��� ������ �ٲ�
            }

            // ���� üũ
            Vector2 frontVec = new Vector2(slimeMove.rigid.position.x + slimeMove.moveDir, slimeMove.rigid.position.y - 0.5f); // ������ ���� ���� ���
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));  // ���� ���� ���� ���͸� �ð�ȭ

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));   // ���� ���ͷ� ����ĳ��Ʈ�� ��� ������ �浹�ϴ��� �˻�

            if (rayHit.collider == null)
            {
                slimeMove.moveDir = 0; // ���������� �ν��ϸ� �̵��� ����
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<SlimeMove>().moveDir; // ���� �̵� ���� ��������
        transform.parent.GetComponent<SlimeMove>().moveDir = -currentMoveDir; // �̵� ������ �ݴ�� ����
        Animator anim = transform.parent.GetComponent<Animator>();               // Animator ������Ʈ ��������
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // Ʈ���� �ݶ��̴��� �������� ����Ͽ� �Ѿƿ��� ������ ǥ��
    }
}
