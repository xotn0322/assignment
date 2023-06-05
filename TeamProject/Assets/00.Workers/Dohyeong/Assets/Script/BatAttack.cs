using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
    public Controller player;              // 플레이어 컨트롤러 객체
    public float damage = 8f;             // 플레이어에게 입힐 피해량
    private Transform batTransform; // 박쥐의 트랜스폼 컴포넌트

    public void Start()
    {
        player = FindObjectOfType<Controller>(); // 플레이어 컨트롤러를 찾아 할당
        batTransform = transform.parent;   // 부모 객체의 트랜스폼 컴포넌트 할당
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")  // 충돌한 객체가 "Player" 태그를 가지고 있는지 확인
        {
            Debug.Log("플레이어가 박쥐 공격범위 내에 진입했습니다.");
            Vector2 batPosition = batTransform.position;  // 박쥐의 위치를 가져옴
            player.Damaged(damage, batPosition);         // 플레이어에게 피해를 입히는 함수 호출
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   // 충돌한 객체가 "Player" 태그를 가지고 있는지 확인
        {
            Debug.Log("플레이어가 박쥐 공격범위 내에 벗어났습니다.");
        }
    }
}
