using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    public Controller player;                // 플레이어 컨트롤러 오브젝트
    public float damage = 13f;             // 플레이어에게 입힐 피해량
    private Transform skeletonTransform;  // 스켈레톤의 트랜스폼

    public void Start()
    {
        player = FindObjectOfType<Controller>();  // 플레이어 컨트롤러 스크립트를 찾아 할당
        skeletonTransform = transform.parent;      // 스켈레톤의 부모 트랜스폼 할당
    }
    private void OnTriggerEnter2D(Collider2D collision)   
    {
        // find player
        if (collision.gameObject.tag == "Player")    // 충돌한 오브젝트의 태그가 "Player"인 경우
        {
            Debug.Log("플레이어가 해골 공격범위 내에 진입했습니다.");
            Vector2 skeletonPosition = skeletonTransform.position;  // 스켈레톤의 위치 정보 저장
            player.Damaged(damage, skeletonPosition);  // 플레이어 컨트롤러의 Damaged() 함수 호출하여 플레이어에게 피해 입히기
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   // 충돌한 오브젝트의 태그가 "Player"인 경우
        {
            Debug.Log("플레이어가 해골 공격범위 내에 벗어났습니다.");
        }
    }
}
