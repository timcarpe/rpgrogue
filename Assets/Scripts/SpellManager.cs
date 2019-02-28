using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MEC;

public class SpellManager : MonoBehaviour
{
	//singleton
	public static SpellManager Instance;
	
	//Dependencies
	private SpellUIManager SpellUIManager;
	private EnemyManager EnemyManager;
	private PlayerManager PlayerManager;

	//Settings objects
	public GameObject fireSpellSettings;
	public GameObject waterSpellSettings;
	public GameObject natureSpellSettings;
	public GameObject arcaneSpellSettings;

	//Particles objects
	public GameObject fireParticles;

	public Transform castingLocation;

	//Spell index of player buttons
	private GameObject[] SpellList = new GameObject[4];
	private int spellIndex = 0;

	//Array for calculating the total spelldamage
	private SpellInfo[] SpellInfoArray = new SpellInfo[4];
	private SpellSettings spellCastContainerSpell;

	//Has player cast spell?
	public static bool playerSpellCast;

	[SerializeField] private int currentManaCost = 0;
	private float manaMultiplier = 0.3f;

	//Player spellpower
	//private int tempSpellPower = 10;

	#region Singleton
	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("More than one singleton!");
			return;
		}

		Instance = this;
	}
	#endregion

	private void Start()
	{
		//Intitialize singletons
		EnemyManager = EnemyManager.Instance;
		PlayerManager = PlayerManager.Instance;
		SpellUIManager = SpellUIManager.Instance;

		//Intialize the array
		Intitialize();
	}

	//Add spell to the spell list array
	public void AddSpellToUI(GameObject spell)
	{
		if (spellIndex > 3)
			return;

		if (currentManaCost + ((spellIndex) * manaMultiplier) * spell.GetComponent<SpellSettings>().GetManaCost() > PlayerManager.PlayerMana)
		{
			Debug.Log("Can't add spell, too much mana cost!");
		}
		else
		{
			currentManaCost += (int)(((spellIndex) * manaMultiplier) * spell.GetComponent<SpellSettings>().GetManaCost());
		}

		SpellList[spellIndex] = spell;

		SpellUIManager.SetSpellImage(spell.GetComponent<SpellSettings>().GetSpellSprite());

		//if (spellIndex == 0)
		//SetSpellActive(spell.GetComponent<SpellSettings>().GetSpellType());

		spellIndex++;
	}

	//Remove last spell from the array
	public void RemoveSpellFromUI(GameObject spell)
	{
		SpellList[spellIndex] = null;

		SpellUIManager.ResetLastSpellImage();

		//if (spellIndex == 1)
		//SetSpellDeactive(spell.GetComponent<SpellSettings>().GetSpellType());

		spellIndex--;
	}

	//Cast the spell and calculate the total spell damager
	public void CastSpell()
	{
		if (spellIndex == 0)
			return;

		string spell = "";
		SpellSettings spellContainerType = SpellList[0].GetComponent<SpellSettings>();

		foreach (GameObject spellObject in SpellList)
		{
			if (spellObject != null)
				spell += (spellObject.GetComponent<SpellSettings>().GetSpellType() + " ");
		}

		//Set spell container type
		SetSpellCastContainerSpellType(spellContainerType);

		//Skip the container type spell and add the spell to the list
		foreach (GameObject spellObject in SpellList.Skip(1))
		{
			if (spellObject != null)
				AddSpell(spellObject.GetComponent<SpellSettings>().GetSpellType(), PlayerManager.PlayerSpellPower);
		}

		//Debug.Log("Casting spell of type " + spell);

		DebugSpell();

		Timing.RunCoroutine(_DealDamage());

		PlaySpellAnimation(GetSpellCastContainerSpellType());

		PlayerManager.ReducePlayerMana(currentManaCost);

		

	}


	private void PlaySpellAnimation(string spellType)
	{
		switch (spellType)
		{
			case "Fire":
				//Fire a firball at the front enemy
				if (EnemyManager.GetFrontEnemy() != null)
				{
					GameObject spellCast = Instantiate(fireParticles);
					spellCast.transform.position = EnemyManager.GetFrontEnemy().transform.position;
				}
				//Fire a fireball at the splash enemy that is next to the front enemy
				if (EnemyManager.GetNextToFrontEnemy() != null)
				{
					GameObject spellCast = Instantiate(fireParticles);
					spellCast.transform.position = EnemyManager.GetNextToFrontEnemy().transform.position;
				}
				//fireSpell.GetComponent<Animator>().Play("FireSpell");
				break;

			case "Water":
				//waterSpell.GetComponent<Animator>().Play("WaterSpell");
				break;

			case "Nature":
				//natureSpell.GetComponent<Animator>().Play("NatureSpell");
				break;

			case "Arcane":
				//arcaneSpell.GetComponent<Animator>().Play("ArcaneSpell");
				break;

			default:
				break;
		}


		//SetSpellDeactive(type);

	}

	//Initialize the array
	public void Intitialize()
	{
		for (int i = 0; i < 4; i++)
		{
			SpellInfoArray[i] = new SpellInfo();
		}

		SpellInfoArray[0].SetSpellType("Fire");
		SpellInfoArray[1].SetSpellType("Water");
		SpellInfoArray[2].SetSpellType("Nature");
		SpellInfoArray[3].SetSpellType("Arcane");

		foreach (SpellInfo spell in SpellInfoArray)
		{
			Debug.Log("Initializing spell array " + spell.GetSpellType());
		}
	}

	//Calculate spell damage and add it to the spell info array for calculating total spell damage
	public void AddSpell(string type, int spellPower)
	{
		//Calculate damage by multiplying a percent by the spell power of the player
		int hit = (int)(spellCastContainerSpell.GetSpellHit() * spellPower);
		int aOE = (int)(spellCastContainerSpell.GetSpellAOE() * spellPower);
		int dOT = (int)(spellCastContainerSpell.GetSpellDOT() * spellPower);

		switch (type)
		{
			case "Fire":
				SpellInfoArray[0].SetSpellHit(hit);
				SpellInfoArray[0].SetSpellAOE(aOE);
				SpellInfoArray[0].SetSpellDOT(dOT);
				break;

			case "Water":
				SpellInfoArray[1].SetSpellHit(hit);
				SpellInfoArray[1].SetSpellAOE(aOE);
				SpellInfoArray[1].SetSpellDOT(dOT);
				break;

			case "Nature":
				SpellInfoArray[2].SetSpellHit(hit);
				SpellInfoArray[2].SetSpellAOE(aOE);
				SpellInfoArray[2].SetSpellDOT(dOT);
				break;

			case "Arcane":
				SpellInfoArray[3].SetSpellHit(hit);
				SpellInfoArray[3].SetSpellAOE(aOE);
				SpellInfoArray[3].SetSpellDOT(dOT);
				break;

			default: break;
		}

	}

	public SpellInfo GetFire()
	{
		return SpellInfoArray[0];
	}

	public SpellInfo GetWater()
	{
		return SpellInfoArray[1];
	}

	public SpellInfo GetNature()
	{
		return SpellInfoArray[2];
	}

	public SpellInfo GetArcane()
	{
		return SpellInfoArray[3];
	}

	//Switch style of spell and deal damage for each type
	IEnumerator<float> _DealDamage()
	{
			Debug.Log("Casting a spell of type " + spellCastContainerSpell.GetSpellType());

			switch (spellCastContainerSpell.GetSpellType())
			{
				case "Fire":
					foreach (SpellInfo spell in SpellInfoArray)
					{
						yield return Timing.WaitForSeconds(0.1f);
						//Fire spell hits the front enemy and then splashes to the next enemy for each element
						EnemyManager.DealSplashDamage(EnemyManager.GetFrontEnemy(), spell.GetSpellAOE(), 1, spell.GetSpellType());
						EnemyManager.DealDamageToEnemy(EnemyManager.GetFrontEnemy(), spell.GetSpellHit(), spell.GetSpellType());
					}
					break;

				case "Water":
					foreach (SpellInfo spell in SpellInfoArray)
					{
						yield return Timing.WaitForSeconds(0.1f);
						//Water spell does damage to all enemies for each element type
						EnemyManager.DealDamageToAllEnemies(spell.GetSpellAOE(), spell.GetSpellType());
					}
					break;

				case "Nature":
					foreach (SpellInfo spell in SpellInfoArray)
					{
						yield return Timing.WaitForSeconds(0.1f);

						//Nature spell does hit damage to the last enemy and then splash damage to all enenmies for each element
						//Also applies a DOT to all enemies
						EnemyManager.DealSplashDamage(EnemyManager.GetLastEnemy(), spell.GetSpellAOE(), 10, spell.GetSpellType());
						EnemyManager.DealDamageToEnemy(EnemyManager.GetLastEnemy(), spell.GetSpellHit(), spell.GetSpellType());
						EnemyManager.AddDOTDamageToAllEnemies(spell.GetSpellType(), spell.GetSpellDOT());
					}
					break;

				default: break;
			}

		playerSpellCast = true;

		Reset();
	}

	public void DebugSpell()
	{
		Debug.Log("Fire: Hit: " + SpellInfoArray[0].GetSpellHit() + " AOE: " + SpellInfoArray[0].GetSpellAOE() + " DOT: " + SpellInfoArray[0].GetSpellDOT());
		Debug.Log("Water: Hit: " + SpellInfoArray[1].GetSpellHit() + " AOE: " + SpellInfoArray[1].GetSpellAOE() + " DOT: " + SpellInfoArray[1].GetSpellDOT());
		Debug.Log("Nature: Hit: " + SpellInfoArray[2].GetSpellHit() + " AOE: " + SpellInfoArray[2].GetSpellAOE() + " DOT: " + SpellInfoArray[2].GetSpellDOT());
		Debug.Log("Arcane: Hit: " + SpellInfoArray[3].GetSpellHit() + " AOE: " + SpellInfoArray[3].GetSpellAOE() + " DOT: " + SpellInfoArray[3].GetSpellDOT());
	}

	public void SetSpellCastContainerSpellType(SpellSettings spell)
	{
		Debug.Log("Setting master spell type to: " + spell.GetSpellType());
		spellCastContainerSpell = spell;
	}

	public string GetSpellCastContainerSpellType()
	{
		return spellCastContainerSpell.GetSpellType();
	}

	public void Reset()
	{
		spellCastContainerSpell = null;

		foreach (SpellInfo spell in SpellInfoArray)
		{
			spell.Reset();
		}

		for (int i = 0; i <= 3; i++)
		{
			SpellList[i] = null;
		}

		currentManaCost = 0;

		SpellUIManager.ResetAllSpellImages();

		spellIndex = 0;

	}

}

//Container class for holding accumulative spell info for casting the master spell
public class SpellInfo
{
	private string type;
	private int hit;
	private int aOE;
	private int dOT;
	private int shield;

	public void SetSpellType(string spellType)
	{
		type = spellType;
	}
	public string GetSpellType()
	{
		return type;
	}
	public void SetSpellHit(int damage)
	{
		hit += damage;
	}
	public int GetSpellHit()
	{
		return hit;
	}
	public void SetSpellAOE(int damage)
	{
		aOE += damage;
	}
	public int GetSpellAOE()
	{
		return aOE;
	}
	public void SetSpellDOT(int damage)
	{
		dOT += damage;
	}
	public int GetSpellDOT()
	{
		return dOT;
	}
	public void SetSpellShield(int damage)
	{
		shield += damage;
	}

	public void Reset()
	{
		hit = 0;
		aOE = 0;
		dOT = 0;
		shield = 0;
	}

}



