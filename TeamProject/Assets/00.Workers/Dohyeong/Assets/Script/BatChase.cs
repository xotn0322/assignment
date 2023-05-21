using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatChase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<BatMove>().follow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<BatMove>().follow = false;
        }
    }
}
