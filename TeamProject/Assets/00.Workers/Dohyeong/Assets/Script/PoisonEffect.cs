using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public bool isPoisoned = false;         // 독 효과가 활성화되었는지를 나타내는 변수
    private float poisonDuration = 3f;     // 독 효과의 지속 시간
    public float damageAmount = 3f;     // 독 효과로 인한 데미지 양
    private Color originalColor;                // 원래 캐릭터 색상
    private SpriteRenderer playerRenderer;   // 캐릭터의 SpriteRenderer 컴포넌트
    public Controller player;                        // 캐릭터 컨트롤러 스크립트

    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();  // 현재 게임 오브젝트의 SpriteRenderer 컴포넌트 가져오기
        originalColor = playerRenderer.color;                             // 원래 캐릭터 색상 저장
        player = FindObjectOfType<Controller>();                     // Scene에서 Controller 스크립트를 찾아 할당
    }

    public void StartPoisonEffect()  // 독 효과가 활성화되어 있지 않은 경우에만 실행
    {
        if (!isPoisoned)
        {
            isPoisoned = true;        // 독 효과 활성화
            StartCoroutine(ApplyPoisonEffect());   // 독 효과 적용을 위한 코루틴 실행
        }
    }

    public IEnumerator ApplyPoisonEffect()
    {
        playerRenderer.color = Color.green;      // 캐릭터의 색상을 녹색으로 변경
        StartCoroutine(poisonDamaged(poisonDuration));  // 독 피해를 주기 위한 코루틴 실행
        float elapsedTime = 0f;
        while (elapsedTime < poisonDuration)  // 독 효과 지속 시간 동안 반복
        {
            elapsedTime += Time.deltaTime;   // 경과 시간 누적
            yield return null;
        }
        playerRenderer.color = originalColor;  // 캐릭터의 색상을 원래 색상으로 복원
        isPoisoned = false;                               // 독 효과 비활성화
    }

    //delayTime마다 반복실행
    IEnumerator poisonDamaged(float delayTime)
    {
        if (isPoisoned)                                             // 독 효과가 활성화된 경우
        {
            GetComponent<Controller>().charHp -= 1f;   // 캐릭터의 체력 감소
            yield return new WaitForSeconds(delayTime); // 일정 시간 대기
            StartCoroutine(poisonDamaged(delayTime));  // 독 피해를 주기 위한 코루틴 재실행
        }
        else
            yield return null;
    }
}
