using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;                   // 독방울 이동 속도
    public float distance;                // 독방울이 이동할 최대 거리
    public LayerMask isLayer;          // 충돌을 감지할 레이어
    public PoisonEffect poison;          // 독 효과 스크립트
    public MushroomController controller;  // 버섯 컨트롤러 스크립트
    private Transform playerTransform;   // 플레이어의 위치를 저장할 변수



    // Start is called before the first frame update
    void Start()
    {
        poison = FindObjectOfType<PoisonEffect>();                    // 독 효과 스크립트를 찾아서 변수에 할당
        controller = FindObjectOfType<MushroomController>();   // 버섯 컨트롤러 스크립트를 찾아서 변수에 할당
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // "Player" 태그를 가진 게임 오브젝트의 위치를 저장
        Invoke("DestroyBullet", 2);           // 2초 후에 DestroyBullet 메서드를 호출하여 독방울 제거
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast; // 충돌 정보를 저장할 변수
        if (controller.isMovingRight)
        {
            raycast = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);          // 오른쪽으로 이동 중일 때 충돌을 감지
        }
        else
        {
            raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);    // 왼쪽으로 이동 중일 때 충돌을 감지
        }

        if (raycast.collider != null)
        {
            if (raycast.collider.tag == "Player")  // 충돌한 오브젝트의 태그가 "Player"인 경우
            {
                Debug.Log("독에 걸렸다");         // 콘솔에 "독에 걸렸다"라는 메시지 출력
                poison.StartPoisonEffect();         // 독 효과 스크립트의 StartPoisonEffect 메서드 호출
            }
            DestroyBullet();                               // 독방울 제거
        }

        if (controller.isMovingRight)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);  // 오른쪽으로 이동
        }
        else
        {
            transform.Translate(transform.right * -1f * speed * Time.deltaTime);  // 왼쪽으로 이동
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);  // 독방울 제거
    }
}
