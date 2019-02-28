using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefab : MonoBehaviour
{
	public float lifetime;
	// Start is called before the first frame update
	private void Awake()
	{
		Destroy(gameObject, lifetime);
	}
}
