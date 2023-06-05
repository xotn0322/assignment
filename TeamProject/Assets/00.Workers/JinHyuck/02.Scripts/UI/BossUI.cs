using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [Header("BoSS HP Image")]
    public Image Health_Filler;
    public Text Health_Text;
    public GameObject BossHUD;

    public Vector2 boxSize;
    public Vector3 offset;

    private BossMove Boss;
    private bool PlayerIn;
    void Start()
    {
        Boss = GameObject.Find("Boss").GetComponent<BossMove>();
    }

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + offset, boxSize, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
                BossHUD.SetActive(true);
        }

        float curHp = Boss.currentHealth;
        float fillValue = curHp / Boss.maxHealth;
        Health_Filler.fillAmount = fillValue;

        float textHp = fillValue * 100;
        Health_Text.text = textHp.ToString("F0") + "%";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }
}
