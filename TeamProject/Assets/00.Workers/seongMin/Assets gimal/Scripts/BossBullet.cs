using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Animator anim;
    public LayerMask layer;
    public float lifetime = 3f; // 수명
    public float bulletSpeed = 3f; // 총알 속도

    private Transform playerPos;
    private Rigidbody2D rb;
    private float spawnTime;
    public float maxDistance = 5f; // 불이 유효한 범위의 최대 거리

    private Vector3 initialPosition; // 불의 초기 위치

    void Start()
    {
        spawnTime = Time.time;
        playerPos = GameObject.Find("Character").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        // 플레이어 방향으로 총알을 발사하기 위해 방향 벡터 설정
        Vector2 direction = (playerPos.position - transform.position).normalized;

        // 총알의 회전 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 총알의 회전 설정
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 총알의 속도 설정
        rb.velocity = direction * bulletSpeed;
    }

    void Update()
    {
        // 수명이 다한 경우 총알 제거
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
        float distance = Vector3.Distance(initialPosition, transform.position);
        if (distance > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("Character").GetComponent<Controller>().Damaged(17f, transform.position);
            Destroy(gameObject);
        }
    }
}

