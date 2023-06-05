using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatChase : MonoBehaviour
{
    
    public float chaserRadius;  // �Ѿư��� ������ �������� ��Ÿ���� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("���� �Ѿƿ´�");
            transform.parent.GetComponent<BatMove>().follow = true;  // �θ� ��ü�� BatMove ��ũ��Ʈ�� follow ������ true�� �����Ͽ� �Ѿư����� ��
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<BatMove>().follow = false;  // �÷��̾���� �浹�� ������ �θ� ��ü�� BatMove ��ũ��Ʈ�� follow ������ false�� �����Ͽ� �Ѿư��� �ʵ��� ��
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // Ʈ���� �ݶ��̴��� �߽� ��ġ�� �������� chaserRadius �������� ���� ���� �׷��� �Ѿư��� ������ �ð������� ǥ����
    }
}
