using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	private PlayerManager PlayerManager;
	//public EnemyManager EnemyManager;
	private ShopManager ShopManager;

	public GameObject overheadTextPrefab;
	//public Slider healthBar;
	public GameObject[] sceneEnemies;
	public TextMeshProUGUI goldText;
	public Image weaponSlot;
	public Image headArmorSlot;
	public Text toolTip;

	public GraphicRaycaster hitInfo;

	#region Singleton
	private void Awake()
	{
		if(Instance != null)
		{
			Debug.LogWarning("More than one singleton!");
			return;
		}

		Instance = this;
	}
	#endregion

	void Start()
	{
		PlayerManager = PlayerManager.Instance;
		ShopManager = ShopManager.Instance;
	}

	void Update()
	{
		//healthBar.value = CalculateHealth();

		//CalculateAndDisplayEnemyUI();

		DisplayGold();

		UpdateInventory();

		UpdateToolTipPosition();
	}
	/*
	private float CalculateHealth()
	{
		if (PlayerManager.PlayerHP > 0)
		{
			return PlayerManager.PlayerHP / PlayerManager.PlayerMaxHP;
		}
		else
			return 0;//die
	}
	*/
	private void DisplayGold()
	{
		goldText.text = PlayerManager.PlayerGold.ToString();
	}

	private void UpdateInventory()
	{
		if(weaponSlot != null && PlayerManager.PlayerWeapon != null)
		{
			weaponSlot.GetComponent<Image>().sprite = PlayerManager.PlayerWeapon.GetComponent<SpriteRenderer>().sprite;

			EquipmentStats itemStats = PlayerManager.PlayerWeapon.GetComponent<EquipmentStats>();
			EquipmentStats slotStats = weaponSlot.GetComponent<EquipmentStats>();

			slotStats.itemName = itemStats.itemName;
			slotStats.rarity = itemStats.rarity;
			slotStats.isWeapon = itemStats.isWeapon;
			slotStats.diceSides = itemStats.diceSides;
			slotStats.diceRollAmount = itemStats.diceRollAmount;
			slotStats.qualityModifer = itemStats.qualityModifer;

		}

		if (headArmorSlot != null && PlayerManager.PlayerHeadArmor != null)
		{
			headArmorSlot.GetComponent<Image>().sprite = PlayerManager.PlayerHeadArmor.GetComponent<SpriteRenderer>().sprite;


			EquipmentStats itemStats =  PlayerManager.PlayerHeadArmor.GetComponent<EquipmentStats>();
			EquipmentStats slotStats =  headArmorSlot.GetComponent<EquipmentStats>();

			slotStats.itemName = itemStats.itemName;
			slotStats.rarity = itemStats.rarity;
			slotStats.isArmor = itemStats.isArmor;
			slotStats.armorValue = itemStats.armorValue;
			slotStats.qualityModifer = itemStats.qualityModifer;

		}
	}
/*
	private void CalculateAndDisplayEnemyUI()
	{
		sceneEnemies = EnemyManager.sceneEnemies;
		Slider enemyHealthBar;

		if (sceneEnemies != null)
			foreach (GameObject enemy in sceneEnemies)
			{
				enemyHealthBar = enemy.GetComponentInChildren<Slider>();

				//enemyHealthBar = enemy.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Slider>();

				enemyHealthBar.value = ( enemy.GetComponent<EnemyStats>().currentHP / enemy.GetComponent<EnemyStats>().maxHP );
			}
		
	}
*/
	private void UpdateToolTipPosition()
	{
		//toolTip.transform.position = Input.mousePosition;
	}

	public void ChangeOverheadText(GameObject entity, string text, int type = 0)
	{
		TextMeshProUGUI overHeadText = entity.GetComponentInChildren<TextMeshProUGUI>();

		if (overHeadText != null)
		{
			overHeadText.text = text;

			if (type == 0)
				overHeadText.faceColor = Color.white;
			else if (type == 1)
				overHeadText.faceColor = Color.red;
			else if (type == 2)
				overHeadText.faceColor = Color.green;

			overHeadText.gameObject.GetComponent<Animator>().Play("animate");
		}

	}

	public void CreateOverheadText(GameObject entity, string text, int type = 0)
	{
		if (text == "0")
			return;

		GameObject clone = Instantiate(overheadTextPrefab);

		TextMeshProUGUI overheadText = clone.GetComponentInChildren<TextMeshProUGUI>();

		clone.transform.position = entity.transform.position;

		overheadText.text = text;

		if (type == 0)
			overheadText.faceColor = Color.white;
		else if (type == 1)
			overheadText.faceColor = Color.red;
		else if (type == 2)
			overheadText.faceColor = Color.blue;
		else if (type == 3)
			overheadText.faceColor = Color.green;
		else if (type == 4)
			overheadText.faceColor = Color.magenta;
		else if (type == 5)
			overheadText.faceColor = Color.grey;

	}

	public void DisplayShopUI()
	{
		ShopManager.SetUpShop();
		ShopManager.gameObject.SetActive(true);
	}

	public void HideShopUI()
	{
		ShopManager.gameObject.SetActive(false);
	}


}

