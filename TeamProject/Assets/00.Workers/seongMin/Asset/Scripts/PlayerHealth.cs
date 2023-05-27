using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;
    public int hp = 2;
    bool isHurt;
    public float speed;
    bool isKnockback = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("EnemyAtk"))
        {
            Hurt(collider.GetComponentInParent<Knigt>().damage, collider.transform.position);
            
            Debug.Log("맞았습니다 ");
        }
    }
    private void Hurt(int damage, Vector3 pos)
    {
        if (!isHurt)
        {
            isHurt = true;
            hp = hp - damage;
            if (hp <= 0)
            {
                
            }
        }
        else
        {
            float x = transform.position.x - pos.x;
            if (x < 0)
                x = 1;
            else
                x = -1;

            StartCoroutine(Knockback(x));
            StartCoroutine(HurtRoutine());
          
        }
    }
    IEnumerator Knockback(float dir)
    {
        isKnockback = true;
        float ctime = 0;
        while (ctime < 0.2f)
        {
            if (transform.rotation.y == 0)
                transform.Translate(Vector2.left * speed * Time.deltaTime * dir);
            else
                transform.Translate(Vector2.left * speed * Time.deltaTime * -1f * dir);

            ctime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
    }
 
    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(5f);
        isHurt = false;
    }
    private void Update()
    {
        if (!isKnockback)
            walk();
            
    }
    public float walk_speed;
    
    void walk()
    {
        float hor = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(Mathf.Abs(hor) * walk_speed * Time.deltaTime, 0, 0));
        if (hor > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (hor < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}


