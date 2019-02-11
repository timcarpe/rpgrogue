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

			/*
			buttonShopItem.GetChild(0).GetComponent<Image>().sprite = lootItem.GetComponent<SpriteRenderer>().sprite;
			child.GetChild(1).GetChild(0).GetComponent<Text>().text = lootItem.GetComponent<EquipmentStats>().itemName;
			child.GetChild(1).GetChild(1).GetComponent<Text>().text = lootItem.GetComponent<EquipmentStats>().itemCost.ToString();
			*/

			buttonShopItem.GetComponent<ShopButton>().shopImage.sprite = lootItem.GetComponent<SpriteRenderer>().sprite;
			buttonShopItem.GetComponent<ShopButton>().shopItemName.text = lootItem.GetComponent<EquipmentStats>().itemName;
			buttonShopItem.GetComponent<ShopButton>().shopItemCost.text = lootItem.GetComponent<EquipmentStats>().itemCost.ToString();
			buttonShopItem.GetComponent<ShopButton>().shopItem = lootItem;
		}
	}
	
}
