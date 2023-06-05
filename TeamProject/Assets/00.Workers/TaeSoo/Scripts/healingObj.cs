using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healingObj : MonoBehaviour
{
    public Controller Player;
    public AudioClip healing;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Character").GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Sound.instance.SFXPlay("Healing", healing);
            if (Player.charHp > 65f)
            {
                Player.charHp = 100f;
            }
            else
            {
                Player.charHp += 35f;
            }
            Destroy(gameObject);
        }
    }
}
