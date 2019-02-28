using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
	public PlayerManager PlayerManager;

	public GameObject shopItem;
	public Image shopImage;
	public Text shopItemName;
	public Text shopItemCost;

	private void Update()
	{
	}

	public void GiveShopItem()
	{
		PlayerManager.GiveItem(shopItem);
	}

	public void SpendGold()
	{
		int spentGold;
		int.TryParse(shopItemCost.text, out spentGold);

		if(spentGold <= PlayerManager.PlayerGold)
			PlayerManager.PlayerGold -= spentGold;
	}
}
