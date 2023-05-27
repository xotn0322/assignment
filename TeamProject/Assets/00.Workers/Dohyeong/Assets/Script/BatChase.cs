using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatChase : MonoBehaviour
{
    
    public float chaserRadius;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("���� �Ѿƿ´�");
            transform.parent.GetComponent<BatMove>().follow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<BatMove>().follow = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // Ʈ���� �ݶ��̴��� �������� ����Ͽ� ���� ǥ��
    }
}
