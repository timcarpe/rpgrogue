using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
	public class EnemyManager : MonoBehaviour
	{
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
					enemy.SetActive(false);
				}
		}

		public void SetSceneEnemeniesSize(int size)
		{
			sceneEnemies = new GameObject[size];
		}

		public void AddSceneEnemy(int index, GameObject enemy)
		{
			sceneEnemies[index] = enemy;
		}
	}
}
