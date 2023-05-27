using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knigt : MonoBehaviour
{
    public Transform pos;
    public Animator anim;
    public int damage = 2;
    public BoxCollider2D box;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public float cooltime;
    private float currenttime;
    void Update()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(pos.position, new Vector2(1f, 1f), 1);

        if (collider != null)
        {
            for (int i = 0; i < collider.Length; i++)
            {
                if (currenttime <= 0)
                {
                    if (collider[i].tag == "Player")
                    {
                        anim.SetTrigger("Attack2");
                    }
                    currenttime = cooltime;
                }
            }
        }
        currenttime -= Time.deltaTime;
    }

    public void enbox()
    {
        box.enabled = true;
    }
    public void debox()
    {
        box.enabled = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, new Vector2(1f, 1f));
    }

}
