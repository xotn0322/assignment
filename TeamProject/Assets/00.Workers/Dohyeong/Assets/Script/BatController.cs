using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform target;
    GameObject circle;

    [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f;

    [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;

    bool follow = false;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GameObject.Find("Circle");
        target = circle.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (Vector2.Distance(transform.position, target.position) > contactDistance && follow)
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        follow = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        follow = false;
    }
}
