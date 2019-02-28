using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSettings : MonoBehaviour
{
	[SerializeField] private string spellType;
	[SerializeField] private int baseManaCost;
	[SerializeField] [Range(0, 1)] public float spellHitMultiplier;
	[SerializeField] [Range(0, 1)] public float spellAOEMultiplier;
	[SerializeField] [Range(0, 1)] public float spellDOTMultiplier;
	[SerializeField] private Sprite spellSprite;


	public Sprite GetSpellSprite()
	{
		return spellSprite;
	}
	public float GetManaCost()
	{
		return baseManaCost;
	}
	public string GetSpellType()
	{
		return spellType;
	}
	public float GetSpellHit()
	{
		return spellHitMultiplier;
	}
	public float GetSpellAOE()
	{
		return spellAOEMultiplier;
	}
	public float GetSpellDOT()
	{
		return spellDOTMultiplier;
	}
}
