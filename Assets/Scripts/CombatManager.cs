using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Player;

namespace Combat
{
	public class CombatManager : MonoBehaviour
	{
		public PlayerManager PlayerManager;
		public EnemyManager EnemyManager;
		public DialogueManager DialogueManager;

		private bool combatHappening = false;

		void Update()
		{
			CheckToEndCombat();

		}
		public int BasicAttack()
		{
			int damage = 0;

			for (int i = 0; i <= PlayerManager.DiceRoll; i++)
				damage += Random.Range(1, PlayerManager.DiceType);

			return damage + PlayerManager.DamageBonus;
		}

		public void AttackPlayer(GameObject enemy)
		{
			int damage = 0;
			int enemyDamageDone;

			for (int i = 0; i <= enemy.GetComponent<EnemyStats>().DiceRoll; i++)
				damage += Random.Range(1, enemy.GetComponent<EnemyStats>().DiceType);

			enemyDamageDone = damage + enemy.GetComponent<EnemyStats>().AddDamage;

			PlayerManager.DamagePlayerHP(enemyDamageDone);

			DialogueManager.UpdateCombatText(enemyDamageDone);
		}

		public void AttackEnemy()
		{
			int rand;
			bool hitEnemy = false;
			int damageDone = BasicAttack();

			while(hitEnemy == false)
			{
				rand = Random.Range(0, EnemyManager.sceneEnemies.Length);

				if(EnemyManager.sceneEnemies[rand].activeInHierarchy)
				{
					damageDone -= EnemyManager.sceneEnemies[rand].GetComponent<EnemyStats>().EnemyArmor;
					EnemyManager.sceneEnemies[rand].GetComponent<EnemyStats>().CurrentHP -= damageDone;
					hitEnemy = true;
				}
			}

			DialogueManager.UpdateCombatText(damageDone);
		}

		public void StartCombat()
		{
			combatHappening = true;
			Debug.Log("Starting Combat!");
		}

		public void CheckToEndCombat()
		{
			if(EnemyManager.AreEnemiesDead() && combatHappening == true)
			{
				Debug.Log("Ending Combat! Enemies dead?:" + EnemyManager.AreEnemiesDead());
				combatHappening = false;
				DialogueManager.SetMoveOnButton("You defeated all of the enemies!");
			}
			EnemyManager.RemoveEnemiesFromScreen();
		}
	}
}
