using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentStats : MonoBehaviour
{
	[Header("Item Details")]
	public string itemName;
	[Range(0, 5)] public int rarity;
	[Space]
	[Header("Armor")]
	public bool isArmor;
	public int armorValue;
	[Space]
	[Header("Weapon")]
	public bool isWeapon;
	[Range(0, 12)] public int diceSides;
	[Range(0, 4)] public int diceRollAmount;
	[Space]
	[Range(0, 4)] public int qualityModifer;
	[Space]
	[Header("Ring")]
	public int maxHPBonus;
	public int maxDamageBonus;
	[Space]
	public int itemCost;
}
