using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCreate : MonoBehaviour
{
    public GameObject prefab;
    public float spawnInterval = 1.0f;
    public float spawnRange = 5.0f;

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnPrefab();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnPrefab()
    {
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0f, 0f);
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Renderer renderer = spawnedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Random.ColorHSV(); // 랜덤한 색상 설정
        }
    }
}