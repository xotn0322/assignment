using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawn : MonoBehaviour
{
    public List<GameObject> MobPool = new List<GameObject>();
    public GameObject[] Mobs;
    public int mobCount = 1; //프리팹당 몇개를 생산할 것인지
    public float minSpawn;
    public float maxSpawn;

    //몬스터 종류 확인 및 생성속도
    private void Awake()
    {
        for (int i = 0; i < Mobs.Length; i++)
        {
            for (int j = 0; j < mobCount; j++)
            {
                MobPool.Add(CreateObj(Mobs[i], transform));
            }
        }

        minSpawn = 1.0f;
        maxSpawn = 3.0f;
}

    private void Start()
    {
        StartCoroutine(CreateMob());
    }

    //몹 생성
    IEnumerator CreateMob()
    {
        while (true)
        {
            MobPool[DeactiveMob()].SetActive(true);
            yield return new WaitForSeconds(Random.Range(minSpawn, maxSpawn));
        }
    }


    int DeactiveMob()
    {
        List<int> num = new List<int>();

        for (int i = 0; i < MobPool.Count; i++)
        {
            if (!MobPool[i].activeSelf)
            {
                num.Add(i);
            }
        }

        int x = 0;

        if (num.Count > 0)
        {
            x = num[Random.Range(0, num.Count)]; 
        }

        return x;
    }

    GameObject CreateObj(GameObject obj, Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }
}
