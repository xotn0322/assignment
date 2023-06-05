using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomChase : MonoBehaviour
{
    private bool isChasing = false; // �߰�: �Ѿƿ��� ������ ���θ� ��Ÿ���� ����
    public float chaserRadius;  // �Ѿƿ��� ���� ������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ ã��
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("���� �Ѿƿ´�");
            transform.parent.GetComponent<MushroomMove>().stopMove(); // MushroomMove ������Ʈ���� ������ �̵��� ���ߴ� �Լ� ȣ��
            Vector3 playerPos = collision.transform.position; // �÷��̾��� ��ġ�� ������
            int moveDir = 0; // �̵� ������ ��Ÿ���� ���� �ʱ�ȭ
            Animator anim = transform.parent.GetComponent<Animator>(); // �߰�: Animator ���� ����


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;          // �÷��̾ �����ʿ� ������ �̵� ������ ���������� ����
                anim.SetInteger("WalkSpeed", moveDir); // Animator�� �̵� ���� ����
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;       // �÷��̾ ���ʿ� ������ �̵� ������ �������� ����
                anim.SetInteger("WalkSpeed", moveDir);  // Animator�� �̵� ���� ����
            }

            transform.parent.GetComponent<MushroomMove>().moveDir = moveDir; // MushroomMove ������Ʈ�� �̵� ���� ����

            isChasing = true; // �Ѿƿ��� ������ ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<MushroomMove>().startMove(); // MushroomMove ������Ʈ���� ������ �̵��� �����ϴ� �Լ� ȣ��
            isChasing = false; //�Ѿƿ��� ���� �ƴϰ� ����
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �÷��̾��� Transform ������Ʈ ��������
            MushroomMove MushroomMove = transform.parent.GetComponent<MushroomMove>();  // MushroomMove ������Ʈ ��������
            if (playerTransform.position.x > transform.position.x && MushroomMove.moveDir == -1)
            {
                Flip(); // �÷��̾ �ݴ������� �̵��� ��� ������ ������Ŵ
            }
            else if (playerTransform.position.x < transform.position.x && MushroomMove.moveDir == 1)
            {
                Flip(); // �÷��̾ �ݴ������� �̵��� ��� ������ ������Ŵ
            }

            // ���� üũ
            Vector2 frontVec = new Vector2(MushroomMove.rigid.position.x + MushroomMove.moveDir, MushroomMove.rigid.position.y - 0.5f); // ������ ���� ���� ���
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));                // ���� ���� ���� ���͸� �ð�ȭ

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // ���� ���ͷ� ����ĳ��Ʈ�� ��� ������ �浹�ϴ��� �˻�

            if (rayHit.collider == null)
            {
                MushroomMove.moveDir = 0; // ���������� �ν��ϸ� �̵��� ����
                MushroomMove.anim.SetInteger("WalkSpeed", MushroomMove.moveDir); // Animator�� �̵� ���� ����
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<MushroomMove>().moveDir; // ���� �̵� ���� ��������
        transform.parent.GetComponent<MushroomMove>().moveDir = -currentMoveDir; // �̵� ������ �ݴ�� ����
        Animator anim = transform.parent.GetComponent<Animator>();    // Animator ������Ʈ ��������
        anim.SetInteger("WalkSpeed", transform.parent.GetComponent<MushroomMove>().moveDir);  // Animator�� �̵� ���� ����
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius);  // Ʈ���� �ݶ��̴��� �������� ����Ͽ� �Ѿƿ��� ������ ǥ��
    }
}