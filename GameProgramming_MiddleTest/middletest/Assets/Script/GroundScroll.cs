using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    public SpriteRenderer[] tiles;
    public Sprite[] groundImg;
    SpriteRenderer temp;

    // Start is called before the first frame update
    void Start()
    {
        temp = tiles[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlay)
        {
            //Ÿ�� ���ѽ�ũ�Ѹ�
            for (int i = 0; i < tiles.Length; i++)
            {
                //��� �������� ������� ����
                if (tiles[i].transform.position.x <= -15) //�� ���� ���ڸ� ������ �� ������� x�� ���� ����
                {
                    for (int j = 0; j < tiles.Length; j++)
                    {
                        if (temp.transform.position.x < tiles[j].transform.position.x)
                        {
                            temp = tiles[j];
                        }
                    }

                    //������ Ÿ�� �ڷ� ���ġ
                    tiles[i].transform.position = new Vector2(temp.transform.position.x + 13.23f, 1);
                    tiles[i].sprite = groundImg[Random.Range(0, groundImg.Length)];
                }
            }

            //��ũ�Ѹ�
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * GameManager.instance.gameSpeed);
            }
        }
    }
}
