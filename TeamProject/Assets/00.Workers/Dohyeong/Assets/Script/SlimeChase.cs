using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChase : MonoBehaviour
{
    private bool isChasing = false; // �߰�: �Ѿƿ��� ������ ���θ� ��Ÿ���� ����
    public float chaserRadius;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("�Ѿƿ´�");
            transform.parent.GetComponent<SlimeMove>().stopMove();
            Vector3 playerPos = collision.transform.position;
            int moveDir = 0; // �߰�: moveDir ���� ����
            Animator anim = transform.parent.GetComponent<Animator>(); // �߰�: Animator ���� ����


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;
                anim.SetInteger("WalkSpeed", moveDir);
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;
                anim.SetInteger("WalkSpeed", moveDir);
            }

            transform.parent.GetComponent<SlimeMove>().moveDir = moveDir; // ����� moveDir ����

            isChasing = true; // �߰�: �Ѿƿ��� ������ ����
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SlimeMove>().startMove();
            isChasing = false; // �߰�: �Ѿƿ��� ���� �ƴϰ� ����
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform.position.x > transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == -1)
            {
                Flip(); // �÷��̾ �ݴ������� �̵��� ��� ������ ����
            }
            else if (playerTransform.position.x < transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == 1)
            {
                Flip(); // �÷��̾ �ݴ������� �̵��� ��� ������ ����
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<SlimeMove>().moveDir;
        transform.parent.GetComponent<SlimeMove>().moveDir = -currentMoveDir; // ������ �ݴ�� ����
        Animator anim = transform.parent.GetComponent<Animator>();
        anim.SetInteger("WalkSpeed", transform.parent.GetComponent<SlimeMove>().moveDir);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // Ʈ���� �ݶ��̴��� �������� ����Ͽ� ���� ǥ��
    }
}