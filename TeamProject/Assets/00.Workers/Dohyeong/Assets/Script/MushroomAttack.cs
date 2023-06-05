using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAttack : MonoBehaviour
{
    public Controller player;                                 // 플레이어 컨트롤러 참조 변수
    public float damage = 5f;                                // 플레이어에게 입힐 피해량
    private Transform mushroomTransform;        // 버섯의 트랜스폼 참조 변수

    public void Start()
    {
        player = FindObjectOfType<Controller>();        // 컨트롤러 객체 찾아서 할당
        mushroomTransform = transform.parent;         // 버섯의 부모 트랜스폼 할당
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")          // 충돌한 게임 오브젝트가 "Player" 태그를 가지고 있는지 확인
        {
            Debug.Log("플레이어가 버섯 공격범위 내에 진입했습니다.");
            Vector2 mushroomPosition = mushroomTransform.position;    // 버섯의 위치 저장
            player.Damaged(damage, mushroomPosition);           // 플레이어의 Damaged() 함수 호출하여 피해 입히기
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")                    // 충돌한 게임 오브젝트가 "Player" 태그를 가지고 있는지 확인
        {
            Debug.Log("플레이어가 버섯 공격범위 내에 벗어났습니다."); 
        }
    }
}
