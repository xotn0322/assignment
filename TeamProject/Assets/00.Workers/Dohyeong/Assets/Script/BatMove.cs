using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMove : MonoBehaviour
{
    Rigidbody2D rigid; // Rigidbody2D 컴포넌트에 접근하기 위한 변수
    Transform target;  // 추적할 대상의 Transform 컴포넌트에 접근하기 위한 변수
    SpriteRenderer spriteRenderer; // SpriteRenderer 컴포넌트에 접근하기 위한 변수

    [SerializeField] [Range(0f, 4f)] float moveSpeed = 1f; // 이동 속도를 조절하기 위한 변수
    [SerializeField] [Range(0f, 3f)] float contactDistance = 0.5f; // 대상과의 접촉 거리를 조절하기 위한 변수
    public bool follow = false;  // 대상을 추적할지 여부를 결정하는 변수

    public GameObject batAttack; // 공격 오브젝트를 참조하기 위한 변수
    public BatController controller; // BatController 스크립트를 참조하기 위한 변수

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // 현재 게임 오브젝트에 연결된 Rigidbody2D 컴포넌트를 가져옴
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // "Player" 태그를 가진 게임 오브젝트의 Transform 컴포넌트를 가져옴
        spriteRenderer = GetComponent<SpriteRenderer>(); // 현재 게임 오브젝트에 연결된 SpriteRenderer 컴포넌트를 가져옴
        batAttack = GameObject.Find("batAttack");   // "batAttack" 이름의 게임 오브젝트를 찾아서 가져옴
        controller = FindObjectOfType<BatController>(); // Scene에서 BatController 스크립트를 찾아 가져옴
    }

    void Update()
    {
        if (!controller.isKnockback)  // BatController의 isKnockback 변수가 false일 때만 실행
        {
            FollowTarget();   // 대상을 추적하는 함수 호출
        }
    }

    void FollowTarget()
    {
        if (Vector2.Distance(transform.position, target.position) > contactDistance && follow)
        {
            // 현재 위치와 대상 위치 사이의 거리가 접촉 거리보다 크고, follow 변수가 true일 때 실행

            // 대상 위치로 일정 속도로 이동
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime*0.45f);

            // 플레이어가 오른쪽에 있을 때 Sprite의 방향을 뒤집지 않고 유지하고, batAttack의 로컬 위치를 설정
            if (target.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
                batAttack.transform.localPosition = new Vector2(Mathf.Abs(batAttack.transform.localPosition.x), batAttack.transform.localPosition.y);
            }

            // 플레이어가 왼쪽에 있을 때 Sprite의 방향을 뒤집고, batAttack의 로컬 위치를 설정
            else if (target.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                batAttack.transform.localPosition = new Vector2(-Mathf.Abs(batAttack.transform.localPosition.x), batAttack.transform.localPosition.y);
            }
        }
        else // 접촉 거리 이내에 있거나, follow 변수가 false일 때 실행
        {
            rigid.velocity = Vector2.zero;   // Rigidbody2D의 속도를 0으로 설정하여 멈춤
        }
    }
}
