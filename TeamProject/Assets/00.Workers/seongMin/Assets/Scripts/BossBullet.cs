using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Animator anim;
    Transform playerPos;
    Vector2 dir;
    public LayerMask layer;
    private float lifetime = 2f; //수명
    private float spawnTime; //생성 시간

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
        dir = playerPos.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(dir * Time.deltaTime * 300);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 0, layer);
        if (ray.collider != null)
        {
            
            if (ray.collider.tag == "Player")
            {
                Debug.Log("over");
                Destroy(gameObject);
               
            }
            if (Time.time - spawnTime > lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}
