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
            //타일 무한스크롤링
            for (int i = 0; i < tiles.Length; i++)
            {
                //어떠한 지점에서 사라지게 할지
                if (tiles[i].transform.position.x <= -15) //이 곳의 숫자를 변경할 시 사라지는 x축 지점 설정
                {
                    for (int j = 0; j < tiles.Length; j++)
                    {
                        if (temp.transform.position.x < tiles[j].transform.position.x)
                        {
                            temp = tiles[j];
                        }
                    }

                    //지나간 타일 뒤로 재배치
                    tiles[i].transform.position = new Vector2(temp.transform.position.x + 13.23f, 1);
                    tiles[i].sprite = groundImg[Random.Range(0, groundImg.Length)];
                }
            }

            //스크롤링
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * GameManager.instance.gameSpeed);
            }
        }
    }
}
