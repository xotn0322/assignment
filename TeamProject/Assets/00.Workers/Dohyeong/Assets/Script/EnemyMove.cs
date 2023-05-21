using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    public float nextThinkTime;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think",nextThinkTime);
    }


    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //���� üũ
        //���ʹ� ���� üũ�ؾ� 
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove+0.2f, rigid.position.y-0.5f);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        // ����,���� ����

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (rayHit.collider == null)
        {
            Debug.Log("���! �� �� ����������!");
            Turn();
        }
    }

    //��� �Լ�
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);
        
        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        //Flip Sprite
        if(nextMove!=0)
            spriteRenderer.flipX = nextMove == -1;

        nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * (-1);
        spriteRenderer.flipX = nextMove == -1;
        CancelInvoke();
        nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }
}