using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public Vector2 StartPosition;

    SpriteRenderer spRender;

    private void OnEnable()
    {
        transform.position = StartPosition;

        //��������Ʈ �¿����
        spRender = GetComponent<SpriteRenderer>();
        spRender.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        //�� ��ũ�Ѹ� �ӵ��� ���� ������
        if (GameManager.instance.isPlay)
        {
            transform.Translate(Vector2.left * Time.deltaTime * GameManager.instance.gameSpeed);

            if (transform.position.x < -6)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
