using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFire : MonoBehaviour
{
    public GameObject RandomFire2Prefab;
    public float span = 1.0f;
    private float delta = 0;

    private void Update()
    {
        delta += Time.deltaTime;
        if (delta > span)
        {
            delta = 0;
            GameObject go = Instantiate(RandomFire2Prefab);
            float px = Random.Range(72f,82f);
            go.transform.position = new Vector3(px,10f,0f);
        }
    }
}