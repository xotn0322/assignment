using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
    public Controller player;              // �÷��̾� ��Ʈ�ѷ� ��ü
    public float damage = 8f;             // �÷��̾�� ���� ���ط�
    private Transform batTransform; // ������ Ʈ������ ������Ʈ

    public void Start()
    {
        player = FindObjectOfType<Controller>(); // �÷��̾� ��Ʈ�ѷ��� ã�� �Ҵ�
        batTransform = transform.parent;   // �θ� ��ü�� Ʈ������ ������Ʈ �Ҵ�
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")  // �浹�� ��ü�� "Player" �±׸� ������ �ִ��� Ȯ��
        {
            Debug.Log("�÷��̾ ���� ���ݹ��� ���� �����߽��ϴ�.");
            Vector2 batPosition = batTransform.position;  // ������ ��ġ�� ������
            player.Damaged(damage, batPosition);         // �÷��̾�� ���ظ� ������ �Լ� ȣ��
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   // �浹�� ��ü�� "Player" �±׸� ������ �ִ��� Ȯ��
        {
            Debug.Log("�÷��̾ ���� ���ݹ��� ���� ������ϴ�.");
        }
    }
}
