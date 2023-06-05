using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;                   // ����� �̵� �ӵ�
    public float distance;                // ������� �̵��� �ִ� �Ÿ�
    public LayerMask isLayer;          // �浹�� ������ ���̾�
    public PoisonEffect poison;          // �� ȿ�� ��ũ��Ʈ
    public MushroomController controller;  // ���� ��Ʈ�ѷ� ��ũ��Ʈ
    private Transform playerTransform;   // �÷��̾��� ��ġ�� ������ ����



    // Start is called before the first frame update
    void Start()
    {
        poison = FindObjectOfType<PoisonEffect>();                    // �� ȿ�� ��ũ��Ʈ�� ã�Ƽ� ������ �Ҵ�
        controller = FindObjectOfType<MushroomController>();   // ���� ��Ʈ�ѷ� ��ũ��Ʈ�� ã�Ƽ� ������ �Ҵ�
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // "Player" �±׸� ���� ���� ������Ʈ�� ��ġ�� ����
        Invoke("DestroyBullet", 2);           // 2�� �Ŀ� DestroyBullet �޼��带 ȣ���Ͽ� ����� ����
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast; // �浹 ������ ������ ����
        if (controller.isMovingRight)
        {
            raycast = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);          // ���������� �̵� ���� �� �浹�� ����
        }
        else
        {
            raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);    // �������� �̵� ���� �� �浹�� ����
        }

        if (raycast.collider != null)
        {
            if (raycast.collider.tag == "Player")  // �浹�� ������Ʈ�� �±װ� "Player"�� ���
            {
                Debug.Log("���� �ɷȴ�");         // �ֿܼ� "���� �ɷȴ�"��� �޽��� ���
                poison.StartPoisonEffect();         // �� ȿ�� ��ũ��Ʈ�� StartPoisonEffect �޼��� ȣ��
            }
            DestroyBullet();                               // ����� ����
        }

        if (controller.isMovingRight)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);  // ���������� �̵�
        }
        else
        {
            transform.Translate(transform.right * -1f * speed * Time.deltaTime);  // �������� �̵�
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);  // ����� ����
    }
}
