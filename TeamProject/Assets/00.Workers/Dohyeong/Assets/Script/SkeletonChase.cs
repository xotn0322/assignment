using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChase : MonoBehaviour
{
    private bool isChasing = false; // 쫓아오는 중인지 여부를 나타내는 변수
    public float chaserRadius;  // 쫓아오는 범위 반지름

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어를 찾음
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("해골 쫓아온다");
            transform.parent.GetComponent<SkeletonMove>().stopMove();  // 부모 객체의 SkeletonMove 스크립트의 stopMove() 메서드를 호출하여 움직임을 멈춤
            Vector3 playerPos = collision.transform.position;   // 플레이어의 위치를 가져옴
            int moveDir = 0;      // 이동 방향을 저장하는 변수
            Animator anim = transform.parent.GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져옴


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;               // 플레이어가 해골 오브젝트의 오른쪽에 있을 경우 moveDir을 1로 설정
                anim.SetInteger("WalkSpeed", moveDir);  // WalkSpeed 매개변수를 moveDir 값으로 설정하여 애니메이션 재생
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;          // 플레이어가 해골 오브젝트의 왼쪽에 있을 경우 moveDir을 -1로 설정
                anim.SetInteger("WalkSpeed", moveDir);   // WalkSpeed 매개변수를 moveDir 값으로 설정하여 애니메이션 재생
            }

            transform.parent.GetComponent<SkeletonMove>().moveDir = moveDir; // 변경된 moveDir 값을 SkeletonMove 스크립트의 moveDir 변수에 적용

            isChasing = true; // 쫓아오는 중으로 설정
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SkeletonMove>().startMove(); // 부모 객체의 SkeletonMove 스크립트의 startMove() 메서드를 호출하여 움직임을 시작
            isChasing = false; // 쫓아오는 중이 아니게 설정
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트를 가져옴
            SkeletonMove skeletonMove = transform.parent.GetComponent<SkeletonMove>();        // 부모 객체의 SkeletonMove 스크립트를 가져옴
            if (playerTransform.position.x > transform.position.x && skeletonMove.moveDir == -1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 뒤집음
            }
            else if (playerTransform.position.x < transform.position.x && skeletonMove.moveDir == 1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 뒤집음
            }

            // 지형 체크
            Vector2 frontVec = new Vector2(skeletonMove.rigid.position.x + skeletonMove.moveDir, skeletonMove.rigid.position.y - 0.5f); // 캐릭터의 앞쪽 벡터 계산
            Debug.DrawRay(frontVec, new Vector3(0, 0.2f, 0), new Color(0, 1, 0));  // 씬을 통해 앞쪽 벡터를 시각화

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, new Vector3(0, 0.2f, 0), 1, LayerMask.GetMask("Ground"));  // 앞쪽 벡터로 레이캐스트를 쏘고 지형과 충돌하는지 검사

            if (rayHit.collider == null)
            {
                skeletonMove.moveDir = 0; // 낭떠러지를 인식하면 이동을 멈춥니다.
                skeletonMove.anim.SetInteger("WalkSpeed", skeletonMove.moveDir);  // Animator에 이동 방향 전달
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<SkeletonMove>().moveDir;  // 현재 이동 방향을 가져옴
        transform.parent.GetComponent<SkeletonMove>().moveDir = -currentMoveDir;  // 이동 방향을 반대로 설정
        Animator anim = transform.parent.GetComponent<Animator>();     // 애니메이터 컴포넌트를 가져옴
        anim.SetInteger("WalkSpeed", transform.parent.GetComponent<SkeletonMove>().moveDir);    // WalkSpeed 매개변수를 변경된 이동 방향으로 설정하여 애니메이션 재생
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // 트리거 콜라이더의 반지름을 사용하여 범위 표시
    }
}
