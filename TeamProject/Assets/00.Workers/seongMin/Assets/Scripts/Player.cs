using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            position.x += -0.1f * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x += 0.1f * speed * Time.deltaTime;
        }
        transform.position = position;
    }
}
