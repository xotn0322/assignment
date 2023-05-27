using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
    public Controller player;
    public float damage = 3f;             // 플레이어에게 입힐 피해량
    private Transform batTransform;

    public void Start()
    {
        player = FindObjectOfType<Controller>();
        batTransform = transform.parent;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player entered trigger");
            Vector2 batPosition = batTransform.position;
            player.Damaged(damage, batPosition);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player exited trigger");
        }
    }
}
