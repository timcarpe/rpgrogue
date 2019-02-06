using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	[Header("Enemy Details")]
	public string EnemyName;					//Clean version of enemy name
	public float MaxHP;							//Maximum default hitpoints of enemy
	public float CurrentHP;
	public int EnemyArmor;
	public int IniativeBonus;

	[Header("Monster Attack")]
	[Range(0, 20)] public int DiceType;
	[Range(0, 10)] public int DiceRoll;
	[Range(0, 10)] public int AddDamage;
}
