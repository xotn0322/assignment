using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public GameObject bullet;
    public Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    public bool Attack = false;
    // Start is called before the first frame update

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);//움직임
        

    }
    void Think() // 행동 설정
    {
        nextMove = Random.Range(-1, 2); // 다음 활동

        Invoke("Think", 2);//2초마다 행동 변환
        if (nextMove != 0) //방향 전환
            spriteRenderer.flipX = nextMove == 1;
        

        if (nextMove == -1) //불스킬 발사 
        {
            GameObject bulletcopy = Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
    
