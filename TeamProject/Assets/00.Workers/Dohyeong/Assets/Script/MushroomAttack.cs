using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAttack : MonoBehaviour
{
    public Controller player;                                 // �÷��̾� ��Ʈ�ѷ� ���� ����
    public float damage = 5f;                                // �÷��̾�� ���� ���ط�
    private Transform mushroomTransform;        // ������ Ʈ������ ���� ����

    public void Start()
    {
        player = FindObjectOfType<Controller>();        // ��Ʈ�ѷ� ��ü ã�Ƽ� �Ҵ�
        mushroomTransform = transform.parent;         // ������ �θ� Ʈ������ �Ҵ�
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")          // �浹�� ���� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        {
            Debug.Log("�÷��̾ ���� ���ݹ��� ���� �����߽��ϴ�.");
            Vector2 mushroomPosition = mushroomTransform.position;    // ������ ��ġ ����
            player.Damaged(damage, mushroomPosition);           // �÷��̾��� Damaged() �Լ� ȣ���Ͽ� ���� ������
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")                    // �浹�� ���� ������Ʈ�� "Player" �±׸� ������ �ִ��� Ȯ��
        {
            Debug.Log("�÷��̾ ���� ���ݹ��� ���� ������ϴ�."); 
        }
    }
}
