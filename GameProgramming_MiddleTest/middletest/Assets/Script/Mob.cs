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

        //스프라이트 좌우반전
        spRender = GetComponent<SpriteRenderer>();
        spRender.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        //맵 스크롤링 속도와 맞춰 움직임
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
