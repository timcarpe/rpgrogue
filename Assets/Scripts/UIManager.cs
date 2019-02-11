using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Enemies;
using TMPro;

public class UIManager : MonoBehaviour
{
	public PlayerManager PlayerManager;
	public EnemyManager EnemyManager;
	public Slider healthBar;
	public GameObject[] sceneEnemies;
	public TextMeshProUGUI goldText;

	void Start()
	{

	}

	void Update()
	{
		healthBar.value = CalculateHealth();

		CalculateAndDisplayEnemyUI();

		DisplayGold();
	}

	private float CalculateHealth()
	{
		if (PlayerManager.PlayerHP > 0)
		{
			return PlayerManager.PlayerHP / PlayerManager.PlayerMaxHP;
		}
		else
			return 0;//die
	}

	private void DisplayGold()
	{
		goldText.text = PlayerManager.PlayerGold.ToString();
	}

	private void CalculateAndDisplayEnemyUI()
	{
		sceneEnemies = EnemyManager.sceneEnemies;
		Slider enemyHealthBar;

		if (sceneEnemies != null)
			foreach (GameObject enemy in sceneEnemies)
			{
				enemyHealthBar = enemy.GetComponentInChildren<Slider>();

				//enemyHealthBar = enemy.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Slider>();

				enemyHealthBar.value = ( enemy.GetComponent<EnemyStats>().currentHP / enemy.GetComponent<EnemyStats>().maxHP );
			}
		
	}

	public void ChangeOverheadText(GameObject entity, string text, int type = 0)
	{
		TextMeshProUGUI overHeadText = entity.GetComponentInChildren<TextMeshProUGUI>();

		if (overHeadText != null)
		{
			overHeadText.text = text;

			if (type == 0)
				overHeadText.faceColor = Color.white;
			else if (type == 1)
				overHeadText.faceColor = Color.red;
			else if (type == 2)
				overHeadText.faceColor = Color.green;

			overHeadText.gameObject.GetComponent<Animator>().Play("animate");
		}

	}


}

