using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

public class ShopButton : MonoBehaviour
{
	public PlayerManager PlayerManager;

	public GameObject shopItem;
	public Image shopImage;
	public Text shopItemName;
	public Text shopItemCost;

	public void GiveShopItem()
	{
		PlayerManager.GiveItem(shopItem);
	}
}
