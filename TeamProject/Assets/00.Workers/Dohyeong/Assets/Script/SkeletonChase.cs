using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChase : MonoBehaviour
{
    private bool isChasing = false; // �Ѿƿ��� ������ ���θ� ��Ÿ���� ����
    public float chaserRadius;  // �Ѿƿ��� ���� ������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ ã��
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("�ذ� �Ѿƿ´�");
            transform.parent.GetComponent<SkeletonMove>().stopMove();  // �θ� ��ü�� SkeletonMove ��ũ��Ʈ�� stopMove() �޼��带 ȣ���Ͽ� �������� ����
            Vector3 playerPos = collision.transform.position;   // �÷��̾��� ��ġ�� ������
            int moveDir = 0;      // �̵� ������ �����ϴ� ����
            Animator anim = transform.parent.GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� ������


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;               // �÷��̾ �ذ� ������Ʈ�� �����ʿ� ���� ��� moveDir�� 1�� ����
                anim.SetInteger("WalkSpeed", moveDir);  // WalkSpeed �Ű������� moveDir ������ �����Ͽ� �ִϸ��̼� ���
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;          // �÷��̾ �ذ� ������Ʈ�� ���ʿ� ���� ��� moveDir�� -1�� ����
                anim.SetInteger("WalkSpeed", moveDir);   // WalkSpeed �Ű������� moveDir ������ �����Ͽ� �ִϸ��̼� ���
            }

            transform.parent.GetComponent<SkeletonMove>().moveDir = moveDir; // ����� moveDir ���� SkeletonMove ��ũ��Ʈ�� moveDir ������ ����

            isChasing = true; // �Ѿƿ��� ������ ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SkeletonMove>().startMove(); // �θ� ��ü�� SkeletonMove ��ũ��Ʈ�� startMove() �޼��带 ȣ���Ͽ� �������� ����
            isChasing = false; // �Ѿƿ��� ���� �ƴϰ� ����
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // �÷��̾��� Transform ������Ʈ�� ������
            SkeletonMove skeletonMove = transform.parent.GetComponent<SkeletonMove>();        // �θ� ��ü�� SkeletonMove ��ũ��Ʈ�� ������
            if (playerTransform.position.x > transform.position.x && skeletonMove.moveDir == -1)
            {
                Flip(); // �÷��̾ �ݴ������� �̵��� ��� ������ ������
            }
            else if (playerTransform.position.x < transform.position.x && skeletonMove.moveDir == 1)
            {
                Flip(); // �÷��̾ �ݴ������� �̵��� ��� ������ ������
            }

            // ���� üũ
            Vector2 frontVec = new Vector2(skeletonMove.rigid.position.x + skeletonMove.moveDir, skeletonMove.rigid.position.y - 0.5f); // ĳ������ ���� ���� ���
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));  // ���� ���� ���� ���͸� �ð�ȭ

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // ���� ���ͷ� ����ĳ��Ʈ�� ��� ������ �浹�ϴ��� �˻�

            if (rayHit.collider == null)
            {
                skeletonMove.moveDir = 0; // ���������� �ν��ϸ� �̵��� ����ϴ�.
                skeletonMove.anim.SetInteger("WalkSpeed", skeletonMove.moveDir);  // Animator�� �̵� ���� ����
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<SkeletonMove>().moveDir;  // ���� �̵� ������ ������
        transform.parent.GetComponent<SkeletonMove>().moveDir = -currentMoveDir;  // �̵� ������ �ݴ�� ����
        Animator anim = transform.parent.GetComponent<Animator>();     // �ִϸ����� ������Ʈ�� ������
        anim.SetInteger("WalkSpeed", transform.parent.GetComponent<SkeletonMove>().moveDir);    // WalkSpeed �Ű������� ����� �̵� �������� �����Ͽ� �ִϸ��̼� ���
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // Ʈ���� �ݶ��̴��� �������� ����Ͽ� ���� ǥ��
    }
}
