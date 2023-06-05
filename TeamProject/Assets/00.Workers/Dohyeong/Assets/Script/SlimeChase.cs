using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChase : MonoBehaviour
{
    private bool isChasing = false; // 쫓아오는 중인지 여부를 나타내는 변수
    public float chaserRadius;  // 쫓아갈 범위 반지름

    public SlimeController controller; // SlimeController 스크립트를 참조하기 위한 변수

    public void Start()
    {
        controller = FindObjectOfType<SlimeController>(); // SlimeController 스크립트를 찾아 변수에 할당
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어를 찾았을 때
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("슬라임이 쫓아온다");
            transform.parent.GetComponent<SlimeMove>().stopMove(); // SlimeMove 스크립트의 stopMove() 함수 호출하여 슬라임 이동 멈춤
            Vector3 playerPos = collision.transform.position;  // 플레이어 위치 저장
            int moveDir = 0; // 이동 방향을 저장하는 변수
            Animator anim = transform.parent.GetComponent<Animator>(); // Animator 컴포넌트를 가져옴


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;    // 플레이어가 슬라임 오른쪽에 있으면 moveDir을 1로 설정
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;   // 플레이어가 슬라임 왼쪽에 있으면 moveDir을 -1로 설정
            }

            transform.parent.GetComponent<SlimeMove>().moveDir = moveDir; // 변경된 moveDir을 SlimeMove 스크립트의 moveDir에 적용

            isChasing = true; // 쫓아오는 중으로 설정
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SlimeMove>().startMove();  // SlimeMove 스크립트의 startMove() 함수 호출하여 슬라임 이동 시작
            isChasing = false; // 쫓아오는 중이 아니게 설정
        }
    }

    private void Update()
    {
        if (isChasing&& (!controller.isJump || !controller.isKnockback))
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;   // 플레이어 Transform 가져오기
            SlimeMove slimeMove = transform.parent.GetComponent<SlimeMove>();                      // SlimeMove 스크립트를 가져옴
            if (playerTransform.position.x > transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == -1)
            {
                Flip(); // 플레이어가 슬라임의 반대쪽으로 이동한 경우 방향을 바꿈
            }
            else if (playerTransform.position.x < transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == 1)
            {
                Flip(); // 플레이어가 슬라임의 반대쪽으로 이동한 경우 방향을 바꿈
            }

            // 지형 체크
            Vector2 frontVec = new Vector2(slimeMove.rigid.position.x + slimeMove.moveDir, slimeMove.rigid.position.y - 0.5f); // 몬스터의 앞쪽 벡터 계산
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));  // 씬을 통해 앞쪽 벡터를 시각화

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));   // 앞쪽 벡터로 레이캐스트를 쏘고 지형과 충돌하는지 검사

            if (rayHit.collider == null)
            {
                slimeMove.moveDir = 0; // 낭떠러지를 인식하면 이동을 멈춤
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<SlimeMove>().moveDir; // 현재 이동 방향 가져오기
        transform.parent.GetComponent<SlimeMove>().moveDir = -currentMoveDir; // 이동 방향을 반대로 설정
        Animator anim = transform.parent.GetComponent<Animator>();               // Animator 컴포넌트 가져오기
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // 트리거 콜라이더의 반지름을 사용하여 쫓아오기 범위를 표시
    }
}
