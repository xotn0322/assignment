using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Variables
	[Header("Player HP Image")]
	public Image Health_Filler;
	public Text Health_Text;
	public Image Mana_Filler;

	private Controller player;
	#endregion

	void Start()
	{
		player = GameObject.Find("Character").GetComponent<Controller>();
	}

	void Update()
	{
		float curHp = player.charHp;
		float fillValue = curHp / 10;
		Health_Filler.fillAmount = fillValue;

		float textHp = curHp * 10;
		Health_Text.text = textHp.ToString("F0") + "/" + 100f;
	}
}
