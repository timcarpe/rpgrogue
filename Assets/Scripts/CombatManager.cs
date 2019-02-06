using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Player;
using System.Linq;

namespace Combat
{
	public class CombatManager : MonoBehaviour
	{
		public PlayerManager PlayerManager;
		public EnemyManager EnemyManager;
		public DialogueManager DialogueManager;
		public UIManager UIManager;

		private bool combatHappening = false;
		private bool playerTurn = false;

		private IEnumerable<InitiativeContainer> turnOrder;

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
					playerTurn = false;

					DialogueManager.UpdateCombatText(damageDone);
					UIManager.ChangeOverheadText(EnemyManager.sceneEnemies[rand], damageDone.ToString(), 1);
				}
			}

		}

		public void StartCombat()
		{
			combatHappening = true;
			Debug.Log("Starting Combat!");
			CalculateInitiative();
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

		public void CalculateInitiative()
		{
			InitiativeContainer[] attackOrder;
			attackOrder = new InitiativeContainer[EnemyManager.sceneEnemies.Length + 1];
			Debug.Log("Created iniative array of length: " + attackOrder.Length);

			int playerRoll = Random.Range(1, 20) + PlayerManager.InitiativeBonus;
			Debug.Log("Player rolled: " + playerRoll + " initiative! With a +" + PlayerManager.InitiativeBonus + " bonus!");

			int i = 0;

			foreach(GameObject enemy in EnemyManager.sceneEnemies)
			{
				attackOrder[i] = new InitiativeContainer();

				attackOrder[i].entity = enemy;
				attackOrder[i].entityRoll = Random.Range(1, 20) + enemy.GetComponent<EnemyStats>().IniativeBonus;
				Debug.Log(attackOrder[i].entity.name + " rolled: " + attackOrder[i].entityRoll + " initiative! With a +" + enemy.GetComponent<EnemyStats>().IniativeBonus + " bonus.");
				i++;
			}

			attackOrder[i] = new InitiativeContainer();

			attackOrder[i].entity = PlayerManager.gameObject;
			attackOrder[i].entityRoll = playerRoll;

			turnOrder = attackOrder.OrderByDescending(x => x.entityRoll);

			Debug.Log("Sorted turn order:");
			foreach (InitiativeContainer item in turnOrder)
			{
				Debug.Log(item.entity.name + " with a " + item.entityRoll + ".");
				Debug.Log(item.entity.ToString() + item.entityRoll.ToString());
				UIManager.ChangeOverheadText(item.entity, item.entityRoll.ToString(), 0);
			}
		}
	}

	public class InitiativeContainer
	{
		public GameObject entity;
		public int entityRoll;
	}
}
