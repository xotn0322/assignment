using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomChase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<MushroomMove>().stopMove();
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

            transform.parent.GetComponent<MushroomMove>().moveDir = moveDir; // 변경된 moveDir 적용

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<MushroomMove>().startMove();
        }
    }
}