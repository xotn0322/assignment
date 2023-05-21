using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] checkPoint;
    bool[] checking;

    // Start is called before the first frame update
    void Start()
    {
        checking = new bool[checkPoint.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
