using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaMonitor : MonoBehaviour
{
	private EnemyStats Enemy;
	Slider manaBar;
	private PlayerManager PlayerManager;

	private void Start()
	{
		PlayerManager = PlayerManager.Instance;

		if (GetComponentInParent<EnemyStats>() != null)
			Enemy = GetComponentInParent<EnemyStats>();

		manaBar = gameObject.GetComponent<Slider>();

		if (Enemy != null && Enemy.maxMana == 0)
			manaBar.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Enemy != null && Enemy.maxMana > 0)
			manaBar.value = (Enemy.currentMana / Enemy.maxMana);
		else
			manaBar.value = (PlayerManager.PlayerMana / PlayerManager.PlayerMaxMana);
	}
}
