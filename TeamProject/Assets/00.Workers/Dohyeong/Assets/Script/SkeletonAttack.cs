using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    public Controller player;                // �÷��̾� ��Ʈ�ѷ� ������Ʈ
    public float damage = 13f;             // �÷��̾�� ���� ���ط�
    private Transform skeletonTransform;  // ���̷����� Ʈ������

    public void Start()
    {
        player = FindObjectOfType<Controller>();  // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ�� ã�� �Ҵ�
        skeletonTransform = transform.parent;      // ���̷����� �θ� Ʈ������ �Ҵ�
    }
    private void OnTriggerEnter2D(Collider2D collision)   
    {
        // find player
        if (collision.gameObject.tag == "Player")    // �浹�� ������Ʈ�� �±װ� "Player"�� ���
        {
            Debug.Log("�÷��̾ �ذ� ���ݹ��� ���� �����߽��ϴ�.");
            Vector2 skeletonPosition = skeletonTransform.position;  // ���̷����� ��ġ ���� ����
            player.Damaged(damage, skeletonPosition);  // �÷��̾� ��Ʈ�ѷ��� Damaged() �Լ� ȣ���Ͽ� �÷��̾�� ���� ������
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   // �浹�� ������Ʈ�� �±װ� "Player"�� ���
        {
            Debug.Log("�÷��̾ �ذ� ���ݹ��� ���� ������ϴ�.");
        }
    }
}
