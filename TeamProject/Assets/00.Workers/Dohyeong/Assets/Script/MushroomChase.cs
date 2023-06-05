using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomChase : MonoBehaviour
{
    private bool isChasing = false; // 추가: 쫓아오는 중인지 여부를 나타내는 변수
    public float chaserRadius;  // 쫓아오기 범위 반지름

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어를 찾음
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("버섯 쫓아온다");
            transform.parent.GetComponent<MushroomMove>().stopMove(); // MushroomMove 컴포넌트에서 정의한 이동을 멈추는 함수 호출
            Vector3 playerPos = collision.transform.position; // 플레이어의 위치를 가져옴
            int moveDir = 0; // 이동 방향을 나타내는 변수 초기화
            Animator anim = transform.parent.GetComponent<Animator>(); // 추가: Animator 변수 선언


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;          // 플레이어가 오른쪽에 있으면 이동 방향을 오른쪽으로 설정
                anim.SetInteger("WalkSpeed", moveDir); // Animator에 이동 방향 전달
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;       // 플레이어가 왼쪽에 있으면 이동 방향을 왼쪽으로 설정
                anim.SetInteger("WalkSpeed", moveDir);  // Animator에 이동 방향 전달
            }

            transform.parent.GetComponent<MushroomMove>().moveDir = moveDir; // MushroomMove 컴포넌트의 이동 방향 변경

            isChasing = true; // 쫓아오는 중으로 설정
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<MushroomMove>().startMove(); // MushroomMove 컴포넌트에서 정의한 이동을 시작하는 함수 호출
            isChasing = false; //쫓아오는 중이 아니게 설정
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트 가져오기
            MushroomMove MushroomMove = transform.parent.GetComponent<MushroomMove>();  // MushroomMove 컴포넌트 가져오기
            if (playerTransform.position.x > transform.position.x && MushroomMove.moveDir == -1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 반전시킴
            }
            else if (playerTransform.position.x < transform.position.x && MushroomMove.moveDir == 1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 반전시킴
            }

            // 지형 체크
            Vector2 frontVec = new Vector2(MushroomMove.rigid.position.x + MushroomMove.moveDir, MushroomMove.rigid.position.y - 0.5f); // 몬스터의 앞쪽 벡터 계산
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));                // 씬을 통해 앞쪽 벡터를 시각화

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // 앞쪽 벡터로 레이캐스트를 쏘고 지형과 충돌하는지 검사

            if (rayHit.collider == null)
            {
                MushroomMove.moveDir = 0; // 낭떠러지를 인식하면 이동을 멈춤
                MushroomMove.anim.SetInteger("WalkSpeed", MushroomMove.moveDir); // Animator에 이동 방향 전달
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<MushroomMove>().moveDir; // 현재 이동 방향 가져오기
        transform.parent.GetComponent<MushroomMove>().moveDir = -currentMoveDir; // 이동 방향을 반대로 설정
        Animator anim = transform.parent.GetComponent<Animator>();    // Animator 컴포넌트 가져오기
        anim.SetInteger("WalkSpeed", transform.parent.GetComponent<MushroomMove>().moveDir);  // Animator에 이동 방향 전달
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius);  // 트리거 콜라이더의 반지름을 사용하여 쫓아오기 범위를 표시
    }
}