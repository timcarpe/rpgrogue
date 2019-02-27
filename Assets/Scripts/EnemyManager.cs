using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spells;
using System.Linq;

namespace Enemies
{
	public class EnemyManager : MonoBehaviour
	{
		public UIManager UIManager;
		public GameObject[] sceneEnemies;

		public void SetEnemyHP()
		{
			foreach (GameObject enemy in sceneEnemies)
				enemy.GetComponent<EnemyStats>().currentHP = enemy.GetComponent<EnemyStats>().maxHP;
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
					StartCoroutine(SetEnemyInActive(enemy));
				}
		}

		public IEnumerator SetEnemyInActive(GameObject enemy)
		{
			yield return new WaitForSecondsRealtime(1f);
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
				UIManager.ChangeOverheadText(enemy, spellDamage.ToString(), 0);
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
					UIManager.ChangeOverheadText(enemy, spellDamage.ToString(), 0);
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
					UIManager.ChangeOverheadText(enemy, spellDamage.ToString(), 0);

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
}
