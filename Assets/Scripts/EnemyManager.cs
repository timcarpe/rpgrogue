using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MEC;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance;
	//public UIManager UIManager;
	//private static UIManager m_UIManager;
	public static GameObject[] sceneEnemies;

	#region Singleton
	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("More than one singleton!");
			return;
		}

		Instance = this;
	}
	#endregion

	public void ResetEnemies()
	{
		foreach (GameObject enemy in sceneEnemies)
			enemy.GetComponent<EnemyStats>().ResetEnemy();
	}

	public bool AreEnemiesDead()
	{
		foreach (GameObject enemy in sceneEnemies)
			if (enemy.GetComponent<EnemyStats>().currentHP > 0)
			{
				return false;
			}
		return true;

	}

	public void RemoveEnemiesFromScreen()
	{
		foreach (GameObject enemy in sceneEnemies)
			if (enemy.GetComponent<EnemyStats>().currentHP <= 0)
			{
				Timing.RunCoroutine(_SetEnemyInActive(enemy));
				//yield return new WaitForSecondsRealtime(.5f);
				//enemy.SetActive(false);
				//Timing.StartCoroutine(_SetEnemyInActive(enemy));
			}
	}
		
	private IEnumerator<float> _SetEnemyInActive(GameObject enemy)
	{
		yield return Timing.WaitForSeconds(1f);
		enemy.SetActive(false);
	}
		
	public void SetSceneEnemeniesSize(int size)
	{
		sceneEnemies = new GameObject[size];
	}

	public void AddSceneEnemy(int index, GameObject enemy)
	{
		sceneEnemies[index] = enemy;
	}

	public void AddDOTDamage(GameObject g, string type, int damage)
	{
		EnemyStats enemy = g.GetComponent<EnemyStats>();

		switch (type)
		{
			case "Fire":
				enemy.fireDot += damage;
				break;
			case "Water":
				enemy.waterDot += damage;
				break;
			case "Nature":
				enemy.natureDot += damage;
				break;
			case "Arcane":
				enemy.arcaneDot += damage;
				break;

			default: break;
		}
	}

	public void AddDOTDamageToAllEnemies(string type, int damage)
	{
		foreach (GameObject enemy in sceneEnemies)
			if (enemy.activeSelf)
			{
				switch (type)
				{
					case "Fire":
						enemy.GetComponent<EnemyStats>().fireDot += damage;
						break;
					case "Water":
						enemy.GetComponent<EnemyStats>().waterDot += damage;
						break;
					case "Nature":
						enemy.GetComponent<EnemyStats>().natureDot += damage;
						break;
					case "Arcane":
						enemy.GetComponent<EnemyStats>().arcaneDot += damage;
						break;

					default: break;
				}
			}
	}

	public IEnumerator<float> _CycleDOTDamage(GameObject g)
	{
		int damage = 0;

		EnemyStats enemy = g.GetComponent<EnemyStats>();

		//Fire Damage
		if (enemy.resistantElement == "Fire")
			damage = enemy.fireDot / 2;
		else if (enemy.weakElement == "Fire")
			damage = enemy.fireDot * 2;
		else
			damage = enemy.fireDot;

		enemy.currentHP -= damage;

		UIManager.Instance.CreateOverheadText(g, damage.ToString(), 1);
		damage = 0;

		yield return Timing.WaitForSeconds(0.1f);

		//Water Damage
		if (enemy.resistantElement == "Water")
			damage = enemy.waterDot / 2;
		else if (enemy.weakElement == "Water")
			damage = enemy.waterDot * 2;
		else
			damage = enemy.waterDot;

		enemy.currentHP -= damage;

		UIManager.Instance.CreateOverheadText(g, damage.ToString(), 2);
		damage = 0;

		yield return Timing.WaitForSeconds(0.1f);

		//Nature Damage
		if (enemy.resistantElement == "Nature")
			damage = enemy.natureDot / 2;
		else if (enemy.weakElement == "Nature")
			damage = enemy.natureDot * 2;
		else
			damage = enemy.natureDot;

		enemy.currentHP -= damage;

		UIManager.Instance.CreateOverheadText(g, damage.ToString(), 3);
		damage = 0;

		yield return Timing.WaitForSeconds(0.1f);

		//Arcane Damage
		if (enemy.resistantElement == "Arcane")
			damage = enemy.arcaneDot / 2;
		else if (enemy.weakElement == "Arcane")
			damage = enemy.arcaneDot * 2;
		else
			damage = enemy.arcaneDot;

		enemy.currentHP -= damage;

		UIManager.Instance.CreateOverheadText(g, damage.ToString(), 4);
		damage = 0;

		enemy.fireDot /= 2;
		enemy.waterDot /= 2;
		enemy.natureDot /= 2;
		enemy.arcaneDot /= 2;
	}

	public void DealDamageToEnemy(GameObject enemy, int damage, string type = null)
	{
		if( enemy != null)
		{
			int spellDamage = damage;

			if (type == enemy.GetComponent<EnemyStats>().weakElement)
				spellDamage = spellDamage * 2;
			else if (type == enemy.GetComponent<EnemyStats>().resistantElement)
				spellDamage = spellDamage / 2;

			enemy.GetComponent<EnemyStats>().currentHP -= spellDamage;


			switch (type)
			{
				case "Fire":
					UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 1);
					break;
				case "Water":
					UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 2);
					break;
				case "Nature":
					UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 3);
					break;
				case "Arcane":
					UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 4);
					break;
			}
		}

		RemoveEnemiesFromScreen();
	}

	public void DealDamageToAllEnemies(int damage, string type = null)
	{
		foreach (GameObject enemy in sceneEnemies)
			if (enemy.activeSelf)
			{
				int spellDamage = damage;

				if (type == enemy.GetComponent<EnemyStats>().weakElement)
					spellDamage = spellDamage * 2;
				else if (type == enemy.GetComponent<EnemyStats>().resistantElement)
					spellDamage = spellDamage / 2;

				enemy.GetComponent<EnemyStats>().currentHP -= spellDamage;
				switch (type)
				{
					case "Fire":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 1);
						break;
					case "Water":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 2);
						break;
					case "Nature":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 3);
						break;
					case "Arcane":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 4);
						break;
				}
			}

		RemoveEnemiesFromScreen();
	}

	public void DealSplashDamage(GameObject target, int damage, int targets = 10, string type = null)
	{
		int hits = 0;
		foreach (GameObject enemy in sceneEnemies)
			if (hits < targets && enemy.activeSelf && enemy != target)
			{
				int spellDamage = damage;

				if (type == enemy.GetComponent<EnemyStats>().weakElement)
					spellDamage = spellDamage * 2;
				else if (type == enemy.GetComponent<EnemyStats>().resistantElement)
					spellDamage = spellDamage / 2;

				enemy.GetComponent<EnemyStats>().currentHP -= spellDamage;

				switch (type)
				{
					case "Fire":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 1);
						break;
					case "Water":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 2);
						break;
					case "Nature":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 3);
						break;
					case "Arcane":
						UIManager.Instance.CreateOverheadText(enemy, spellDamage.ToString(), 4);
						break;
				}

				hits++;
			}

		RemoveEnemiesFromScreen();
	}

	public GameObject GetFrontEnemy()
	{
		foreach (GameObject enemy in sceneEnemies)
			if (enemy.activeSelf)
			{
				return enemy;
			}
		return null;
	}

	public GameObject GetLastEnemy()
	{
		foreach (GameObject enemy in sceneEnemies.Reverse())
			if (enemy.activeSelf)
			{
				return enemy;
			}
		return null;
	}

	public GameObject GetNextToFrontEnemy()
	{
		GameObject front = GetFrontEnemy();
		foreach (GameObject enemy in sceneEnemies)
			if (enemy.activeSelf && enemy != front)
			{
				return enemy;
			}
		return null;
	}
}