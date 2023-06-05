using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFire2 : MonoBehaviour
{
    GameObject Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("Character").GetComponent<Controller>().Damaged(5f, transform.position);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        this.Player = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 0.1f; // 이동 속도 조정

        transform.Translate(speed,0,0); // y 방향으로 떨어지는 속도 조정

        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }

        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }
        Vector2 p1 = transform.position;
        Vector2 p2 = this.Player.transform.position;
        Vector2 dir = p1 - p2;
        float d = dir.magnitude;

    }
}
