using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    Animator anim;
    public int maxHealth = 10;         // 몬스터의 최대 체력
    private int currentHealth;          // 몬스터의 현재 체력
    public Controller player;
    public SlimeMove move;

    private Transform playerTransform;   // 플레이어의 위치를 저장하기 위한 변수
    private Transform slimeTransform;

    public SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트


    public void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;    // 몬스터의 체력을 최대 체력으로 초기화
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;  // 플레이어의 Transform 컴포넌트 가져오기
        player = FindObjectOfType<Controller>();
        move = FindObjectOfType<SlimeMove>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기
        slimeTransform = transform;
    }


    public void Update()
    {
        
    }

    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("슬라임과 충돌");
            playerTransform.GetComponent<Controller>().Damaged(3f, slimeTransform.position); // 플레이어에게 데미지를 입히는 메서드 호출
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (player.isAttack == true)
        {
            currentHealth -= damageAmount;   // 몬스터 체력에서 피해량을 감소시킴
            anim.SetTrigger("Hurt");
        }
        if (currentHealth <= 0)
        {
            Die();    // 몬스터의 체력이 0 이하면 죽음 처리
        }
    }

    private void Die()
    {
        // 몬스터가 죽을 때의 동작을 구현
        anim.SetBool("Death", true);
        Destroy(gameObject);   // 몬스터 오브젝트 파괴
    }
}