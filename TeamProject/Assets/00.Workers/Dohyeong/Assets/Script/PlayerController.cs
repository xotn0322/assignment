using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾� �̵�
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-0.07f, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0.085f, 0, 0);
        }

        //�÷��̾ ȭ�� ������ �������� �ϴ� �ڵ�
        /*Vector3 worldpos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worldpos.x < 0f) worldpos.x = 0f;
        if (worldpos.y < 0f) worldpos.y = 0f;
        if (worldpos.x > 1f) worldpos.x = 1f;
        if (worldpos.y > 1f) worldpos.y = 1f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worldpos);*/
    }
}