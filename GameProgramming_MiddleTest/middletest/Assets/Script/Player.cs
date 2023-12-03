using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isJump = false;
    public bool isTop = false;
    public float jumpHeight = 0; //������ y : -4���� ���̼���
    public float jumpSpeed = 0;

    Vector2 startPosition;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlay)
            anim.SetBool("isRun", true);
        else
            anim.SetBool("isRun", false);

        //�����ν�
        if (Input.GetKey(KeyCode.Space) && GameManager.instance.isPlay)
        {
            isJump = true;
        }
        else if (transform.position.y <= startPosition.y)
        {
            isJump = false;
            isTop = false;
            transform.position = startPosition;
        }

        //����
        if (isJump)
        {
            //�Ʒ����� ����
            if (transform.position.y <= jumpHeight - 0.1f && !isTop)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, jumpHeight), jumpSpeed * Time.deltaTime);
            }
            else
            {
                isTop = true;
            }
            //������ ������
            if (transform.position.y > startPosition.y && isTop)
            {
                transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
            }
        }
    }

    //�� �浹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            GameManager.instance.GameOver();
        }
    }
}
