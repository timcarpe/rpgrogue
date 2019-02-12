using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Player;
using System.Linq;
using UnityEngine.UI;

namespace Combat
{
	public class CombatManager : MonoBehaviour
	{
		public PlayerManager PlayerManager;
		public EnemyManager EnemyManager;
		public DialogueManager DialogueManager;
		public UIManager UIManager;
		public EventsManager EventsManager;

		private bool combatHappening = false;

		public IEnumerable<InitiativeContainer> turnOrder;

		[SerializeField] private string attackType;

		void Update()
		{
		}

		public void BasicAttack(GameObject enemy)
		{
			int damage = 0;

			int playerAttackRoll = Random.Range(1, 20) + PlayerManager.AttackBonus;
			
			int enemyArmor = enemy.GetComponent<EnemyStats>().enemyArmor;

			Debug.Log("Player rolled a " + playerAttackRoll + " vs a " + enemyArmor);

			if (playerAttackRoll >= enemyArmor && EnemyManager.AreEnemiesDead() == false)
			{
				for (int i = 0; i < PlayerManager.DiceRoll; i++)
					damage += Random.Range(1, PlayerManager.DiceType);

				damage += PlayerManager.DamageBonus;

				damage -= enemy.GetComponent<EnemyStats>().damageReduction;

				enemy.GetComponent<EnemyStats>().currentHP -= damage;

				DialogueManager.UpdateCombatText("You", damage);

				UIManager.ChangeOverheadText(enemy, damage.ToString(), 1);

				Debug.Log("Player attacking " + enemy.name + "  for " + damage + " damage! Used Basic Attack!");
			}
			else if (playerAttackRoll < enemyArmor)
			{
				Debug.Log("Player missed!");
			}
		}

		public void PowerAttack(GameObject enemy)
		{
			int powerAttackAmount = 3;
			int damage = 0;

			int playerAttackRoll = Random.Range(1, 20) + PlayerManager.AttackBonus - powerAttackAmount;

			int enemyArmor = enemy.GetComponent<EnemyStats>().enemyArmor;

			Debug.Log("Player rolled a " + playerAttackRoll + " vs a " + enemyArmor);

			if (playerAttackRoll >= enemyArmor && EnemyManager.AreEnemiesDead() == false)
			{
				for (int i = 0; i < PlayerManager.DiceRoll; i++)
					damage += Random.Range(1, PlayerManager.DiceType);

				damage += PlayerManager.DamageBonus + powerAttackAmount;
				
				damage -= enemy.GetComponent<EnemyStats>().damageReduction;

				enemy.GetComponent<EnemyStats>().currentHP -= damage;

				DialogueManager.UpdateCombatText("You", damage);

				UIManager.ChangeOverheadText(enemy, damage.ToString(), 1);

				Debug.Log("Player attacking " + enemy.name + "  for " + damage + " damage! Used Power Attack!");
			}
			else if (playerAttackRoll < enemyArmor)
			{
				Debug.Log("Player missed!");
			}
		}

		public void Whirlwind()
		{
			int playerAttackRoll;
			int enemyArmor;
			int damage = 0;
			//bool playerHit = false;

			foreach (GameObject enemy in EnemyManager.sceneEnemies)
			{
				if (enemy.activeSelf)
				{
					playerAttackRoll = Random.Range(1, 20) + PlayerManager.AttackBonus - 5;

					enemyArmor = enemy.GetComponent<EnemyStats>().enemyArmor;

					Debug.Log("Whirlwind: Player rolled a " + playerAttackRoll + " vs a " + enemyArmor);

					if (playerAttackRoll >= enemyArmor && EnemyManager.AreEnemiesDead() == false)
					{
						for (int i = 0; i < PlayerManager.DiceRoll; i++)
							damage += Random.Range(1, PlayerManager.DiceType);

						enemy.GetComponent<EnemyStats>().currentHP -= damage;

						//playerHit = true;

						DialogueManager.UpdateCombatText("You", damage);

						UIManager.ChangeOverheadText(enemy, damage.ToString(), 1);

						Debug.Log("Player attacking " + enemy.name + "  for " + damage + " damage! Used attack whirlwind!");

						damage = 0;

					}
					else if (playerAttackRoll < enemyArmor)
					{
						Debug.Log("Player missed!");
					}
				}
			}
		}

		public void AttackPlayer(GameObject enemy)
		{
			int enemyDamageDone = 0;

			int enemyAttackRoll = Random.Range(1, 20) + enemy.GetComponent<EnemyStats>().attackBonus;

			int playerArmor = Random.Range(1, 20) + PlayerManager.ArmorBonus;

			Debug.Log(enemy.name + " rolled a " + enemyAttackRoll + " vs a " + playerArmor);

			//Roll the damage from dice
			if (enemyAttackRoll >= playerArmor)
				for (int i = 0; i < enemy.GetComponent<EnemyStats>().diceRoll; i++)
					enemyDamageDone += Random.Range(1, enemy.GetComponent<EnemyStats>().diceType);
			else
			{
				Debug.Log(enemy.name + " missed!");
				return;
			}

			//Add any damage bonuses
			enemyDamageDone += enemy.GetComponent<EnemyStats>().addDamage;

			enemyDamageDone -= PlayerManager.DamageReduction;

			//Deal damage to the player
			PlayerManager.DamagePlayerHP(enemyDamageDone);

			DialogueManager.UpdateCombatText(enemy.GetComponent<EnemyStats>().enemyName, enemyDamageDone);

			UIManager.ChangeOverheadText(PlayerManager.gameObject, enemyDamageDone.ToString(), 1);

			Debug.Log(enemy.name + " attacking player for " + enemyDamageDone + " damage!");
		}

