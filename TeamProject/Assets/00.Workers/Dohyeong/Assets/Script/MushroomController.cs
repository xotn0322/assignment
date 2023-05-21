using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    public GameObject bullet;
    void Start()
    {
        StartCoroutine(Bullet());
    }

    void FixedUpdate()
    {

    }

    IEnumerator Bullet()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Bullet());
    }
}