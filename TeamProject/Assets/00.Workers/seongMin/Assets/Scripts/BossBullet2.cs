using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet2 : MonoBehaviour
{
    
    public GameObject BossBullet2Prefab;
    float span = 1.0f;
    float delta = 0;
    void Start()
    {
        
    }
    void Update()
    {
        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject go = Instantiate(BossBullet2Prefab);
            int px = Random.Range(-9, 7);
            go.transform.position = new Vector3(px, 10, 0);

        }
    }
    
}