		/*public void AttackEnemyWrapper(GameObject enemy)
		{
			AttackEnemy();
		}*/

		public void AttackEnemy(GameObject enemy, string attackType)
		{
			switch (attackType)
			{
				case "TYPE1":

					BasicAttack(enemy);

					break;

				case "TYPE2":

					PowerAttack(enemy);

					break;

				case "TYPE3":

					Whirlwind();

					break;

				default: break;

			}
		}

		public void StartCombat()
		{
			combatHappening = true;

			Debug.Log("Starting Combat!");

			CalculateInitiative();

			StartCoroutine(RunCombat());
		}

		public IEnumerator RunCombat()
		{
			InitiativeContainer[] order = turnOrder.ToArray();

			int i = 0;
			while(i < order.Length)
			{
				yield return new WaitForSecondsRealtime(1f);

				//If the item in the array is not the player and combat is happening
				if (order[i].entity.activeSelf && order[i].entity.name != "Player" && combatHappening == true)
				{
					AttackPlayer(order[i].entity);
					i++;
				}
				//The item is the player
				else
				{
					bool choseEnemy = false;

					GameObject enemy;
					
					ToggleGroup toggleGroup = GameObject.Find("CombatUI").GetComponent<ToggleGroup>();

					while(choseEnemy == false)
					{
						if(toggleGroup.ActiveToggles().FirstOrDefault() != null)
							attackType = toggleGroup.ActiveToggles().FirstOrDefault().name;
						
						if (Input.GetMouseButtonDown(0) && attackType != null)
						{
							RaycastHit hitInfo = new RaycastHit();
							if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
							{
								enemy = hitInfo.collider.gameObject;

								if (enemy.CompareTag("Enemy"))
								{
									choseEnemy = true;

									AttackEnemy(enemy, attackType);

									Debug.Log("Clicked on: " + hitInfo.collider.gameObject.name);
								}
							}
						}
						yield return null;
					}
					i++;
				}

				CheckToEndCombat();

				//If combat ended exit the loop
				if (combatHappening == false)
					break;

				//If i is on the last item in the array reset it
				if (i == (order.Length) && combatHappening == true)
				{
					i = 0;
					Debug.Log("Starting another round of combat!");
				}

			}
		}
		public bool IsCombatHappening()
		{
			return combatHappening;
		}

		public void CheckToEndCombat()
		{
			//If enemies are dead and combat is currently happening
			if(PlayerManager.PlayerHP <= 0)
			{
				combatHappening = false;
			}
			else if (EnemyManager.AreEnemiesDead() && combatHappening == true)
			{
				Debug.Log("Ending Combat!");
				//Set combat to false
				combatHappening = false;
				//Bring up the move on dialogue
				//DialogueManager.SetMoveOnButton("You defeated all of the enemies!");
				EventsManager.ChooseLoot();
				DialogueManager.SetUpNewDialogue(15);
			}

			EnemyManager.RemoveEnemiesFromScreen();
		}

		public void CalculateInitiative()
		{
			InitiativeContainer[] attackOrder;
			//Set initiative container array to amount of scene enemies plus 1 for player
			attackOrder = new InitiativeContainer[EnemyManager.sceneEnemies.Length + 1];
			Debug.Log("Created iniative array of length: " + attackOrder.Length);

			//Get player roll
			int playerRoll = Random.Range(1, 20) + PlayerManager.InitiativeBonus;
			Debug.Log("Player rolled: " + playerRoll + " initiative! With a +" + PlayerManager.InitiativeBonus + " bonus!");

			int i = 0;

			//Loop through all scene enemies and roll their initiative
			foreach(GameObject enemy in EnemyManager.sceneEnemies)
			{
				attackOrder[i] = new InitiativeContainer();
				//Assign the enemy to the array
				attackOrder[i].entity = enemy;
				//Roll enemy iniative
				attackOrder[i].entityRoll = Random.Range(1, 20) + enemy.GetComponent<EnemyStats>().iniativeBonus;
				Debug.Log(attackOrder[i].entity.name + " rolled: " + attackOrder[i].entityRoll + " initiative! With a +" + enemy.GetComponent<EnemyStats>().iniativeBonus + " bonus.");

				i++;
			}

			attackOrder[i] = new InitiativeContainer();

			//Assign player object to the array
			attackOrder[i].entity = PlayerManager.gameObject;
			attackOrder[i].entityRoll = playerRoll;

			//Sort the array and assign it to the globabl array
			turnOrder = attackOrder.OrderByDescending(x => x.entityRoll);

			Debug.Log("Sorted turn order:");
			foreach (InitiativeContainer item in turnOrder)
			{
				Debug.Log(item.entity.name + " with a " + item.entityRoll + ".");
				UIManager.ChangeOverheadText(item.entity, item.entityRoll.ToString(), 0);
			}
		}
	}

	//Class for containing the enemy or player and their roll
	public class InitiativeContainer
	{
		public GameObject entity;
		public int entityRoll;
	}
}
