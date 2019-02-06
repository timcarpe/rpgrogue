using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Enemies;

public class UIManager : MonoBehaviour
{
	public PlayerManager PlayerManager;
	public EnemyManager EnemyManager;
	public Slider healthBar;
	public GameObject[] sceneEnemies;

	void Start()
	{

	}

	void Update()
	{
		healthBar.value = CalculateHealth();

		CalculateAndDisplayEnemyHealth();
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

	private void CalculateAndDisplayEnemyHealth()
	{
		sceneEnemies = EnemyManager.sceneEnemies;
		Slider enemyHealthBar;

		if(sceneEnemies != null)
			foreach (GameObject enemy in sceneEnemies)
			{
				enemyHealthBar = enemy.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Slider>();

				enemyHealthBar.value = ( enemy.GetComponent<EnemyStats>().CurrentHP / enemy.GetComponent<EnemyStats>().MaxHP );
			}
		
	}


}

