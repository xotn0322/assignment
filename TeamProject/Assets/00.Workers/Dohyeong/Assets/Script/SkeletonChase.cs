using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChase : MonoBehaviour
{
    private bool isChasing = false; // 추가: 쫓아오는 중인지 여부를 나타내는 변수
    public float chaserRadius;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("쫓아온다");
            transform.parent.GetComponent<SkeletonMove>().stopMove();
            Vector3 playerPos = collision.transform.position;
            int moveDir = 0; // 추가: moveDir 변수 선언
            Animator anim = transform.parent.GetComponent<Animator>(); // 추가: Animator 변수 선언


            if (playerPos.x > transform.position.x)
            {
                moveDir = 1;
                anim.SetInteger("WalkSpeed", moveDir);
            }
            else if (playerPos.x < transform.position.x)
            {
                moveDir = -1;
                anim.SetInteger("WalkSpeed", moveDir);
            }

            transform.parent.GetComponent<SkeletonMove>().moveDir = moveDir; // 변경된 moveDir 적용

            isChasing = true; // 추가: 쫓아오는 중으로 설정
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<SkeletonMove>().startMove();
            isChasing = false; // 추가: 쫓아오는 중이 아니게 설정
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform.position.x > transform.position.x && transform.parent.GetComponent<SkeletonMove>().moveDir == -1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 돌림
            }
            else if (playerTransform.position.x < transform.position.x && transform.parent.GetComponent<SkeletonMove>().moveDir == 1)
            {
                Flip(); // 플레이어가 반대편으로 이동한 경우 방향을 돌림
            }
        }
    }

    private void Flip()
    {
        int currentMoveDir = transform.parent.GetComponent<SkeletonMove>().moveDir;
        transform.parent.GetComponent<SkeletonMove>().moveDir = -currentMoveDir; // 방향을 반대로 설정
        Animator anim = transform.parent.GetComponent<Animator>();
        anim.SetInteger("WalkSpeed", transform.parent.GetComponent<SkeletonMove>().moveDir);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // 트리거 콜라이더의 반지름을 사용하여 범위 표시
    }
}