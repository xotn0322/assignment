using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public bool isPoisoned = false;         // �� ȿ���� Ȱ��ȭ�Ǿ������� ��Ÿ���� ����
    private float poisonDuration = 3f;     // �� ȿ���� ���� �ð�
    public float damageAmount = 3f;     // �� ȿ���� ���� ������ ��
    private Color originalColor;                // ���� ĳ���� ����
    private SpriteRenderer playerRenderer;   // ĳ������ SpriteRenderer ������Ʈ
    public Controller player;                        // ĳ���� ��Ʈ�ѷ� ��ũ��Ʈ

    private void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();  // ���� ���� ������Ʈ�� SpriteRenderer ������Ʈ ��������
        originalColor = playerRenderer.color;                             // ���� ĳ���� ���� ����
        player = FindObjectOfType<Controller>();                     // Scene���� Controller ��ũ��Ʈ�� ã�� �Ҵ�
    }

    public void StartPoisonEffect()  // �� ȿ���� Ȱ��ȭ�Ǿ� ���� ���� ��쿡�� ����
    {
        if (!isPoisoned)
        {
            isPoisoned = true;        // �� ȿ�� Ȱ��ȭ
            StartCoroutine(ApplyPoisonEffect());   // �� ȿ�� ������ ���� �ڷ�ƾ ����
        }
    }

    public IEnumerator ApplyPoisonEffect()
    {
        playerRenderer.color = Color.green;      // ĳ������ ������ ������� ����
        StartCoroutine(poisonDamaged(poisonDuration));  // �� ���ظ� �ֱ� ���� �ڷ�ƾ ����
        float elapsedTime = 0f;
        while (elapsedTime < poisonDuration)  // �� ȿ�� ���� �ð� ���� �ݺ�
        {
            elapsedTime += Time.deltaTime;   // ��� �ð� ����
            yield return null;
        }
        playerRenderer.color = originalColor;  // ĳ������ ������ ���� �������� ����
        isPoisoned = false;                               // �� ȿ�� ��Ȱ��ȭ
    }

    //delayTime���� �ݺ�����
    IEnumerator poisonDamaged(float delayTime)
    {
        if (isPoisoned)                                             // �� ȿ���� Ȱ��ȭ�� ���
        {
            GetComponent<Controller>().charHp -= 1f;   // ĳ������ ü�� ����
            yield return new WaitForSeconds(delayTime); // ���� �ð� ���
            StartCoroutine(poisonDamaged(delayTime));  // �� ���ظ� �ֱ� ���� �ڷ�ƾ �����
        }
        else
            yield return null;
    }
}
