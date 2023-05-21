using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet22 : MonoBehaviour
{
    GameObject Player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌 충돌 ");
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.25f, 0, 0);

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
