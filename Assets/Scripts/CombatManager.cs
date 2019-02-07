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
		public EventsManager EventsManager;

		private bool combatHappening = false;

		private IEnumerable<InitiativeContainer> turnOrder;

		void Update()
		{

		}

		public int BasicAttack()
		{
			int damage = 0;

			for (int i = 0; i < PlayerManager.DiceRoll; i++)
				damage += Random.Range(1, PlayerManager.DiceType);

			return damage + PlayerManager.DamageBonus;
		}

		public void AttackPlayer(GameObject enemy)
		{
			int enemyDamageDone = 0;

			//Roll the damage from dice
			for (int i = 0; i < enemy.GetComponent<EnemyStats>().DiceRoll; i++)
				enemyDamageDone += Random.Range(1, enemy.GetComponent<EnemyStats>().DiceType);

			//Add any damage bonuses
			enemyDamageDone += enemy.GetComponent<EnemyStats>().AddDamage;

			enemyDamageDone -= PlayerManager.ArmorBonus;

			//Deal damage to the player
			PlayerManager.DamagePlayerHP(enemyDamageDone);

			DialogueManager.UpdateCombatText(enemy.GetComponent<EnemyStats>().EnemyName, enemyDamageDone);

			UIManager.ChangeOverheadText(PlayerManager.gameObject, enemyDamageDone.ToString(), 1);

			Debug.Log(enemy.name + " attacking player for " + enemyDamageDone + " damage!");
		}

		/*public void AttackEnemyWrapper(GameObject enemy)
		{
			AttackEnemy();
		}*/

		public void AttackEnemy(GameObject enemy)
		{
			//int rand;
			//bool hitEnemy = false;
			//Get the damage attack
			int damageDone = BasicAttack();

			//while(hitEnemy == false && EnemyManager.AreEnemiesDead() == false)
			if(EnemyManager.AreEnemiesDead() == false)
			{
				//Choose a random enemy
				//rand = Random.Range(0, EnemyManager.sceneEnemies.Length);

				//if(EnemyManager.sceneEnemies[rand].activeInHierarchy)
				//{
				//Subtract armor from the damage
				//damageDone -= EnemyManager.sceneEnemies[rand].GetComponent<EnemyStats>().EnemyArmor;
				damageDone -= enemy.GetComponent<EnemyStats>().EnemyArmor;
				//Deal damage to the enemy
				//EnemyManager.sceneEnemies[rand].GetComponent<EnemyStats>().CurrentHP -= damageDone;
				enemy.GetComponent<EnemyStats>().CurrentHP -= damageDone;

				//Exit the loop
				//hitEnemy = true;

				DialogueManager.UpdateCombatText("You", damageDone);
				//UIManager.ChangeOverheadText(EnemyManager.sceneEnemies[rand], damageDone.ToString(), 1);
				UIManager.ChangeOverheadText(enemy, damageDone.ToString(), 1);
				Debug.Log("Player attacking " + enemy.name + "  for " + damageDone + " damage!");
				//}
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

					while(choseEnemy == false)
					{
						if (Input.GetMouseButtonDown(0))
						{
							/*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
							RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
							Debug.Log(ray.origin + ray.direction);
							if (hit)
								if (hit.collider.gameObject.tag == "Enemy")
								{
									Debug.Log("Clicked on " + hit.collider.gameObject.name);
									choseEnemy = true;
									enemy = hit.collider.gameObject;
									AttackEnemy(enemy);
								}
								*/
							RaycastHit hitInfo = new RaycastHit();
							if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
							{
								enemy = hitInfo.collider.gameObject;

								if (enemy.CompareTag("Enemy"))
								{
									choseEnemy = true;
									AttackEnemy(enemy);
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
				attackOrder[i].entityRoll = Random.Range(1, 20) + enemy.GetComponent<EnemyStats>().IniativeBonus;
				Debug.Log(attackOrder[i].entity.name + " rolled: " + attackOrder[i].entityRoll + " initiative! With a +" + enemy.GetComponent<EnemyStats>().IniativeBonus + " bonus.");

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
