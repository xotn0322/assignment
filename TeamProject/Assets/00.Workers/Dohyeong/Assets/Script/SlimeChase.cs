using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChase : MonoBehaviour
{
    private bool isChasing = false; // 추가: 쫓아오는 중인지 여부를 나타내는 변수
    public float chaserRadius;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("쫓아온다");
            transform.parent.GetComponent<SlimeMove>().stopMove();
            Vector3 playerPos = collision.transform.position;
            int moveDir = 0; // 추가: moveDir 변수 선언
            Animator anim = transform.parent.GetComponent<Animator>(); // 추가: Animator 변수 선언


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;
            }

            transform.parent.GetComponent<SlimeMove>().moveDir = moveDir; // 변경된 moveDir 적용

            isChasing = true; // 추가: 쫓아오는 중으로 설정
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SlimeMove>().startMove();
            isChasing = false; // 추가: 쫓아오는 중이 아니게 설정
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            SlimeMove slimeMove = transform.parent.GetComponent<SlimeMove>();
            if (playerTransform.position.x > transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == -1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 돌림
            }
            else if (playerTransform.position.x < transform.position.x && transform.parent.GetComponent<SlimeMove>().moveDir == 1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 돌림
            }

            // 지형 체크
            Vector2 frontVec = new Vector2(slimeMove.rigid.position.x + slimeMove.moveDir, slimeMove.rigid.position.y - 0.5f);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

            if (rayHit.collider == null)
            {
                slimeMove.moveDir = 0; // 낭떠러지를 인식하면 이동을 멈춥니다.
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<SlimeMove>().moveDir;
        transform.parent.GetComponent<SlimeMove>().moveDir = -currentMoveDir; // 방향을 반대로 설정
        Animator anim = transform.parent.GetComponent<Animator>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // 트리거 콜라이더의 반지름을 사용하여 범위 표시
    }
}