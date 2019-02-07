using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Player;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public DialogueManager DialogueManager;
	public PlayerManager PlayerManager;
	public EnemyManager EnemyManager;

	private bool playerDied = false;

	private void Start()
	{
		playerDied = false;
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
