using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
	public EventsManager EventsManager;
	// Start is called before the first frame update

	void Start()
	{
		StartCoroutine(EventsManager.SceneStart());
	}

	// Update is called once per frame
	void Update()
	{

	}
}
