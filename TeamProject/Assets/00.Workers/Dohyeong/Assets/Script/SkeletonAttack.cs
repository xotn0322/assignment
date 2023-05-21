using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    public Controller player;
    public float damage = 5f;             // 플레이어에게 입힐 피해량
    private Transform skeletonTransform;

    public void Start()
    {
        player = FindObjectOfType<Controller>();
        skeletonTransform = transform.parent;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player entered trigger");
            Vector2 skeletonPosition = skeletonTransform.position;
            player.Damaged(damage, skeletonPosition);
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
