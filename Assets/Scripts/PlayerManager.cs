using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

namespace Player
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Objects")]
		public GameObject player;
		public GameObject playerModel;
		public GameObject playerWeaponContainer;
		public GameObject playerHeadArmorContainer;
		public GameObject playerRingContainer;
		public GameObject playerAmuletContainer;

		[Header("Health")]
		[SerializeField] private float playerMaxHP;
		[SerializeField] private float playerHP = -1;
		[SerializeField] private float maxHPBonus;

		[Header("Class")]
		[SerializeField] private string playerClass;

		[Header("Attributes")]
		[SerializeField] private int playerSTR = 16;
		[SerializeField] private int playerDEX = 14;
		[SerializeField] private int playerCON = 16;
		[SerializeField] private int playerINT = 12;
		[SerializeField] private int playerCHA = 10;
		[SerializeField] private int playerLUC = 10;

		[Header("Bonuses")]
		[SerializeField] private int strBonus;
		[SerializeField] private int dexBonus;
		[SerializeField] private int conBonus;
		[SerializeField] private int intBonus;
		[SerializeField] private int chaBonus;
		[SerializeField] private int lucBonus;

		[SerializeField] private int attackBonus;
		[SerializeField] private int armorBonus;
		[SerializeField] private int damageReduction;
		[SerializeField] private int initiativeBonus;

		[Header("Equipment")]

		[SerializeField] private GameObject playerHeadArmor;
		[SerializeField] private GameObject playerWeapon;
		[SerializeField] private GameObject playerRing1;
		[SerializeField] private GameObject playerRing2;
		[SerializeField] private GameObject playerAmulet;

		[SerializeField] private int diceType;
		[SerializeField] private int diceRoll;
		[SerializeField] private int damageBonus;

		private Vector3 defaultScale;

		[SerializeField] private GameObject lootItem;
		[SerializeField] private int playerGold;

		private List<GameObject> equipableArmor = new List<GameObject>();
		private List<GameObject> equipableWeapon = new List<GameObject>();
		private List<GameObject> equipableRing = new List<GameObject>();
		private List<GameObject> equipableAmulet = new List<GameObject>();

		private GameObject[] lootableItems;

		// Start is called before the first frame update
		void Start()
		{

			GenerateWeaponList();

			GenerateArmorList();

			GenerateRingList();

			GenerateLootableItemList();

			//Run through all of the methods for calculating bonuses one time before starting
			Update();

			SetPlayerHP();

			defaultScale = playerModel.transform.localScale;
		}

		// Update is called once per frame
		void Update()
		{
			//Constantly check for changes to the player head armor and weapon
			EquipPlayerHeadArmor();
			EquipPlayerWeapon();

			//Constantly check for changes to these attributes
			CalculateAttackBonus();
			CalculateArmorBonus();
			CalculateAttributeBonuses();
			CalculateIntiative();
			CalculateMaxHP();
			CalculateWeaponDamage();

			ChangePlayerSize();
		}

		private void EquipPlayerHeadArmor()
		{
			if (playerHeadArmor != null)
			{
				//Go through each object in the Head Armor container and disable or enable to equip
				foreach (Transform child in playerHeadArmorContainer.transform)
				{
					if (child.name == playerHeadArmor.name)
						child.gameObject.SetActive(true);
					else
						child.gameObject.SetActive(false);
				}
			}
		}

		private void EquipPlayerWeapon()
		{
			if (playerWeapon != null)
			{
				//Go through each object in the Weapon container and disable or enable to equip
				foreach (Transform child in playerWeaponContainer.transform)
				{
					if (child.name == playerWeapon.name)
						child.gameObject.SetActive(true);
					else
						child.gameObject.SetActive(false);
				}
			}
		}

		public void GiveItem()
		{
			//If item is in list of armor change the playerHeadArmor object to the lootItem
			foreach (GameObject child in equipableArmor)
			{
				if (lootItem != null && child == lootItem)
					playerHeadArmor = lootItem;
			}
			//If item is in list of weapons change the playerWeapon object to the lootItem
			foreach (GameObject child in equipableWeapon)
			{
				if (lootItem != null && child == lootItem)
					playerWeapon = lootItem;
			}

			foreach (GameObject child in equipableRing)
			{
				if (lootItem != null && child == lootItem)
					playerRing1 = lootItem;
			}

			foreach (GameObject child in equipableAmulet)
			{
				if (lootItem != null && child == lootItem)
					playerAmulet = lootItem;
			}
		}

		public void GiveItem(GameObject item)
		{
			//If item is in list of armor change the playerHeadArmor object to the lootItem
			foreach (GameObject child in equipableArmor)
			{
				if (item != null && child == item)
					playerHeadArmor = item;
			}
			//If item is in list of weapons change the playerWeapon object to the lootItem
			foreach (GameObject child in equipableWeapon)
			{
				if (item != null && child == item)
					playerWeapon = item;
			}

			foreach (GameObject child in equipableRing)
			{
				if (item != null && child == item)
					playerRing1 = item;
			}

			foreach (GameObject child in equipableAmulet)
			{
				if (item != null && child == item)
					playerAmulet = item;
			}
		}

		public void UpdatePlayerGold(int goldAmount)
		{
			playerGold += goldAmount;
		}

		public void DamagePlayerHP(int damage)
		{
			if (damage > 0)
			{
				if ((playerHP - damage) <= 0)
				{
					playerHP = 0;

					//Die
				}
				else
					playerHP -= damage;
			}
		}

		public void HealPlayerHP(int heal)
		{
			if ((playerHP + heal) > playerMaxHP)
				playerHP = playerMaxHP;
			else
				playerHP += heal;
		}
		
		//Get and Set methods for all of the attributes
		public float PlayerHP
		{
			get { return playerHP; }
			set { playerHP = value; }

		}
		public float PlayerMaxHP
		{
			get { return playerMaxHP; }
			set { playerMaxHP = value; }
		}
		public int AttackBonus
		{
			get { return attackBonus; }
			set { attackBonus = value; }
		}
		public int ArmorBonus
		{
			get { return armorBonus; }
			set { armorBonus = value; }
		}
		public int PlayerGold
		{
			get { return playerGold; }
			set { playerGold = value; }
		}
		public int DamageReduction
		{
			get { return damageReduction; }
			set { damageReduction = value; }
		}
		public int InitiativeBonus
		{
			get { return initiativeBonus; }
			set { initiativeBonus = value; }
		}
		public int DiceType
		{
			get { return diceType; }
			set { diceType = value; }
		}
		public int DiceRoll
		{
			get { return diceRoll; }
			set { diceRoll = value; }
		}
		public int DamageBonus
		{
			get { return damageBonus; }
			set { damageBonus = value; }
		}
		public GameObject LootItem
		{
			get { return lootItem; }
			set { lootItem = value; }
		}
		public GameObject[] LootableItems
		{
			get { return lootableItems; }
			set { lootableItems = value; }
		}
		public GameObject PlayerWeapon
		{
			get {
				if (playerWeapon != null)
					return playerWeapon;
				else return null;
			}
		}
		public GameObject PlayerHeadArmor
		{
			get
			{
				if (playerHeadArmor != null)
					return playerHeadArmor;
				else return null;
			}
		}
		public bool IsLucky()
		{
			float rand = Random.Range(-18f, 2f) + lucBonus;

			if (rand > 0)
				return true;
			else
				return false;
		}

		//Generate a list of armor that are attached to the player model. These will likely all be inactive.
		private void GenerateArmorList()
		{
			foreach (Transform child in playerHeadArmorContainer.transform)
			{
				equipableArmor.Add(child.gameObject);
			}
		}

		private void GenerateRingList()
		{
			foreach (Transform child in playerRingContainer.transform)
			{
				equipableRing.Add(child.gameObject);
			}
		}

		//Generate a list of weapons that are attached to the player model. These will likely all be inactive.
		private void GenerateWeaponList()
		{
			foreach (Transform child in playerWeaponContainer.transform)
			{
				equipableWeapon.Add(child.gameObject);
			}
		}

		private void GenerateLootableItemList()
		{
			List<GameObject> lootList = new List<GameObject>();

			lootableItems = equipableArmor.ToArray().Concat(equipableRing.ToArray()).Concat(equipableWeapon).ToArray().ToArray();

			Debug.Log("Generated a list of " + (lootableItems.Length - 1) + " lootable items.");
		}

		//Change the player size slightly based on the chosen attributes.
		private void ChangePlayerSize()
		{
			//This needs to be tweaked! Need to change only the head and body not the ui elements
			//playerModel.transform.localScale = defaultScale + new Vector3((strBonus + conBonus) / 15f, (conBonus + intBonus) / 15f, 0f);
		}

		private void CalculateAttackBonus()
		{
			if (playerWeapon != null)
				attackBonus = strBonus + playerWeapon.GetComponent<EquipmentStats>().qualityModifer;
			else
				attackBonus = strBonus;
		}

		private void CalculateArmorBonus()
		{
			if (playerHeadArmor != null)
				armorBonus = (dexBonus * 2) + playerHeadArmor.GetComponent<EquipmentStats>().armorValue + playerHeadArmor.GetComponent<EquipmentStats>().qualityModifer;
			else
				armorBonus = (dexBonus * 2);
		}
		
		private void CalculateIntiative()
		{
			initiativeBonus = dexBonus * 2;
		}

		private void CalculateAttributeBonuses()
		{
			strBonus = ((playerSTR - 10) / 2);
			dexBonus = ((playerDEX - 10) / 2);
			conBonus = ((playerCON - 10) / 2);
			intBonus = ((playerINT - 10) / 2);
			chaBonus = ((playerCHA - 10) / 2);
			lucBonus = ((playerLUC - 10) / 2);
		}

		private void CalculateMaxHP()
		{
			playerMaxHP = (30 + (conBonus * 5)) + maxHPBonus;

			if (playerRing1 != null)
			{
				playerMaxHP += playerRing1.GetComponent<EquipmentStats>().maxHPBonus;
			}
		}

		private void CalculateWeaponDamage()
		{
			if (playerWeapon != null)
			{
				diceType = playerWeapon.GetComponent<EquipmentStats>().diceSides;
				diceRoll = playerWeapon.GetComponent<EquipmentStats>().diceRollAmount;
				damageBonus = playerWeapon.GetComponent<EquipmentStats>().qualityModifer + ((int)strBonus*2);
			}

			if(playerRing1 != null)
			{
				damageBonus += playerRing1.GetComponent<EquipmentStats>().maxDamageBonus;
			}
		}

		public void changeMaxHPBonus(int amount)
		{
			//Change the max hp by amount including negative amounts
			maxHPBonus += amount;

			//However if the amount is greater than 0 increase playerHP by the same amount
			if(amount > 0)
				playerHP += amount;

		}
	

		//Set player HP first time
		private void SetPlayerHP()
		{
			playerHP = playerMaxHP;
		}

	}
}
