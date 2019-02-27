using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enemies;

namespace Spells
{
	public class SpellManager : MonoBehaviour
	{
		public SpellUIManager SpellUIManager;
		public EnemyManager EnemyManager;

		public GameObject fireSpellSettings;
		public GameObject waterSpellSettings;
		public GameObject natureSpellSettings;
		public GameObject arcaneSpellSettings;

		public GameObject fireParticles;

		public Transform castingLocation;

		private GameObject[] m_SpellList = new GameObject[4];
		private int m_SpellIndex = 0;

		private SpellInfo[] SpellInfoArray = new SpellInfo[4];
		private SpellSettings spellCastType;

		public static bool playerSpellCast;

		private int tempSpellPower = 10;

		private void Start()
		{
			Intitialize();
		}

		public void AddSpell(GameObject spell)
		{
			if (m_SpellIndex > 3)
				return;

			m_SpellList[m_SpellIndex] = spell;

			SpellUIManager.SetSpellImage(spell.GetComponent<SpellSettings>().GetSpellSprite());

			//if (m_SpellIndex == 0)
			//SetSpellActive(spell.GetComponent<SpellSettings>().GetSpellType());

			m_SpellIndex++;
		}

		public void RemoveSpell(GameObject spell)
		{
			m_SpellList[m_SpellIndex] = null;

			SpellUIManager.ResetLastSpellImage();

			//if (m_SpellIndex == 1)
			//SetSpellDeactive(spell.GetComponent<SpellSettings>().GetSpellType());

			m_SpellIndex--;
		}

		public void CastSpell()
		{
			string spell = "";
			SpellSettings spellType = m_SpellList[0].GetComponent<SpellSettings>();

			foreach (GameObject spellObject in m_SpellList)
			{
				if (spellObject != null)
					spell += (spellObject.GetComponent<SpellSettings>().GetSpellType() + " ");
			}

			SetSpellCastType(spellType);

			foreach (GameObject spellObject in m_SpellList.Skip(1))
			{
				if (spellObject != null)
					AddSpell(spellObject.GetComponent<SpellSettings>().GetSpellType(), tempSpellPower);
			}

			Debug.Log("Casting spell of type " + spell);

			DebugSpell();
			DealDamage();
			PlaySpellAnimation(GetSpellCastType());
			Reset();
			SpellUIManager.ResetAllSpellImages();

			//GameObject spellCast = Instantiate(fireSpell) as GameObject;
			//spellCast.transform.position = castingLocation.position;
			//Rigidbody rb = spellCast.GetComponentInChildren<Rigidbody>();
			//rb.velocity = Camera.main.transform.forward * 10;

			m_SpellIndex = 0;

			//StartCoroutine(PlaySpellAnimation(spellType));
			
			ResetSpellList();
		}

		private void ResetSpellList()
		{
			for (int i = 0; i <= 3; i++)
			{
				m_SpellList[i] = null;
			}
		}

		private void PlaySpellAnimation(string spellType)
		{
			switch (spellType)
			{
				case "Fire":
					if (EnemyManager.GetFrontEnemy() != null)
					{
						GameObject spellCast = Instantiate(fireParticles);
						spellCast.transform.position = EnemyManager.GetFrontEnemy().transform.position;
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

		public void AddSpell(string type, int spellPower)
		{
			int hit = (int)(spellCastType.GetSpellHit() * spellPower);
			int aOE = (int)(spellCastType.GetSpellAOE() * spellPower);
			int dOT = (int)(spellCastType.GetSpellDOT() * spellPower);

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

		public void DealDamage()
		{
			switch (spellCastType.GetSpellType())
			{
				case "Fire":
					EnemyManager.DealSplashDamage(EnemyManager.GetFrontEnemy(), 5, 1,"Fire");
					EnemyManager.DealDamageToEnemy(EnemyManager.GetFrontEnemy(), 15, "Fire");
					break;

				case "Water":
					EnemyManager.DealDamageToAllEnemies(5, "Water");
					break;

				case "Nature":
					EnemyManager.DealSplashDamage(EnemyManager.GetLastEnemy(), 5, 10, "Nature");
					EnemyManager.DealDamageToEnemy(EnemyManager.GetLastEnemy(), 15, "Nature");
					break;

				default: break;
			}
			playerSpellCast = true;
		}

		public void DebugSpell()
		{
			Debug.Log("Fire: Hit: " + SpellInfoArray[0].GetSpellHit() + " AOE: " + SpellInfoArray[0].GetSpellAOE() + " DOT: " + SpellInfoArray[0].GetSpellDOT());
			Debug.Log("Water: Hit: " + SpellInfoArray[1].GetSpellHit() + " AOE: " + SpellInfoArray[1].GetSpellAOE() + " DOT: " + SpellInfoArray[1].GetSpellDOT());
			Debug.Log("Nature: Hit: " + SpellInfoArray[2].GetSpellHit() + " AOE: " + SpellInfoArray[2].GetSpellAOE() + " DOT: " + SpellInfoArray[2].GetSpellDOT());
			Debug.Log("Arcane: Hit: " + SpellInfoArray[3].GetSpellHit() + " AOE: " + SpellInfoArray[3].GetSpellAOE() + " DOT: " + SpellInfoArray[3].GetSpellDOT());
		}

		public void SetSpellCastType(SpellSettings spell)
		{
			Debug.Log("Setting master spell type to: " + spell.GetSpellType());
			spellCastType = spell;
		}

		public string GetSpellCastType()
		{
			return spellCastType.GetSpellType();
		}

		public void Reset()
		{
			spellCastType = null;

			foreach (SpellInfo spell in SpellInfoArray)
			{
				spell.Reset();
			}
		}

	}
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

}


