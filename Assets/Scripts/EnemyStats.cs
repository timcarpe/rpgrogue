using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	[Header("Enemy Details")]
	public string enemyName;					//Clean version of enemy name
	public float maxHP;							//Maximum default hitpoints of enemy
	public float currentHP;
	public string weakElement;
	public string resistantElement;
	public int enemyArmor;
	public int damageReduction;
	public int iniativeBonus;

	[Header("Monster Attack")]
	[Range(0, 10)] public int attackBonus;
	[Range(0, 20)] public int diceType;
	[Range(0, 10)] public int diceRoll;
	[Range(0, 10)] public int addDamage;

	/*public void OnMouseEnter()
	{
		transform.Find("Hover").gameObject.SetActive(true);
	}

	public void OnMouseExit()
	{
		transform.Find("Hover").gameObject.SetActive(false);
	}*/
}
