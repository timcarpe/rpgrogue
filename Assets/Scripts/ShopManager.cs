using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ShopManager : MonoBehaviour
{
	public PlayerManager PlayerManager;

	private GameObject[] lootableItems;

	private GameObject itemsContainer;

	private GameObject[] buttonsArray;


	private void Update()
	{
		foreach (Transform child in itemsContainer.transform)
		{
			int cost;
			int.TryParse(child.GetComponent<ShopButton>().shopItemCost.text, out cost);

			if (cost > PlayerManager.PlayerGold)
			{
				child.GetComponent<Button>().interactable = false;
			}

		}
	}

	public void SetUpShop()
	{
		lootableItems = PlayerManager.LootableItems;

		itemsContainer = gameObject.transform.Find("ItemsContainer").gameObject;

		GameObject lootItem;

		Transform buttonShopItem;

		foreach (Transform child in itemsContainer.transform)
		{
			lootItem = lootableItems[Random.Range(0, lootableItems.Length)];

			buttonShopItem = child;

			Debug.Log("Setting shop " + child.name + " to " + lootItem.name);

			buttonShopItem.GetComponent<ShopButton>().shopImage.sprite = lootItem.GetComponent<SpriteRenderer>().sprite;
			buttonShopItem.GetComponent<ShopButton>().shopItemName.text = lootItem.GetComponent<EquipmentStats>().itemName;
			buttonShopItem.GetComponent<ShopButton>().shopItemCost.text = lootItem.GetComponent<EquipmentStats>().itemCost.ToString();
			buttonShopItem.GetComponent<ShopButton>().shopItem = lootItem;

			int cost = lootItem.GetComponent<EquipmentStats>().itemCost;

			if (cost > PlayerManager.PlayerGold)
			{
				buttonShopItem.GetComponent<Button>().interactable = false;
			}
		}
	}
	
}
