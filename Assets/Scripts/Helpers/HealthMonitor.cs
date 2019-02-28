using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMonitor : MonoBehaviour
{
	private EnemyStats Enemy;
	Slider healthBar;
	private PlayerManager PlayerManager;

	private void Start()
	{
		PlayerManager = PlayerManager.Instance;

		if (GetComponentInParent<EnemyStats>() != null)
			Enemy = GetComponentInParent<EnemyStats>();

		healthBar = gameObject.GetComponent<Slider>();


	}

	private void Update()
	{
		if (Enemy != null)
			healthBar.value = (Enemy.currentHP / Enemy.maxHP);
		else
			healthBar.value = (PlayerManager.PlayerHP / PlayerManager.PlayerMaxHP);
	}
}
