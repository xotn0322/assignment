using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatChase : MonoBehaviour
{
    
    public float chaserRadius;  // 쫓아가는 범위의 반지름을 나타내는 변수

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("박쥐 쫓아온다");
            transform.parent.GetComponent<BatMove>().follow = true;  // 부모 객체의 BatMove 스크립트의 follow 변수를 true로 설정하여 쫓아가도록 함
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<BatMove>().follow = false;  // 플레이어와의 충돌이 끝나면 부모 객체의 BatMove 스크립트의 follow 변수를 false로 설정하여 쫓아가지 않도록 함
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaserRadius); // 트리거 콜라이더의 중심 위치를 기준으로 chaserRadius 반지름을 가진 원을 그려서 쫓아가는 범위를 시각적으로 표시함
    }
}
