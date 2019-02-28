using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private DialogueManager DialogueManager;
	private PlayerManager PlayerManager;
	//public EnemyManager EnemyManager;

	private bool playerDied = false;

	private void Start()
	{
		playerDied = false;
		PlayerManager = PlayerManager.Instance;
		DialogueManager = DialogueManager.Instance;
	}
	private void Update()
	{
		MonitorPlayerHealth();
	}

	private void MonitorPlayerHealth()
	{
		if (playerDied == false)
			if (PlayerManager.PlayerHP <= 0)
			{
				playerDied = true;
				DialogueManager.PlayerDiedText();
			}
	}

	public void EndGameLoadScene()
	{
		Debug.Log("Player has died!");
		SceneManager.LoadScene(0);
	}
}
