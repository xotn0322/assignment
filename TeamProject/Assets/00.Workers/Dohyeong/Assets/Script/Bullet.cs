using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform playerPos;
    Vector2 dir;
    // 총알이 닿으면 없어질 레이어
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Character").GetComponent<Transform>();
        dir = playerPos.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(dir * Time.deltaTime * 5000);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 0, layer);
        RaycastHit2D groundRay = Physics2D.Raycast(transform.position, transform.right, 0, LayerMask.GetMask("Ground"));

        if (ray.collider != null)
        {
            Destroy(gameObject);
            if (ray.collider.tag == "Player")
            {
                Debug.Log("맞았다");
            }
        }

        if (groundRay.collider != null)
        {
            Destroy(gameObject);
        }

        // 카메라 밖으로 나갈 때 파괴
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }
}
