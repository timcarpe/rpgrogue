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
				enemy.GetComponent<EnemyStats>().CurrentHP = enemy.GetComponent<EnemyStats>().MaxHP;
		}

		public bool AreEnemiesDead()
		{
			foreach (GameObject enemy in sceneEnemies)
				if (enemy.GetComponent<EnemyStats>().CurrentHP > 0)
				{
					return false;
				}
			return true;

		}

		public void RemoveEnemiesFromScreen()
		{
			foreach (GameObject enemy in sceneEnemies)
				if (enemy.GetComponent<EnemyStats>().CurrentHP <= 0)
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
