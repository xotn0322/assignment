using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int moveDir;
    public float nextThinkTime;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("monsterAI");
    }


    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(moveDir, rigid.velocity.y);
        if (rigid.velocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        //지형 체크
        //몬스터는 앞을 체크해야 
        Vector2 frontVec = new Vector2(rigid.position.x + moveDir, rigid.position.y - 0.5f);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        // 시작,방향 색깔

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (rayHit.collider == null)
        {
            Debug.Log("경고! 이 앞 낭떨어지다!");
            Turn();
        }
    }

    IEnumerator monsterAI()
    {
        moveDir = Random.Range(-1, 2);
        anim.SetInteger("WalkSpeed", moveDir);
        nextThinkTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(nextThinkTime);
        StartCoroutine("monsterAI");
    }

    void Turn()
    {
        moveDir = moveDir * (-1);
        CancelInvoke();
    }

    public void startMove()
    {
        StartCoroutine("monsterAI");
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI");
    }
}