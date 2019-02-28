using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResistanceMonitor : MonoBehaviour
{
	public Image[] Images;
	private EnemyStats Enemy;
	//private PlayerManager PlayerManager;

	private void Start()
	{
		if (GetComponentInParent<EnemyStats>() != null)
		{
			Enemy = GetComponentInParent<EnemyStats>();

			foreach (Image image in Images)
				if (image.name == Enemy.resistantElement)
					image.enabled = true;
				else
					image.enabled = false;
		}
	}
}
